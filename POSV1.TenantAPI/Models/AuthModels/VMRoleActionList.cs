namespace POSV1.TenantAPI.Models.AuthModels
{
    public partial class VMRoleActionList
    {
        public string RoleId { get; set; }
        public string Role { get; set; }
        public IList<VMActionList> Actions { get; set; }
    }

    public partial class VMActionList
    {
        public int ActionId { get; set; }
        public string ActionCaption { get; set; }
    }
}
