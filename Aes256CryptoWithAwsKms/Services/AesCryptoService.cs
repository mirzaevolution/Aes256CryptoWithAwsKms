using Aes256CryptoWithAwsKms.Models;
using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using System.Net;
using System.Text;

namespace Aes256CryptoWithAwsKms.Services
{
	public class AesCryptoService : IAesCryptoService
	{
		private readonly IAmazonKeyManagementService _amazonKeyManagementService;
		private readonly string _keyConfigSelector = "AesCustomKeyId";
		private readonly IConfiguration _configuration;
		private readonly ILogger<AesCryptoService> _logger;

		public AesCryptoService(
				IAmazonKeyManagementService amazonKeyManagementService,
				IConfiguration configuration,
				ILogger<AesCryptoService> logger
			)
		{
			_amazonKeyManagementService = amazonKeyManagementService;
			_configuration = configuration;
			_logger = logger;

		}

		public async Task<AesEncryptResponse> EncryptAsync(AesEncryptRequest request)
		{
			byte[] plainBytes = Encoding.UTF8.GetBytes(request.PlainText);
			using MemoryStream plainStream = new MemoryStream();
			plainStream.Write(plainBytes);
			plainStream.Seek(0, SeekOrigin.Begin);

			EncryptResponse encryptResponse = await _amazonKeyManagementService.EncryptAsync(new EncryptRequest
			{
				KeyId = _configuration[_keyConfigSelector] ?? throw new NullReferenceException("KMS Key not found"),
				Plaintext = plainStream ?? throw new NullReferenceException("Plain-text stream is null")
			});
			if (encryptResponse != null && encryptResponse.HttpStatusCode == HttpStatusCode.OK)
			{
				_logger.LogInformation($"Encryption response: {encryptResponse.HttpStatusCode.ToString()}");
				byte[] cipherBytes = encryptResponse.CiphertextBlob.ToArray();
				return new AesEncryptResponse
				{
					CipherText = Convert.ToBase64String(cipherBytes)
				};
			}

			throw new Exception($"Encryption process got an error from server");
		}

		public async Task<AesDecryptResponse> DecryptAsync(AesDecryptRequest request)
		{
			byte[] cipherBytes = Convert.FromBase64String(request.CipherText);
			using MemoryStream cipherStream = new MemoryStream();
			cipherStream.Write(cipherBytes);
			cipherStream.Seek(0, SeekOrigin.Begin);
			DecryptResponse decryptResponse =
				await _amazonKeyManagementService.DecryptAsync(new DecryptRequest
				{
					KeyId = _configuration[_keyConfigSelector] ?? throw new NullReferenceException("KMS Key not found"),
					CiphertextBlob = cipherStream
				});
			if (decryptResponse != null && decryptResponse.HttpStatusCode == HttpStatusCode.OK)
			{
				_logger.LogInformation($"Decryption response: {decryptResponse.HttpStatusCode.ToString()}");

				byte[] plainBytes = decryptResponse.Plaintext.ToArray();
				return new AesDecryptResponse
				{
					PlainText = Encoding.UTF8.GetString(plainBytes)
				};
			}
			throw new Exception($"Decryption process got an error from server");

		}


	}
}
