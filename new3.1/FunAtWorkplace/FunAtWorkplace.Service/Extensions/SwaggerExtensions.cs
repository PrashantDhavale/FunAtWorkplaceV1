using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;

namespace FunAtWorkplace.Service.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllParametersInCamelCase();
                options.SwaggerDoc(configuration["Swagger:ApiVersion"], new OpenApiInfo
                {
                    Version = configuration["Swagger:ApiVersion"],
                    Title = configuration["Swagger:ApiTitle"],
                    TermsOfService = new Uri(configuration["Swagger:ApiTermsOfServiceUrl"]),
                    Contact = new OpenApiContact
                    {
                        Name = configuration["Swagger:ApiContactName"],
                        Email = configuration["Swagger:ApiContactEmail"]
                    },
                    License = new OpenApiLicense
                    {
                        Name = configuration["Swagger:ApiLicense:Name"],
                        Url = new Uri(configuration["Swagger:ApiLicense:Url"]),
                    }
                });
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //options.IncludeXmlComments(xmlPath);
            });
            return services;
        }

        public static IApplicationBuilder UseSwaggerExtension(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/{configuration["Swagger:ApiVersion"]}/swagger.json", $"{configuration["Swagger:ApiTitle"]} - {configuration["Swagger:ApiVersion"]}");
                options.DocExpansion(DocExpansion.None);
                options.DisplayRequestDuration();
                options.RoutePrefix = string.Empty;
            });
            return app;
        }
    }
}