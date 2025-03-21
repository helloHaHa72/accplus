using BaseAppSettings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using POSV1.MasterDBModel.AuthModels;
using POSV1.MasterDBModel.Data;
using POSV1.TenantModel;

namespace POSV1.TenantAPI.Startup
{
    public static class DBContextExtension
    {
        public static void ConfigureDatabase(
            this WebApplicationBuilder builder,
            IConfiguration configuration)
        {
            builder.Services.AddDbContext<MainDbContext>(options =>
                            options.UseSqlServer(configuration.GetConnectionString("MainConnection")));

            builder.Services.AddDbContext<LoggingDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("MainConnection")));

            // For Identity in master database which stores all users and organization data
            //builder.Services
            //    .AddScoped<IUserStore<ApplicationUser>, UserStore<ApplicationUser, ApplicationRole, MasterDbContext>>();

            //builder.Services
            //    .AddIdentity<ApplicationUser, ApplicationRole>()
            //    .AddEntityFrameworkStores<MasterDbContext>()
            //    .AddDefaultTokenProviders();

            // For Identity
            builder.Services.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MainDbContext>()
                .AddDefaultTokenProviders();

            //builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
            //    .AddEntityFrameworkStores<MasterDbContext>()
            //    .AddDefaultTokenProviders();

            //builder.Services
            //    .AddScoped<IUserStore<ApplicationUser>, UserStore<ApplicationUser, ApplicationRole, MasterDbContext>>();

        }
        public static async Task UseDBMigrationAsync(this WebApplication app, WebApplicationBuilder builder)
        {
            // Get an instance of your DbContext
            using (var scope = builder.Services.BuildServiceProvider().CreateScope())
            {
                //1. Auto migrate Master Database
                var dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();
                await DatabaseMigrationService.MigrateDatabaseAsync(dbContext);

                //var logDBContext = scope.ServiceProvider.GetRequiredService<LoggingDbContext>();
                //logDBContext.Database.Migrate();

            }
        }

        /// Seeds initial master data (Roles, Users, Permissions, AccessLists).
        public static async Task SeedMasterDataAsyncV2(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await DatabaseSeeder.InitializeAsync(services);
            }
        }

        //public static async Task SeedMasterDataAsync(this IApplicationBuilder app)
        //{
        //    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
        //    {

        //        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        //        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        //        foreach (var roleName in Enum.GetNames(typeof(EnumApplicationUserType)))
        //        {
        //            if (!await roleManager.RoleExistsAsync(roleName))
        //            {
        //                var role = new ApplicationRole(roleName);
        //                var result = await roleManager.CreateAsync(role);
        //                if (!result.Succeeded)
        //                {
        //                    //logger.LogError($"Error creating role {roleName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        //                }
        //            }
        //        }

        //        // Check if there is at least one user with the SuperAdmin role
        //        var superAdminRole = EnumApplicationUserType.SuperAdmin.ToString();
        //        var superAdminUsers = await userManager.GetUsersInRoleAsync(superAdminRole);

        //        if (superAdminUsers.Count == 0)
        //        {
        //            var superAdminEmail = "superadmin@sebs.asia";
        //            var defaultUser = new ApplicationUser
        //            {
        //                UserName = superAdminEmail,
        //                Email = superAdminEmail,
        //                EmailConfirmed = true,
        //                CanAccessAllBranches = true,
        //            };
        //            var createUserResult = await userManager.CreateAsync(defaultUser, "Admin@123!");
        //            if (createUserResult.Succeeded)
        //            {
        //                await userManager.AddToRoleAsync(defaultUser, superAdminRole);
        //            }
        //            else
        //            {
        //                //logger.LogError($"Error creating default SuperAdmin user: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
        //            }
        //        }
        //    }
        //}
    }
}
