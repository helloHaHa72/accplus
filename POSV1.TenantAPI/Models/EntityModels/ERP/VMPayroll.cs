namespace POSV1.TenantAPI.Models
{
    public partial class VMPayroll
    {
        public DateTime Transaction_Date { get; set; }
        public DateTime Month { get; set; }
        public string Remarks { get; set; }
        public virtual IList<VMPayrollDetails> VMPayrollDetails { get; set; }
    }

    public partial class VMPayrollDetails
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string? Ref_No { get; set; }
        public decimal Paid_Amt { get; set; }
        public string Remarks { get; set; }
        public virtual bool Status { get; set; }
    }
}
