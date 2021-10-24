// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
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
                OpenAPIConfiguration openAPIConfiguration = localConfigurations.OpenAPI;
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(
                        url: openAPIConfiguration.OpenAPIEndpoint.Url,
                        name: openAPIConfiguration.OpenAPIEndpoint.Name);
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
            OpenAPIDocumentConfiguration openAPIDocumentConfiguration = localConfigurations.OpenAPI.Document;

            services.AddSwaggerGen(options =>
            {
                OpenApiInfo openApiInfo = new OpenApiInfo
                {
                    Title = openAPIDocumentConfiguration.Title,
                    Version = openAPIDocumentConfiguration.Version,
                    Description = openAPIDocumentConfiguration.Description,
                    TermsOfService = new Uri(openAPIDocumentConfiguration.TermsOfService),
                    Contact = new OpenApiContact
                    {
                        Name = openAPIDocumentConfiguration.ContactName,
                        Email = openAPIDocumentConfiguration.ContactEmail,
                        Url = new Uri(openAPIDocumentConfiguration.ContactUrl),
                    },
                    License = new OpenApiLicense
                    {
                        Name = openAPIDocumentConfiguration.LicenseName,
                        Url = new Uri(openAPIDocumentConfiguration.LicenseUrl),
                    }
                };

                options.SwaggerDoc(name: openAPIDocumentConfiguration.Version, info: openApiInfo);

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }
    }
}
