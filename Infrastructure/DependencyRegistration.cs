﻿using Application.Common.Interfaces;
using Infrastructure.Files;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IPdfFileBuilder, PdfFileBuilder>();
            services.AddTransient<IFileShareService, FileShareService>();

            return services;
        }
    }
}
