namespace POSV1.TenantAPI.Models
{
    public class VMSettlementListDetail
    {
        public int ID { get; set; }
        public DateTime _Date { get; set; }
        public string Settlement_Date => _Date.ToString("yyyy/MM/dd");
        public string Remarks { get; set; }
        public string Verified_By { get; set; }
        public virtual IList<VMSetDetails> VMSetDetails { get; set; }
    }

    public partial class VMSetDetails
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Ref_No { get; set; }
        public decimal Paid_Amt { get; set; }
        public decimal? Received_Amt { get; set; }
        public string Remarks { get; set; }
        public bool Is_Verified { get; set; }
        public bool Status { get; set; }
    }
}
