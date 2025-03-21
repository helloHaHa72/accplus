//using Microsoft.AspNetCore.Mvc;
//using POSV1.MasterDBModel.AuthModels;
//using POSV1.MasterDBModel.Interface;
//using POSV1.TenantAPI.Models;
//using POSV1.TenantModel;
//using System.Reflection;

//namespace POSV1.TenantAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class MenuInfoController : ControllerBase
//    {
//        private readonly MasterDbContext _context;
//        private readonly IMenuInfoRepo _menuInfoRepo;
//        private readonly IActionRepo _actionRepo;

//        public MenuInfoController(
//            MasterDbContext context,
//            IMenuInfoRepo menuInfoRepo,
//            IActionRepo actionRepo)
//        {
//            _context = context;
//            _menuInfoRepo = menuInfoRepo;
//            _actionRepo = actionRepo;
//        }

//        [HttpPost]
//        public async Task<IActionResult> CreateControllersAndActions()
//        {
//            List<con01menuInfo> controllerInfoList = new List<con01menuInfo>();

//            // Get all assemblies in the current domain
//            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

//            foreach (var assembly in assemblies)
//            {
//                var controllerTypes = assembly.GetTypes().Where(t => typeof(ControllerBase).IsAssignableFrom(t));
//                foreach (var controllerType in controllerTypes)
//                {
//                    var controllerInfo = new con01menuInfo
//                    {
//                        con01controller = controllerType.Name.Replace("Controller", ""),
//                        con02action = new List<con02action>(),
//                        con01module = "default"
//                    };
//                    controllerInfo.con01caption = $"{controllerInfo.con01controller}";

//                    var methods = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
//                        .Where(m => !m.IsSpecialName && m.DeclaringType == controllerType);

//                    foreach (var method in methods)
//                    {
//                        var actionName = method.Name;
//                        var attribute = method.GetCustomAttribute<RouteAttribute>();

//                        if (attribute != null)
//                        {
//                            actionName = attribute.Template;
//                        }
//                        if (!string.IsNullOrEmpty(actionName))
//                        {
//                            var action = new con02action
//                            {
//                                con02name = actionName,
//                                con02caption = $"{controllerInfo.con01controller}_{actionName}"
//                            };
//                            controllerInfo.con02action.Add(action);
//                        }
//                    }

//                    // Get methods of base controllers
//                    Type baseType = controllerType.BaseType;
//                    while (baseType != null && baseType != typeof(ControllerBase))
//                    {
//                        var baseMethods = baseType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
//                            .Where(m => !m.IsSpecialName && m.DeclaringType == baseType);

//                        foreach (var method in baseMethods)
//                        {
//                            var actionName = method.Name;
//                            var attribute = method.GetCustomAttribute<RouteAttribute>();

//                            if (attribute != null)
//                            {
//                                actionName = attribute.Template;
//                            }

//                            if (!string.IsNullOrEmpty(actionName))
//                            {
//                                var action = new con02action
//                                {
//                                    con02name = actionName,
//                                    con02caption = $"{controllerInfo.con01controller}_{actionName}"
//                                };

//                                controllerInfo.con02action.Add(action);
//                            }
//                        }

//                        baseType = baseType.BaseType;
//                    }

//                    controllerInfoList.Add(controllerInfo);
//                }
//            }

//            _context.con01menuInfo.AddRange(controllerInfoList);
//            _context.SaveChanges();

//            return Ok("Created successfully");
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetMenuInfoList()
//        {
//            var menuInfoList = _menuInfoRepo.GetList();

//            var vmMenuInfoList = menuInfoList.Select(menuInfo => new VMMenuInfoList
//            {
//                Id = menuInfo.con01uin,
//                //Controller_Name = menuInfo.con01controller, 
//                Controller_Caption = menuInfo.con01caption,
//                Module = menuInfo.con01module,
//                VMActions = menuInfo.con02action.Select(action => new VMActions
//                {
//                    Id = action.con02uin,
//                    //Action_Name = action.con02name, 
//                    Action_Caption = action.con02caption
//                }).ToList()
//            }).ToList();

//            return Ok(vmMenuInfoList);
//        }
//    }
//}
