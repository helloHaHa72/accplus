using System.ComponentModel.DataAnnotations;

namespace POSV1.TenantAPI.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? PhoneNumber { get; set; }
        public string Password { get; set; }
        public string[] UserType { get; set; }
        public string BranchCode { get; set; }
    }
}
