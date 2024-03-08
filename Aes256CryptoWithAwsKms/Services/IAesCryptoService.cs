using Aes256CryptoWithAwsKms.Models;

namespace Aes256CryptoWithAwsKms.Services
{
	public interface IAesCryptoService
	{
		Task<AesEncryptResponse> EncryptAsync(AesEncryptRequest request);
		Task<AesDecryptResponse> DecryptAsync(AesDecryptRequest request);
	}
}
