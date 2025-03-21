using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace POSV1.TenantAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public abstract class _AbsAuthenticatedController : ControllerBase
    {
        public ClaimsPrincipal _ActiveUser => HttpContext.User;
        public string _ActiveUserName => _ActiveUser.Identity == null ? "Admin" : _ActiveUser.Identity.Name;
        public string TenantId => _ActiveUser.Claims is null ? "" : _ActiveUser.Claims.FirstOrDefault(x => x.Type == "X-Tenant-Id")?.Value;
        //public string TenantId => _ActiveUser.Claims is null ? "" : _ActiveUser.Claims.FirstOrDefault(x => x.Type == "Tenant-Id")?.Value;
        public _AbsAuthenticatedController()
        {

        }
    }
}
