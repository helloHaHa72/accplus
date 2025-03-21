using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantAPI.Models
{
    public partial class VMSaleList
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Invoice_No { get; set; }
        public string Remarks { get; set; }
        public decimal Sub_Total { get; set; }
        public decimal Disc_Amt { get; set; }
        public decimal Disc_Percentage { get; set; }
        public decimal Total { get; set; }
        public double VAT_Per { get; set; }
        public decimal VAT_Amt { get; set; }
        public decimal Net_Amt { get; set; }
        public string VoucherNo { get; set; }
        public bool IsVoucherLinked { get; set; }
    }

    public partial class VMSaleDetailList
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Invoice_No { get; set; }
        public string ItemName { get; set; }
        public int itemId { get; set; }
        public string UnitName { get; set; }
        public double Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Sub_Total { get; set; }
        public decimal Disc_Amt { get; set; }
        public decimal Disc_Percentage { get; set; }
        public decimal Total { get; set; }
        public string VoucherNo { get; set; }
        public bool IsVoucherLinked { get; set; }
    }
}
