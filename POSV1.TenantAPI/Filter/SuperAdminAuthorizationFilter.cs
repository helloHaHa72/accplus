using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;

namespace POSV1.TenantAPI.Filter
{
    public class SuperAdminAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<SuperAdminAuthorizationFilter> _logger;

        public SuperAdminAuthorizationFilter(UserManager<IdentityUser> userManager, ILogger<SuperAdminAuthorizationFilter> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = await _userManager.GetUserAsync(context.HttpContext.User);

            if (user != null && await _userManager.IsInRoleAsync(user, "SuperAdmin"))
            {
                // User is authenticated and has the "SuperAdmin" role, allow access.
                return;
            }

            // User is not authenticated or not a "SuperAdmin", deny access.
            context.Result = new ForbidResult();
        }
    }
}
