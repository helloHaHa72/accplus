using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantAPI.Models.AuthModels
{
    public partial class VMRoleActions
    {
        public string RoleId { get; set; }
        public int[] ActionId { get; set; }
    }
}
