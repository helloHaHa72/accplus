using POSV1.TenantModel;

namespace POSV1.MasterDBModel.AuthModels;

public class AccessList
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public EnumMainHeading MainHeading { get; set; }
    public string CreatedBy { get; set; } = null!;
}
