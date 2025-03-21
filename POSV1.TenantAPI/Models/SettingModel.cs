using BaseAppSettings;
using POSV1.TenantModel.Modules;

namespace POSV1.TenantAPI.Models
{
    public class SettingModel
    {
        public int ID { get; set; }
        public EnumModules Modules { get; set; }
        public string Values { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
