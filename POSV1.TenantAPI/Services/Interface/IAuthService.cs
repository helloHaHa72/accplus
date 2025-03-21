using Microsoft.AspNetCore.Identity;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Models.AuthModels;
using static POSV1.TenantAPI.Services.Implementation.AuthService;

namespace POSV1.TenantAPI.Services.Interface
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUser(RegisterModel model, int TenantId);
        //Task<IdentityResult> ResetPassword(ResetPasswordModel model);
        Task<LoginResult> Login(LoginModel model);
        //Task ForgetPassword(string username);
        Task<IdentityResult> RegisterAdmin(RegisterModel model);
        Task SeedDefaultData();
        Task<IdentityResult> ChangePassword(string username, string oldPassword, string newPassword);
        bool IsTokenValid(string token);
    }
}
