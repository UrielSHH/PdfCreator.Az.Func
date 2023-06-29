using Application;
using Azure.Identity;
using Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;

[assembly: FunctionsStartup(typeof(PdfCreator.Az.Func.Startup))]
namespace PdfCreator.Az.Func
{
    public class Startup : FunctionsStartup
    {
        private IConfiguration _configuration;
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            string environmentLabel = Environment.GetEnvironmentVariable("Environment");
            string connectionString = Environment.GetEnvironmentVariable("AppConfConnectionString");
            string tenantId = Environment.GetEnvironmentVariable("TenantId");
            string clientId = Environment.GetEnvironmentVariable("ClientId");
            string clientSecret = Environment.GetEnvironmentVariable("ClientSecret");

            ClientSecretCredential credential = new(tenantId, clientId, clientSecret);

            IConfiguration config = builder.ConfigurationBuilder
                .AddAzureAppConfiguration(options =>
                {
                    options.Connect(connectionString);
                    options.Select("app.storage.connection.key1", environmentLabel)
                    .ConfigureKeyVault(kv =>
                    {
                        kv.SetCredential(credential);
                    });
                })
                .Build();
            _configuration = config;
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(_configuration);
        }
    }
}
