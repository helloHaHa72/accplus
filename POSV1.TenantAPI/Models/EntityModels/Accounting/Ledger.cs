namespace POSV1.TenantAPI.Models
{
    public partial class Ledger
    {
        public int ID;
        
        public string LedgerName;

        public string Code;

        public EnumGLType GLType;

        public string Description;

        public int ParentGLId;

        public string? ParentGLName;

        public string Status;
    }

    public class LedgerDto
    {
        public string LedgerName { get; set; }
        public string Code { get; set; }
        public decimal OpenBalance { get; set; }
        public decimal Balance { get; set; }
        public decimal PrevBalance { get; set; }
    }

}
