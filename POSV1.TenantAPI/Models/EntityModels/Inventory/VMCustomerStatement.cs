namespace POSV1.TenantAPI.Models
{
    public class VMCustomerStatement
    {
        public int ID { get; set; }
        public DateTime _txnDate { get; set; }
        public string TxnDate => _txnDate.ToString("yyyy/MM/dd");
        public string Bill_No { get; set; }
        public decimal Total_Amt { get; set; }
        public string Remarks { get; set; }
    }

    public partial class VMCustomerStatementSummary
    {
        public string CustomerName { get; set; }
        public DateTime _txnDate { get; set; }
        public string TxnDate => _txnDate.ToString("yyyy/MM/dd");
        public string Bill_No { get; set; }
        public string Remarks { get; set; }
        public IList<VMCustomerStatementDetail> VMCustomerStatementDetail { get; set; }
    }

    public partial class VMCustomerStatementDetail
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public decimal Rate { get; set; }
        public double Quantity { get; set; }
        public double Sub_Total { get; set; }
        public decimal Disc_Amt { get; set; }
        public double Net_Amt { get; set; }
    }
}
