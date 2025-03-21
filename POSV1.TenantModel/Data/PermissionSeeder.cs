using BaseAppSettings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using POSV1.MasterDBModel.AuthModels;
using POSV1.TenantModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.MasterDBModel.Data
{
    public static class PermissionSeeder
    {
        public static async Task SeedPermissionsAsync(MainDbContext context, RoleManager<IdentityRole> roleManager)
        {
            var generalAdminRole = await roleManager.FindByNameAsync(nameof(EnumApplicationUserType.GeneralAdmin));

            if (generalAdminRole == null)
            {
                return; // Ensure role exists before seeding permissions
            }

            var existingPermissions = await context.UserPermissionLists
                .Where(p => p.roleId == generalAdminRole.Id)
                .Select(p => (int)p.AccesListId)
                .ToListAsync();

            var allPermissions = Enum.GetValues(typeof(EnumSubCategory))
                .Cast<EnumSubCategory>()
                .Select(e => (int)e)
                .ToList();

            var missingPermissions = allPermissions.Except(existingPermissions).ToList();

            if (missingPermissions.Any())
            {
                var newPermissions = missingPermissions.Select(permissionId => new UserPermissionList
                {
                    roleId = generalAdminRole.Id,
                    AccesListId = (EnumSubCategory)permissionId,
                    DateCreated = DateTime.UtcNow,
                    CreatedName = "SYSTEM",
                }).ToList();

                await context.UserPermissionLists.AddRangeAsync(newPermissions);
                await context.SaveChangesAsync();
            }
        }
    }

}
