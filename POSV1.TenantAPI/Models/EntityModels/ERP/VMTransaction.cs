namespace POSV1.TenantAPI.Models
{
    public partial class VMTransaction
    {

        public virtual DateTime Transaction_Date { get; set; }

        public virtual string Remarks { get; set; }

        public virtual decimal Total_Amt => VMTransactionDetails.Select(x => x.Amount).DefaultIfEmpty(0).Sum();

        public virtual IList<VMTransactionDetails> VMTransactionDetails { get; set; }
    }



    public partial class VMTransactionDetails
    {
        public int ID { get; set; }

        public int EmployeeID { get; set; }

        public string? Ref_No { get; set; }

        public decimal Amount { get; set; }

        public string Remarks { get; set; }

        public string RefrenceLedgerCode {  get; set; }

        public bool Status { get; set; }
    }

}
