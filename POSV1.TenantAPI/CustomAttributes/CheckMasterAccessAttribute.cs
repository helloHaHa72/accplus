//using BaseAppSettings;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using POSV1.TenantAPI.Models;
//using System.Security.Claims;

//namespace POSV1.TenantAPI.CustomAttributes
//{
//    //ActionFilterAttribute
//    //public class CheckAccessAttribute : ActionFilterAttribute
//    //{
//    //    private readonly EnumApplicationUserType[] _requiredRoles;
//    //    public CheckAccessAttribute(params EnumApplicationUserType[] requiredRoles)
//    //    {
//    //        _requiredRoles = requiredRoles;
//    //    }
//    //    public override async Task OnActionExecutionAsync(ActionExecutingContext context,
//    //        ActionExecutionDelegate next)
//    //    {
//    //        var username = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
//    //        var menuRepository = (IRoleActionsRepo)context.HttpContext.RequestServices.GetService(typeof(IRoleActionsRepo));

//    //        var roleNames = _requiredRoles.Select(role => Enum.GetName(typeof(EnumApplicationUserType), role)).ToArray();
//    //        var hasAccess = await menuRepository.HasAccessForRole(username, roleNames);
//    //        if (!hasAccess)
//    //        {
//    //            context.Result = new ContentResult()
//    //            {
//    //                StatusCode = 401,
//    //                Content = "You are not authorized to access this resource."
//    //            };
//    //        }
//    //        else
//    //        {
//    //            await next();
//    //        }
//    //    }
//    //}

//    public class CheckMasterAccessAttribute : AuthorizeAttribute, IAuthorizationFilter
//    {
//        private readonly EnumApplicationUserType[] _requiredRoles;

//        public CheckMasterAccessAttribute(params EnumApplicationUserType[] requiredRoles)
//        {
//            _requiredRoles = requiredRoles;
//        }

//        public void OnAuthorization(AuthorizationFilterContext context)
//        {
//            var username = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
//            var menuRepository = (IRoleActionsRepo)context.HttpContext.RequestServices.GetService(typeof(IRoleActionsRepo));

//            var roleNames = _requiredRoles.Select(role => Enum.GetName(typeof(EnumApplicationUserType), role)).ToArray();
//            if (roleNames.Contains(nameof(EnumApplicationUserType.SuperAdmin))) { return; }
//            var hasAccess = menuRepository.HasAccessForRole(username, roleNames).Result;

//            if (!hasAccess)
//            {
//                context.Result = new ContentResult()
//                {
//                    StatusCode = 401,
//                    Content = "You are not authorized to access this resource."
//                };
//            }
//        }
//    }

//    public class CheckTenantAccessAttribute : AuthorizeAttribute, IAuthorizationFilter
//    {
//        private readonly EnumApplicationUserType[] _requiredRoles;

//        public CheckTenantAccessAttribute(params EnumApplicationUserType[] requiredRoles)
//        {
//            _requiredRoles = requiredRoles;
//        }

//        public void OnAuthorization(AuthorizationFilterContext context)
//        {
//            var username = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
//            var menuRepository = (IRoleActionsRepo)context.HttpContext.RequestServices.GetService(typeof(IRoleActionsRepo));

//            var roleNames = _requiredRoles.Select(role => Enum.GetName(typeof(EnumApplicationUserType), role)).ToArray();
//            if (roleNames.Contains(nameof(EnumApplicationUserType.GeneralAdmin))) { return; }
//            var hasAccess = menuRepository.HasAccessForRole(username, roleNames).Result;

//            if (!hasAccess)
//            {
//                context.Result = new ContentResult()
//                {
//                    StatusCode = 401,
//                    Content = "You are not authorized to access this resource."
//                };
//            }
//        }
//    }

//}
