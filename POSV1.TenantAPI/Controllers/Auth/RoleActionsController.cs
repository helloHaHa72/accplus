//using BaseAppSettings;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using POSV1.MasterDBModel.AuthModels;
//using POSV1.MasterDBModel.Interface;
//using POSV1.TenantAPI.CustomAttributes;
//using POSV1.TenantAPI.Models;
//using POSV1.TenantAPI.Models.AuthModels;

//namespace POSV1.TenantAPI.Controllers.Auth
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//    [CheckMasterAccess(EnumApplicationUserType.SuperAdmin, EnumApplicationUserType.GeneralAdmin, EnumApplicationUserType.SystemAdmin, EnumApplicationUserType.SystemOperator, EnumApplicationUserType.SystemReporter)]

//    public class RoleActionsController : _AbsAuthenticatedController
//    {
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly IRoleActionsRepo _roleActionsRepo;
//        public RoleActionsController(RoleManager<IdentityRole> roleManager , IRoleActionsRepo roleActionsRepo)
//        {
//            _roleManager = roleManager;
//            _roleActionsRepo = roleActionsRepo;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetDetail(string roleId)
//        {
//            try
//            {
//                IdentityRole role = await _roleManager.FindByIdAsync(roleId);

//                if (role == null)
//                {
//                    throw new Exception("roleId is not found");
//                }

//                var roleActionEntities = await _roleActionsRepo.GetList()
//                    .Where(x => x.RoleId == roleId)
//                    .Include(x => x.Role)
//                    .Include(x => x.Actions)
//                    .ToListAsync();

//                VMRoleActionList Result = new VMRoleActionList()
//                {                  
//                    RoleId = role.Id,
//                    Role = role.Name,
//                };

//                Result.Actions = roleActionEntities.Select(x => new VMActionList()
//                {
//                    ActionId = x.ActionId,
//                    ActionCaption = x.Actions?.con02caption
//                })
//                    .ToList();

//                return Ok(Result);
//            }
//            catch
//            {
//                throw new Exception("invalid data");
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> AssignActions(VMRoleActions data)
//        {
//            try
//            {
//                var roles = await _roleManager.Roles.Where(x => x.Id == data.RoleId).ToListAsync();
//                if (roles == null)
//                {
//                    throw new Exception($"The particular role is not found");
//                }

//                var roleActionEntities = data.ActionId.Select(x => new RoleActions
//                {
//                    RoleId = data.RoleId,
//                    ActionId = x,
//                    DateCreated = DateTime.Now,
//                    CreatedName = _ActiveUserName,
//                });

//                await _roleActionsRepo.AssignActionsToRoleAsync(roleActionEntities);
//                return Ok("Actions assigned succesfully");
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpPut()]
//        public async Task<IActionResult> UpdateRoleAction(VMRoleActions data)
//        {
//            try
//            {
//                IdentityRole role = await _roleManager.FindByIdAsync(data.RoleId);
//                if (role == null)
//                {
//                    return NotFound($"Role with id {data.RoleId} not found");
//                }
             
//                SoftDeleteNonExisingRoleActions(data);

//                foreach (int actionId in data.ActionId)
//                {            
//                    var existingRoleAction =await _roleActionsRepo.GetList()
//                        .FirstOrDefaultAsync(x => x.RoleId == data.RoleId && x.ActionId == actionId);

//                    if (existingRoleAction == null)
//                    {
//                        AddNewRoleAction(data.RoleId, actionId);
//                    }
//                }
//                return Ok("RoleAction updated successfully");
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        private void AddNewRoleAction(string roleId, int actionId)
//        {
//            _roleActionsRepo.Insert(new RoleActions { RoleId = roleId, ActionId = actionId });
//            _roleActionsRepo.SaveAsync();
//        }

//        private void SoftDeleteNonExisingRoleActions(VMRoleActions data)
//        {
//            var actionsToDelete = _roleActionsRepo.GetList()
//                 .Where(x => x.RoleId == data.RoleId && !data.ActionId.Contains(x.ActionId))
//                 .ToList();
//            foreach (var actions in actionsToDelete)
//            {
//                actions.DateDeleted = DateTime.UtcNow;
//                _roleActionsRepo.Update(actions);
//                _roleActionsRepo.SaveAsync();
//            }
//        }



//    }
//}
