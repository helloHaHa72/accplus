using BaseAppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.MasterDBModel.AuthModels;
using POSV1.TenantAPI.Models;
using POSV1.TenantModel.Repo.Interface.Settings;
using System.Security.Claims;

namespace POSV1.TenantAPI.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : _AbsAuthenticatedController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAccessListRepo _accessListRepo;
        private readonly IUserPermissionListRepo _userPermissionListRepo;

        //public ClaimsPrincipal _ActiveUser => HttpContext.User;
        //public string _ActiveUserName => _ActiveUser.Identity == null ? "Admin" : _ActiveUser.Identity.Name;
        public UserRolesController(
            RoleManager<IdentityRole> roleManager,
            IAccessListRepo accessListRepo,
            IUserPermissionListRepo userPermissionListRepo)
        {
            _roleManager = roleManager;
            _accessListRepo = accessListRepo;
            _userPermissionListRepo = userPermissionListRepo;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList()
        {
            var roles = await _roleManager.Roles
                .OrderByDescending(r => r.Id)
                .ToListAsync();
            return Ok(roles);
        }

        [HttpGet("GetList/AccessList")]
        public async Task<IActionResult> GetAccessList()
        {
            var roles = await _accessListRepo.GetList().ToListAsync();

            var groupedRoles = roles
                .GroupBy(r => r.MainHeading)
                .Select(g => new
                {
                    MainHeadingId = g.Key,
                    MainHeading = g.Key.ToString(),
                    Items = g.ToList()
                })
                .ToList();

            return Ok(groupedRoles);
        }


        //[HttpPost("Create")]
        //public async Task<IActionResult> Create([FromBody] VMRoles model)
        //{
        //    IdentityRole role = new()
        //    {
        //        Name = model.Name,
        //    };
        //    IdentityResult result = await _roleManager.CreateAsync(role);

        //    if (result.Succeeded)
        //    {
        //        return Ok("Role Created Successfully");
        //    }
        //    else
        //    {
        //        return BadRequest(result.Errors);
        //    }
        //}

        [HttpPost("Create/WithPermissions")]
        public async Task<IActionResult> Create([FromBody] VMRoles model)
        {
            try
            {
                IdentityRole role = new()
                {
                    Name = model.Name,
                    //TenantId = TenantId,
                };

                IdentityResult result = await _roleManager.CreateAsync(role);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                // Retrieve the newly created role to get its ID
                var createdRole = await _roleManager.FindByNameAsync(model.Name);
                if (createdRole == null)
                {
                    throw new Exception("Failed to retrieve the newly created role.");
                }

                // Insert permissions for the role
                foreach (var permissionId in model.PermissionIds)
                {
                    var insertData = new UserPermissionList()
                    {
                        roleId = createdRole.Id, // Use the ID of the newly created role
                        AccesListId = permissionId,
                        DateCreated = DateTime.UtcNow,
                        CreatedName = _ActiveUserName,
                    };

                    _userPermissionListRepo.Insert(insertData);
                }

                await _userPermissionListRepo.SaveAsync();

                return Ok("Role Created Successfully with Permissions");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //[HttpPost("SeedRoles")]
        //public async Task<IActionResult> SeedRoles()
        //{
        //    var roles = Enum.GetValues(typeof(EnumApplicationUserType)).Cast<EnumApplicationUserType>().Select(r => r.ToString()).ToList();
        //    var existingRoles = _roleManager.Roles.Select(r => r.Name).ToList();

        //    foreach (var role in roles)
        //    {
        //        if (!existingRoles.Contains(role))
        //        {
        //            IdentityRole identityRole = new() { Name = role };
        //            IdentityResult result = await _roleManager.CreateAsync(identityRole);

        //            if (!result.Succeeded)
        //            {
        //                return BadRequest($"Error creating role: {role}");
        //            }
        //        }
        //    }

        //    return Ok("Roles seeded successfully");
        //}

        //[HttpGet("Detail/{id}")]
        //public async Task<IActionResult> GetRoleDetails(string id)
        //{
        //    IdentityRole role = await _roleManager.FindByIdAsync(id);

        //    if (role == null)
        //    {
        //        return NotFound("Role not found");
        //    }
        //    return Ok(role);
        //}

        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> GetRoleDetails(string id)
        {
            try
            {
                IdentityRole role = await _roleManager.FindByIdAsync(id);

                if (role == null)
                {
                    return NotFound("Role not found");
                }

                // Retrieve associated permissions
                var permissions = await _userPermissionListRepo.GetList()
                    .Where(p => p.roleId == id)
                    .Select(p => p.AccesListId)
                    .ToListAsync();

                var roleDetails = new
                {
                    Id = role.Id,
                    Name = role.Name,
                    Permissions = permissions
                };

                return Ok(roleDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        //[HttpPut("Edit/{id}")]
        //public async Task<IActionResult> Edit(string id, [FromBody] VMRoles model)
        //{
        //    IdentityRole role = await _roleManager.FindByIdAsync(id);

        //    if (role == null)
        //    {
        //        return NotFound("Role not found");
        //    }

        //    role.Name = model.Name;

        //    IdentityResult result = await _roleManager.UpdateAsync(role);

        //    if (result.Succeeded)
        //    {
        //        return Ok("Role Updated Successfully");
        //    }
        //    else
        //    {
        //        return BadRequest(result.Errors);
        //    }
        //}

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] VMRoles model)
        {
            try
            {
                IdentityRole role = await _roleManager.FindByIdAsync(id);

                if (role == null)
                {
                    return NotFound("Role not found");
                }

                // Update Role Name
                role.Name = model.Name;
                IdentityResult result = await _roleManager.UpdateAsync(role);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                // Delete previous permissions
                await DeleteRolePermissions(id);

                // Insert new permissions
                foreach (var permissionId in model.PermissionIds)
                {
                    var newPermission = new UserPermissionList()
                    {
                        roleId = id,
                        AccesListId = permissionId,
                        UpdatedName = _ActiveUserName,
                        DateUpdated = DateTime.UtcNow,
                    };

                    _userPermissionListRepo.Insert(newPermission);
                }

                await _userPermissionListRepo.SaveAsync();

                return Ok("Role Updated Successfully with New Permissions");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task DeleteRolePermissions(string roleId)
        {
            var existingPermissions = _userPermissionListRepo.GetList()
                .Where(p => p.roleId == roleId)
                .ToList();

            if (existingPermissions.Any())
            {
                foreach (var permission in existingPermissions)
                {
                    permission.DateDeleted = DateTime.UtcNow; // Set DeletedDate
                    permission.DeletedName = _ActiveUserName;

                    _userPermissionListRepo.Update(permission);
                }

                await _userPermissionListRepo.SaveAsync();
            }
        }




        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound("Role not found");
            }

            IdentityResult result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                await DeleteRolePermissions(id);

                return Ok("Role Deleted Successfully");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}
