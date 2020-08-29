using System;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DatingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Used to initialize the host
            var host = CreateHostBuilder(args).Build();

            #region Seeding data instructions
            //Use host to configure a service but, scope needs to be defined
            using (var scope = host.Services.CreateScope())
            {
                //Resolves scope Dependencies
                var services = scope.ServiceProvider;
                try
                {                 //instance of context to talk w/ DB
                    var context = services.GetRequiredService<DataContext>();
                    //Creates DB if none exists
                    context.Database.Migrate();
                    Seed.SeedUsers(context);
                }
                catch (Exception e)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError("Seeding error", e);
                }
            }
            #endregion

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
