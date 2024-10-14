using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guids.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Guids.Api.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using ApplicationDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.Migrate();
        }
    }
}