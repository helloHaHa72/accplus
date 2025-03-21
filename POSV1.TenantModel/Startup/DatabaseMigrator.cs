using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using RepoBaseModelCore;

namespace POSV1.TenantModel
{
    public class DatabaseMigrationService
    {
        public static async Task MigrateDatabaseAsync(DbContext dbContext)
        {

            if (dbContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.SqlServer")
            {
                throw new NotSupportedException("Auto migration is only supported for SQL Server provider.");
            }

            var migrator = dbContext.GetInfrastructure().GetService<IMigrator>();
            await migrator.MigrateAsync();
        }
    }
}