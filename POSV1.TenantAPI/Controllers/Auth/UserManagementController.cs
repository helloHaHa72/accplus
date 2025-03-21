using BaseAppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.MasterDBModel.AuthModels;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Services.Interface;
using POSV1.TenantModel;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Models.EntityModels.Settings;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace POSV1.TenantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserManagementController : _AbsAuthenticatedController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MainDbContext _context;
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserManagementController(
            MainDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IAuthService authService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authService = authService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList()
        {
            var users = await _userManager.Users
                .OrderBy(u => u.UserName)
                .ToListAsync();

            var result = new List<VMUserList>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var userVM = new VMUserList
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Status = true,
                    user_Roles = roles.Select(role => new VMUserRole { Name = role }).ToList()
                };

                result.Add(userVM);
            }

            return Ok(result);
        }



        //[HttpPost("Create")]
        //public async Task<IActionResult> Create([FromBody] VMUser vmUser)
        //{
        //    try
        //    {
        //        if (vmUser == null)
        //        {
        //            return BadRequest("Invalid data received");
        //        }
        //        //var userDetail = await _userManager.FindByNameAsync(vmUser.UserName);

        //        //var user = new IdentityUser
        //        //{
        //        //    UserName = vmUser.UserName,
        //        //    Email = vmUser.Email,
        //        //    PhoneNumber = vmUser.PhoneNumber,
        //        //    PasswordHash = userDetail.PasswordHash,
        //        //};

        //        var model = new RegisterModel()
        //        {
        //            Username = vmUser.UserName,
        //            Email = vmUser.Email,
        //            Password = vmUser.Password,
        //            PhoneNumber = vmUser.PhoneNumber,
        //        };

        //        var userInsert = await _authService.RegisterUser(model);

        //        var userDetail = await _userManager.FindByNameAsync(vmUser.UserName);

        //        if (vmUser.user_Roles != null && vmUser.user_Roles.Any())
        //        {
        //            foreach (var role in vmUser.user_Roles)
        //            {
        //                var roleExists = await _roleManager.RoleExistsAsync(role.Name);

        //                if (!roleExists)
        //                {
        //                    var createRoleResult = await _roleManager.CreateAsync(new IdentityRole(role.Name));

        //                    if (!createRoleResult.Succeeded)
        //                    {
        //                        return BadRequest($"Unable to create role '{role.Name}': {string.Join(", ", createRoleResult.Errors)}");
        //                    }
        //                }

        //                var addToRoleResult = await _userManager.AddToRoleAsync(userDetail, role.Name);

        //                if (!addToRoleResult.Succeeded)
        //                {
        //                    return BadRequest($"Unable to assign role '{role.Name}' to user: {string.Join(", ", addToRoleResult.Errors)}");
        //                }
        //            }
        //        }

        //        // Assign user to branch
        //        if (!string.IsNullOrEmpty(vmUser.BranchCode)) // Ensure the branch code is provided
        //        {
        //            var branch = await _context.BranchDatas.FindAsync(vmUser.BranchCode);
        //            if (branch != null)
        //            {
        //                var userBranch = new UserBranch
        //                {
        //                    UserId = userDetail.Id,
        //                    BranchCode = vmUser.BranchCode
        //                };

        //                _context.UserBranches.Add(userBranch);
        //                await _context.SaveChangesAsync();
        //            }
        //            else
        //            {
        //                return BadRequest("Invalid branch code!");
        //            }
        //        }

        //        return Ok("Role assigned successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Failed to assign role. {ex.Message}");
        //    }
        //}


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] VMUser vmUser)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext.User.FindFirst("BranchCode")?.Value;

                if (vmUser == null)
                {
                    return BadRequest("Invalid data received");
                }

                // Create the user model for registration
                var model = new RegisterModel()
                {
                    Username = vmUser.UserName,
                    Email = vmUser.Email,
                    Password = vmUser.Password,
                    PhoneNumber = vmUser.PhoneNumber,
                };

                // Register the user using the _authService
                //var userInsert = await _authService.RegisterUser(model, (int)TenantId);

                int tenantId;

                if (int.TryParse(TenantId, out tenantId))
                {
                    var userInsert = await _authService.RegisterUser(model, tenantId);
                }

                var userDetail = await _userManager.FindByNameAsync(vmUser.UserName);

                // Create and assign roles dynamically based on the user input
                if (vmUser.user_Roles != null && vmUser.user_Roles.Any())
                {
                    foreach (var role in vmUser.user_Roles)
                    {
                        // Check if the role exists; if not, create it
                        var roleExists = await _roleManager.RoleExistsAsync(role.Name);
                        if (!roleExists)
                        {
                            var createRoleResult = await _roleManager.CreateAsync(new IdentityRole(role.Name));
                            if (!createRoleResult.Succeeded)
                            {
                                return BadRequest($"Unable to create role '{role.Name}': {string.Join(", ", createRoleResult.Errors)}");
                            }
                        }

                        // Add the user to the role
                        var addToRoleResult = await _userManager.AddToRoleAsync(userDetail, role.Name);
                        if (!addToRoleResult.Succeeded)
                        {
                            return BadRequest($"Unable to assign role '{role.Name}' to user: {string.Join(", ", addToRoleResult.Errors)}");
                        }
                    }
                }

                // Assign user to branch dynamically based on provided branch code
                //if (!string.IsNullOrEmpty(branchCode)) // Ensure branch code is provided
                //{
                //    var branch = await _masterDbContext.TenantBranches
                //            .Where(b => b.BranchCode == branchCode)
                //            .FirstOrDefaultAsync();

                //    if (branch != null)
                //    {
                //        var userBranch = new UserBranch
                //        {
                //            UserId = userDetail.Id,
                //            BranchCode = vmUser.BranchCode
                //        };

                //        _context.UserBranches.Add(userBranch);
                //        await _context.SaveChangesAsync();
                //    }
                //    else
                //    {
                //        return BadRequest("Invalid branch code!");
                //    }
                //}
                //else
                //{
                //    //this will be used if the userbranch is not null

                //    var userBranch = new UserBranch
                //    {
                //        UserId = userDetail.Id,
                //        BranchCode = branchCode
                //    };

                //    _context.UserBranches.Add(userBranch);
                //    await _context.SaveChangesAsync();
                //}

                return Ok("User created and roles assigned successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create user and assign roles. {ex.Message}");
            }
        }

        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> GetDetail(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            VMUserDetail userEntity = new VMUserDetail()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            var assignedRoles = await _userManager.GetRolesAsync(user);

            var userRoles = await _roleManager.Roles
                .Where(role => assignedRoles.Contains(role.Name))
                .ToListAsync();

            userEntity.user_RolesDetail = userRoles.Select(role => new VMUserRoleDetail()
            {
                Id = role.Id,
                Name = role.Name
            }).ToList();

            return Ok(userEntity);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] VMUser vmUserUpdate)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            user.UserName = vmUserUpdate.UserName;
            user.Email = vmUserUpdate.Email;
            user.PhoneNumber = vmUserUpdate.PhoneNumber;

            // Update password only if provided
            //if (!string.IsNullOrEmpty(user.PasswordHash))
            //{
            //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            //    await _userManager.ResetPasswordAsync(user, token, user.PasswordHash);
            //}

            var roleNames = vmUserUpdate.user_Roles.Select(role => role.Name);

            var existingRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, existingRoles);
            var addRoleResult = await _userManager.AddToRolesAsync(user, roleNames);

            if (!addRoleResult.Succeeded)
            {
                return BadRequest("Unable to update roles");
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest("Unable to update user");
            }

            return Ok("User updated successfully");
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);

            // Remove user from all existing roles
            var removeRoleResult = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!removeRoleResult.Succeeded)
            {
                return BadRequest("Unable to remove user from roles");
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest("Unable to delete user");
            }

            return Ok("User deleted successfully");
        }





    }
}
