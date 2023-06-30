using Application;
using Application.Common;
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
            string environmentLabel = Environment.GetEnvironmentVariable(ConfigurationConstants.Environment);
            string connectionString = Environment.GetEnvironmentVariable(ConfigurationConstants.AppConfConnectionString);
            string tenantId = Environment.GetEnvironmentVariable(ConfigurationConstants.TenantId);
            string clientId = Environment.GetEnvironmentVariable(ConfigurationConstants.ClientId);
            string clientSecret = Environment.GetEnvironmentVariable(ConfigurationConstants.ClientSecret);

            ClientSecretCredential credential = new(tenantId, clientId, clientSecret);

            IConfiguration config = builder.ConfigurationBuilder
                .AddAzureAppConfiguration(options =>
                {
                    options.Connect(connectionString)
                    .Select(ConfigurationConstants.DirectoryName, environmentLabel)
                    .Select(ConfigurationConstants.FileShareName, environmentLabel)
                    .Select(ConfigurationConstants.FileShareConnection, environmentLabel)
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
