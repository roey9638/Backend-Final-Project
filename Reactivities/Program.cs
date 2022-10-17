using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Reactivities.DataDBContext;
using Reactivities.Modules;
using Reactivitiess.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace Reactivities
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //This here is a [Dependency Injection]
            //[scope] will be (eual/=) to (host) which he wil have any [services] that [we create] [inside this patricular method]
            //[using] will do [Dispose] when it's [finished running].
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            try
            {
                //This will [try] to to get the [service] that [we want] which is [DataContext]
                //[context] will be a type of [DataContext]
                var context = services.GetRequiredService<DataContext>();

                var userManager = services.GetRequiredService<UserManager<AppUser>>();

                //This [line] will [execute] and apply the [Migraton] if the [DataBase which is DataContext] still hasnt been created. Continue DownVV
                //and if the [1 line] above^^ will be [succesfull]
                await context.Database.MigrateAsync();

                await Seed.SeedData(context, userManager);
            }
            catch (Exception ex)
            {
                //This [<Ilogger>>] [Requires] to know where we try to [Login] to a [Service] and in this case [it's here] in [Program]
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An Error occured during migration");
            }

            //This will start the app.
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
