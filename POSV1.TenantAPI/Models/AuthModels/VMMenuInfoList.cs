namespace POSV1.TenantAPI.Models
{
    public partial class VMMenuInfoList
    {
        public int Id { get; set; }
        public string Controller_Name { get; set; }
        public string Controller_Caption { get; set;}
        public string Module { get; set;}
        public virtual IList<VMActions> VMActions { get; set;}
    }

    public partial class VMActions
    {
        public int Id { get; set; }
        public string Action_Name { get; set; }
        public string Action_Caption { get; set;}
    }
}
