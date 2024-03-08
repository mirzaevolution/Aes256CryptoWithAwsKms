
using Aes256CryptoWithAwsKms.Services;
using Amazon.KeyManagementService;

namespace Aes256CryptoWithAwsKms
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var services = builder.Services;

			services.AddControllers();
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			services.AddScoped<IAesCryptoService, AesCryptoService>();
			services.AddAWSService<IAmazonKeyManagementService>(builder.Configuration.GetAWSOptions());
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}