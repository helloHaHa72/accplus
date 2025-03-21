namespace POSV1.TenantAPI.Models
{
    public partial class VMSale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public string Invoice_No { get; set; }
        public string Remarks { get; set; }
        public decimal Sub_Total => (decimal)VMSaleItem.Select(x => x.Net_Amt).DefaultIfEmpty(0).Sum();
        public decimal Disc_Amt { get; set; }
        public decimal Disc_Percentage { get; set; }
        public decimal Total => (Sub_Total - Disc_Amt); 
        //public double VAT_Per => AppSettingsWrapper.AppSettings.VatPercent;
        //public decimal VAT_Amt => (decimal)Total * (decimal)(VAT_Per * 0.01);
        //public decimal Net_Amt => Total + VAT_Amt;
        public IList<VMSaleItem> VMSaleItem { get; set; }
    }

    public partial class VMSaleItem
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public double Quantity { get; set; }
        public int UnitID { get; set; }
        public decimal Rate { get; set; }
        public double Sub_Total => Quantity * (double)Rate + (double)Transportation_Fee;
        public decimal Disc_Amt => 0;
        public double Net_Amt => Sub_Total - (double)Disc_Amt;
        public bool IsVatApplied {  get; set; }
        public int? DriverId { get; set; }
        public int? VechileId { get; set; }
        public string Destination { get; set; }
        public string Chalan_No { get; set; }
        public decimal Transportation_Fee { get; set; }
    }

}


