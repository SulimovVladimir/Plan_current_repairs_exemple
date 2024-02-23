using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Plan_current_repairs.Extensions;
using System.IO;
using System.Net;


namespace Plan_current_repairs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().MigrateDatabase().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(AddAppConfiguration =>
                {
                    AddAppConfiguration.AddJsonFile("Settings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureLogging(builder => builder.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "Logs")))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(options =>
                    {
                        options.Listen(IPAddress.Loopback, 5301);
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
