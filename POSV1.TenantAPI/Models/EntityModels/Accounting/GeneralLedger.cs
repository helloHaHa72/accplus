namespace POSV1.TenantAPI.Models
{
    public partial class GeneralLedger
    {
        public int? ID;

        public string GlName;

        public string Code;

        public EnumGLType GLType;

        public string Description;

        public int? ParentGLId;

        public string? ParentGLName;

        public bool Status;
    }

    public enum EnumGLType
    {
        Assets,
        Liabilities,
        Income,
        Expenses
    }
}
