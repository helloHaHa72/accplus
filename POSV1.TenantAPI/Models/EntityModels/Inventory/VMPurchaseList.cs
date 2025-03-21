namespace POSV1.TenantAPI.Models
{
    public partial class VMPurchaseList
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string Invoice_No { get; set; }
        public string Remarks { get; set; }
        public decimal Sub_Total { get; set; }
        public decimal Disc_Amt { get; set; }
        public decimal Disc_Percentage { get; set; }
        public decimal Additional_Disc_Amt { get; set; }
        public string VoucherNo { get; set; }
        public bool IsVoucherLinked { get; set; }
        public decimal Total { get; set; }
        public double VAT_Per { get; set; }
        public decimal VAT_Amt { get; set; }
        public decimal Net_Amt { get; set; }
        public decimal AdditionalCharge {  get; set; }
    }

    public partial class VMPurchaseDetailList
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string Invoice_No { get; set; }
        public string ItemName {  get; set; }
        public int itemId {  get; set; }
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
