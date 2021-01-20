using FunAtWorkplace.Service.Extensions;
using FunAtWorkplace.Service.poc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace FunAtWorkplace.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {            
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<TdHostedService>();
            services.AddMvc(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);            

            services.AddSwaggerExtension(Configuration);
            services.InjectDependencies(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerExtension(Configuration);
            //ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol
            //                                     | SecurityProtocolType.Tls12
            //                                     //| SecurityProtocolType.Ssl3 
            //                                     | SecurityProtocolType.Tls12
            //                                     | SecurityProtocolType.Tls11
            //                                     | SecurityProtocolType.Tls;
            app.UseMvc();
        }
    }
}