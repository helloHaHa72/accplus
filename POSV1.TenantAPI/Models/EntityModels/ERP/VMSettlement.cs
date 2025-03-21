namespace POSV1.TenantAPI.Models
{
    public partial class VMSettlement
    {
        public int ID { get; set; }
        public DateTime Settlement_Date { get; set; }
        public string Remarks { get; set; }
        public virtual IList<VMSettlementDetails> VMSettlementDetails { get; set; }
    }

    public partial class VMSettlementDetails
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string Ref_No { get; set; }
        public decimal Paid_Amt { get; set; }
        public decimal? Received_Amt { get; set; }
        public string Remarks { get; set; }
        public bool Is_Verified { get; set; }
        public bool Status { get; set; }
    }
}
