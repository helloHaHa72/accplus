using POSV1.TenantModel.Models;

namespace POSV1.TenantAPI.Models
{
    public class VMConfigList
    {
        public int id { get; set; }
        public string key { get; set; }
        public object value { get; set; }
    }

    public partial class VMConfig
    {
        public Enum key { get; set; }
        public object value { get; set; }
    }

    public class VMSystemConfig
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value {  get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public bool? CanBeEdited {  get; set; }
    }
}
