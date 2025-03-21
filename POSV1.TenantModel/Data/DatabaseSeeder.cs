using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using POSV1.MasterDBModel.AuthModels;
using POSV1.TenantModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.MasterDBModel.Data
{
    public static class DatabaseSeeder
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MainDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await context.Database.MigrateAsync();

            // Call individual seeders
            //await RoleSeeder.SeedRolesAsync(roleManager);
            //await UserSeeder.SeedUsersAsync(userManager, roleManager);
            await AccessListSeeder.SeedAccessListAsync(context);
            await PermissionSeeder.SeedPermissionsAsync(context, roleManager);
        }
    }

}
