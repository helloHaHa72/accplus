using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace POSV1.TenantAPI.Models
{
    public partial class VMUserList
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Status { get; set; }
        public virtual IList<VMUserRole> user_Roles { get; set; }
    }

    public partial class VMUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public virtual IList<VMUserRole> user_Roles { get; set; }
        public string BranchCode { get; set; }
    }

    public partial class VMUserRole
    {
        public string Name { get; set; }
    }

    public partial class VMUserDetail
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public virtual IList<VMUserRoleDetail> user_RolesDetail { get; set; }
    }

    public partial class VMUserRoleDetail
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
