using POSV1.MasterDBModel;
using POSV1.TenantModel;

namespace POSV1.TenantAPI.Models
{
    public partial class VMRoles
    {
        public string Name { get; set; }
        public List<EnumSubCategory> PermissionIds { get; set; } = new List<EnumSubCategory>();
    }
}
