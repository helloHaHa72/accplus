using BaseAppSettings;

namespace POSV1.TenantModel.Models.EntityModels.Settings
{
    public class MainSetup : Auditable
    {
        public int Id { get; set; }
        public string OrgName { get; set; } = null!;
        public string? Logo { get; set; }
        public string? Server { get; set; }
        public string? DbName { get; set; }
        public string? DbPassword { get; set; }
    }
}
