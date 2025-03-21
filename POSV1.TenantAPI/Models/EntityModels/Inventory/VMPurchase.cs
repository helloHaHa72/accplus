using POSV1.TenantAPI.Models.EntityModels.Production;

namespace POSV1.TenantAPI.Models
{
    public partial class VMPurchase
    {
        public int Id { get; set; }
        public DateTime Purchase_Date { get; set; }
        public int VendorID { get; set; }
        public string Invoice_No { get; set; }
        public string Remarks { get; set; }
        public decimal Sub_Total => (decimal)VMPurchaseItems.Select(x => x.Net_Amt).DefaultIfEmpty(0).Sum();
        public decimal Disc_Amt { get; set; }
        public decimal Disc_Percentage { get; set; }
        public bool VatApplicable { get; set; }
        public bool VatClaimable { get; set; }
        public decimal Additional_Disc_Amt => 0;
        public decimal Total => (Sub_Total - Disc_Amt  - Additional_Disc_Amt);
        //public double VAT_Per => AppSettingsWrapper.AppSettings.VatPercent;
        public double VAT_Per => 0;
        public decimal VAT_Amt => (decimal)Total * (decimal)(VAT_Per * 0.01);
        public decimal Net_Amt => Total + VAT_Amt;
        public virtual IList<VMPurchaseItems> VMPurchaseItems { get; set; } = new List<VMPurchaseItems>();
        public virtual IList<ChargeData> AdditionalCharge { get; set; } = new List<ChargeData>();
    }

    public partial class VMPurchaseItems
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public int UnitID { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount => (decimal)Quantity * Rate;
        public DateTime? Mgf_Date { get; set; }
        public DateTime? Exp_Date { get; set; }
        public string? Batch_No { get; set; }
        //public decimal Disc_Amt => Amount * (decimal)0.05;
        public decimal Disc_Amt => 0;
        public decimal Net_Amt => Amount - Disc_Amt;
    }
}
