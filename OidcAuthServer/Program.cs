using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace ServerSite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureKestrel(options =>
            {
                var env = Environment.GetEnvironmentVariable("ENABLE_SSL");
                bool.TryParse(env, out bool isEnableSSL);
                if (isEnableSSL)
                {
                    options.Listen(IPAddress.Any, 443, listenoptions =>
                    {
                        var certificatePassword = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "certificate", "PasswordKey"));
                        listenoptions.UseHttps(Path.Combine(AppContext.BaseDirectory, "certificate", "certificate.pfx"), certificatePassword);
                    });
                }
                options.Listen(IPAddress.Any, 80);
            })
                .CaptureStartupErrors(true)
                .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
    }
}