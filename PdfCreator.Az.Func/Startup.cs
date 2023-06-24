using Application;
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
            IConfiguration config = builder.ConfigurationBuilder
                .AddAzureAppConfiguration(options =>
                {
                    options.Connect(connectionString);
                    options.Select("IronPdfLicense", environmentLabel);
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
