using BankIssueRegApi.Helpers;
using BankIssueRegApi.Infrastructure.Contracts;
using BankIssueRegApi.Infrastructure.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Extensions
{
    public static class ApplicationServiceExtensions
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IBankProblemService, BankProblemService>();
            services.AddScoped<IQueryService, QueryService>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            // services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
            return services;
        }

    }
}
