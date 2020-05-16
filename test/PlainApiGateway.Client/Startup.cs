using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using PlainApiGateway.Configuration;
using PlainApiGateway.Extension;

namespace PlainApiGateway.TestClient
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPlainApiGateway(this.configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var plainMiddlewareConfiguration = new PlainMiddlewareConfiguration
            {
                PreResponseMiddleware = async (context, next) =>
                {
                    await next.Invoke();
                }
            };
            app.UsePlainApiGateway(plainMiddlewareConfiguration);
        }
    }
}
