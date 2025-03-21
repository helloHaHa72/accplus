using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.MasterDBModel.AuthModels;
using POSV1.TenantAPI.Extensions;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Models.AuthModels;
using POSV1.TenantAPI.Services.Interface;
using POSV1.TenantModel;
using System.Data;

namespace POSV1.TenantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        private readonly JwtService _jwtService;
        public AuthController(
            ILogger<AuthController> logger,
            IAuthService authService,
            JwtService jwtService
            )
        {
            _logger = logger;
            _jwtService = jwtService;
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                //auto migrate tenant db

                LoginResult result = await _authService.Login(model);

                if (result is null)
                {
                    return Unauthorized();
                }

                return Ok(new ResLogin
                {
                    username = result.Username,
                    TenatnId = result.TenantID,
                    OrgName = result.OrgName,
                    //branch = result.BranchCode,
                    email = result.Email,
                    associatedRoles = result.AssociatedRoles.ToArray(),
                    token = result.Token,
                    expiration = result.Expiration,
                    Permissions = result.Permissions.ToArray(),
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Login Error :" + ex.DetailErrorLog(), ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("checkTokenStatus")]
        //[Authorize]
        public IActionResult CheckTokenStatus([FromBody] string token)
        {
            try
            {
                var result = _jwtService.ValidateToken(Request.HttpContext, token);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError("Token Validation Error: " + ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("SeedDefaultData")]
        public async Task<IActionResult> SeedDefultData()
        {
            try
            {
                await _authService.SeedDefaultData();
                return Ok("Data added sucessfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            try
            {
                var result = await _authService.RegisterAdmin(model);

                if (result.Succeeded)
                {
                    return Ok(new { Status = "Success", Message = "User created successfully!" });
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: "User creation failed! " + errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while registering admin: " + ex.DetailErrorLog(), ex);
                return StatusCode(500, "An error occurred while registering admin.");
            }
        }


        //[HttpGet("ForgotPassword")]
        //public async Task<IActionResult> ForgotPassword(string username)
        //{
        //    try
        //    {
        //        await _authService.ForgetPassword(username);

        //        return Ok(true);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Forget password error:" + ex.DetailErrorLog(), ex);
        //        throw new Exception(ex.Message, ex);
        //    }
        //}

        //var callbackUrl = Url.Action(
        //    "ResetPassword",
        //    "Account",
        //    new { userId = user.Id },
        //    protocol: HttpContext.Request.Scheme
        //);

        //[HttpPost("ResetPassword")]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        //{
        //    try
        //    {
        //        var result = await _authService.ResetPassword(model);
        //        if (result.Succeeded)
        //        {
        //            return Ok("Password reset successful.");
        //        }
        //        else
        //        {
        //            return BadRequest("Failed to reset password: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Error while resetting password: " + ex.DetailErrorLog(), ex);
        //        return StatusCode(500, "An error occurred while resetting the password.");
        //    }
        //}


        [HttpGet("ChangePassword")]
        public async Task<IActionResult> ChangePassword(string username, string oldPassword, string newPassword)
        {
            try
            {
                var result = await _authService.ChangePassword(username, oldPassword, newPassword);

                if (result.Succeeded)
                {
                    return Ok("Password changed successfully.");
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BadRequest("Failed to change password: " + errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while password reset" + ex.DetailErrorLog(), ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //[HttpPost]
        ////[ServiceFilter(typeof(SuperAdminAuthorizationFilter))]
        //[Route("Register")]
        //public async Task<IActionResult> Register([FromBody] RegisterModel model)
        //{
        //    try
        //    {
        //        var result = await _authService.RegisterUser(model);
        //        if (!result.Succeeded)
        //            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

        //        return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Error while customer register" + ex.DetailErrorLog(), ex);
        //        throw new Exception(ex.Message, ex);
        //    }
        //}

        //[HttpGet("RefreshToken")]
        //public async Task<bool> RefreshToken(string username, string email)
        //{
        //    return true;
        //}
    }
}