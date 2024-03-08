using Aes256CryptoWithAwsKms.Models;
using Aes256CryptoWithAwsKms.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aes256CryptoWithAwsKms.Controllers
{
	[Route("api/crypto/aes")]
	[ApiController]
	public class AesCryptoController : ControllerBase
	{
		private readonly IAesCryptoService _aesCryptoService;
		public AesCryptoController(
				IAesCryptoService cryptoService
			)
		{
			_aesCryptoService = cryptoService;
		}

		[HttpPost(nameof(Encrypt))]
		[ProducesResponseType(typeof(AesEncryptResponse), 200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> Encrypt([FromBody] AesEncryptRequest request)
		{
			if (ModelState.IsValid)
			{
				var response = await _aesCryptoService.EncryptAsync(request);
				return Ok(response);
			}
			return BadRequest();
		}

		[HttpPost(nameof(Decrypt))]
		[ProducesResponseType(typeof(AesDecryptResponse), 200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> Decrypt([FromBody] AesDecryptRequest request)
		{
			if (ModelState.IsValid)
			{
				var response = await _aesCryptoService.DecryptAsync(request);
				return Ok(response);
			}
			return BadRequest();
		}
	}
}
