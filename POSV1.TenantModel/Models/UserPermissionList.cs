using BaseAppSettings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel;

namespace POSV1.MasterDBModel.AuthModels;

public class UserPermissionList : Auditable
{
    public int Id { get; set; }
    public string roleId { get; set; } = null!;
    public EnumSubCategory AccesListId { get; set; }
}
