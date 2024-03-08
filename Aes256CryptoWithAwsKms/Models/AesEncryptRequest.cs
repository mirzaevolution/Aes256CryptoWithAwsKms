using System.ComponentModel.DataAnnotations;

namespace Aes256CryptoWithAwsKms.Models
{
	public class AesEncryptRequest
	{
		[Required]
		public string PlainText { get; set; }
	}
}
