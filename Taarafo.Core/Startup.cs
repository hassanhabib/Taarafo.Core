// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Taarafo.Core.Brokers.DateTimes;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.Configurations;
using Taarafo.Core.Services.Foundations.Posts;

namespace Taarafo.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            LocalConfigurations localConfigurations = Configuration.Get<LocalConfigurations>();
            services.AddScoped<LocalConfigurations>(sp => localConfigurations);

            services.AddLogging();
            services.AddControllers();
            services.AddDbContext<StorageBroker>();
            AddBrokers(services);
            AddServices(services);
            ConfigureOpenAPIDocument(services, localConfigurations);
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            LocalConfigurations localConfigurations)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                IEnumerable<OpenAPIConfiguration> openAPIConfigurations = localConfigurations.OpenAPIs;

                app.UseSwaggerUI(options =>
                {
                    foreach (var openAPIConfiguration in openAPIConfigurations)
                    {
                        options.SwaggerEndpoint(
                            url: openAPIConfiguration.OpenAPIEndpoint.Url,
                            name: openAPIConfiguration.OpenAPIEndpoint.Name);
                    }
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private static void AddServices(IServiceCollection services) =>
            services.AddTransient<IPostService, PostService>();

        private static void AddBrokers(IServiceCollection services)
        {
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
        }

        private static void ConfigureOpenAPIDocument(
            IServiceCollection services,
            LocalConfigurations localConfigurations)
        {
            IEnumerable<OpenAPIConfiguration> openAPIConfigurations = localConfigurations.OpenAPIs;

            foreach (OpenAPIConfiguration openAPIConfiguration in openAPIConfigurations)
            {
                string version = openAPIConfiguration.Version;
                OpenAPIDocumentConfiguration document = openAPIConfiguration.Document;
                services.AddSwaggerGen(options =>
                {
                    OpenApiInfo openApiInfo = new OpenApiInfo
                    {
                        Title = document.Title,
                        Version = version,
                        Description = document.Description,
                        TermsOfService = new Uri(document.TermsOfService),
                        Contact = new OpenApiContact
                        {
                            Name = document.ContactName,
                            Email = document.ContactEmail,
                            Url = new Uri(document.ContactUrl),
                        },
                        License = new OpenApiLicense
                        {
                            Name = document.LicenseName,
                            Url = new Uri(document.LicenseUrl),
                        }
                    };

                    options.SwaggerDoc(name: version, info: openApiInfo);

                    options.OperationFilter<SecurityRequirementsOperationFilter>();
                });
            }
        }
    }
}
