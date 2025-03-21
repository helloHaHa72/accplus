using POSV1.TenantModel.Models;

namespace POSV1.TenantAPI.Models
{
    public partial class VMConfigurationList
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }


    public partial class VMConfiguration
    {
        public string module { get; set; }
        public enumConfigSettingsKeys Key { get; set; }
        public string Value { get; set; }
    }

    public class VMEditConfig
    {
        public enum ValueTypeEnum
        {
            Integer,
            Boolean,
            Decimal,
            String
        }

        public enumConfigSettingsKeys Key { get; set; }
        public string Value { get; set; }
    }
}
