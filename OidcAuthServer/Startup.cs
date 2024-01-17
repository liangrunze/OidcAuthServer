using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServerSite.Ids4;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace ServerSite
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            OidcConfig.Configure(_configuration);
            services.AddLogging(_ => _.AddConsole());

            services.AddRouting();

            services.AddControllers();

            services.AddIds4();
        }

        public void Configure(IApplicationBuilder app)
        {

            app.UseDeveloperExceptionPage();

            app.UseIds4();

            app.UseStaticFiles();

            app.UseRouting();
            app.Use(async (context, next) =>
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (env != "Development" && context.Request.Path.Value.StartsWith("/.debug", StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.StatusCode = 404;
                }
                else
                {
                    await next();
                }
            });
            app.UseEndpoints(_=>_.MapDefaultControllerRoute());
        }
    }
}