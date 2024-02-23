using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Plan_current_repairs.Data.Models;
using System;

namespace Plan_current_repairs.Extensions
{
    public static class MigrationExtensions
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var appDBContext = scope.ServiceProvider.GetRequiredService<AppDBContext>())
                {
                    try
                    {
                        appDBContext.Database.Migrate();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            return host;
        }
    }
}
