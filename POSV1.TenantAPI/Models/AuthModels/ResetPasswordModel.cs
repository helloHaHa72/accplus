namespace POSV1.TenantAPI.Models
{
    public partial class ResetPasswordModel
    {
        public string Username { get; set; }
        public string OTP { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
