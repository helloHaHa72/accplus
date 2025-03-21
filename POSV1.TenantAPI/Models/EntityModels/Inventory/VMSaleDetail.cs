namespace POSV1.TenantAPI.Models
{
    public partial class VMSaleDetail
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        //public int CustomerId { get; set; }
        //public string CustomerName { get; set; }
        public VMViewCustomerData CustomerData { get; set; }
        public string Invoice_No { get; set; }
        public string Remarks { get; set; }
        public decimal Sub_Total { get; set; }
        public decimal Disc_Amt { get; set; }
        public decimal Disc_Percentage { get; set; }
        public decimal Total {get; set; }
        public double VAT_Per {get; set; }
        public decimal VAT_Amt {get; set; }
        public decimal Net_Amt { get; set; }
        public string VoucherNo { get; set; }
        public bool IsVoucherLinked { get; set; }
        public string Net_Amt_Words { get; set; }
        public IList<VMSaleItemDetails> VMSaleItemDetails { get; set; }
    }

    public partial class VMSaleItemDetails
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public int UnitID { get; set; }
        public string HsCode { get; set; }
        public double Ratio { get; set; }
        public string UnitName { get; set; }
        public decimal Rate { get; set; }
        public double Sub_Total { get; set; }
        public decimal Disc_Amt { get; set; }
        public double Net_Amt {get; set; }
        public int? Driver_Id { get; set; }
        public string? Driver_Name { get; set; }
        public int? Vehicle_Id { get; set; }
        public string? Vechile_Number { get; set; }
        public string Destination { get; set; }
        public string Chalan_No { get; set; }
        public decimal? transportation_Fee { get; set; }
        //public double? VatAmt { get; set; }
        public double? VatPer {  get; set; }
        public bool IsVatApplied { get; set; }
        public double? VatAmt
        {
            get
            {
                return IsVatApplied && VatPer.HasValue ? (Net_Amt * VatPer.Value) / 100 : (double?)null;
            }
        }
    }
}
