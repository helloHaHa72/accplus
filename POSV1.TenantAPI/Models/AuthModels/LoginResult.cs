using BaseAppSettings;

namespace POSV1.TenantAPI.Models.AuthModels
{
    public record struct ResLogin(string username, string TenatnId, string OrgName, string email, string[] associatedRoles, string token, DateTime expiration, string[] Permissions);

    public class LoginResult
    {
        public string Username { get; set; }
        //public string BranchCode { get; set; }
        public string Email { get; set; }
        public IList<string> AssociatedRoles { get; set; }
        public string TenantID { get; set; }
        public string? OrgName { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public List<string> Permissions { get; set; }
        public bool IsSuperAdmin => AssociatedRoles is not null && AssociatedRoles.Any(x => x == nameof(EnumApplicationUserType.SuperAdmin));
        public bool IsOrgAdmin => AssociatedRoles is not null && AssociatedRoles.Any(x => x == nameof(EnumApplicationUserType.GeneralAdmin));
    }

    //public class OrgInfo
    //{
    //    public string OrgName { get; set; }
    //}
}
