using System.ComponentModel.DataAnnotations.Schema;

namespace POSV1.TenantAPI.Models
{
    public partial class VMVendorStatement
    {
        public int Id { get; set; }
        public DateTime _txnDate { get; set; }
        public string TxnDate => _txnDate.ToString("yyyy/MM/dd");
        public string Bill_No { get; set; }
        public decimal Total_Amt { get; set; }
        public string Remarks { get; set; }
    }

    public partial class VMVendorStatementSummary
    {
        public string VendorName { get; set; }
        public DateTime _txnDate { get; set; }
        public string TxnDate => _txnDate.ToString("yyyy/MM/dd");
        public string Bill_No { get; set; }
        public string Remarks { get; set; }
        public IList<VMVendorStatementDetail> VMVendorStatementDetail { get; set; }
    }

    public partial class VMVendorStatementDetail
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public decimal Rate { get; set; }
        public decimal Quantity { get; set; }
        public decimal? Sub_Total { get; set; }
        public string Batch_No { get; set; }
        public decimal Disc_Amt { get; set; }
        public decimal Net_Amt { get; set; }
    }

}
