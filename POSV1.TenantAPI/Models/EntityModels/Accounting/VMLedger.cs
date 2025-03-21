namespace POSV1.TenantAPI.Models
{
    public partial class VMLedger
    {
        public virtual int ID { get; set; }
        
        public virtual string LedgerName { get; set; }

        public virtual string Code { get; set; }
        public string LedgerNameCode { get; set; }

        public virtual GLType GLType { get; set; }

        public virtual string GLTypeName { get; set; } 

        public virtual string Description { get; set; }

        public virtual int ParentGLID { get; set; }

        public virtual string ParentGLName { get; set; }
        public decimal Balance { get; set; }
        public bool IsDefault { get; set; }
        public virtual bool Status { get; set; }
    }

    public class VMUserStatement
    {
        public string VoucherNo { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public string ChequeNo { get; set; }
        public DateTimeOffset Date { get; set; }
    }

    public class VMLedgerTypes
    {
        public int Id {  get; set; }
        public string Title {  get; set; }
    }


    public enum GLType
    {
        Assets = 1 ,
        Liabilities = 2,
        Income = 3,
        Expenses = 4
    }
}


