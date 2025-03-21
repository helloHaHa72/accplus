using POSV1.TenantAPI.Models.EntityModels.Inventory;
using POSV1.TenantAPI.Models.EntityModels.Production;

namespace POSV1.TenantAPI.Models
{
    public partial class VMPurchaseDetail
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public VMViewVendorData VendorData { get; set; }
        //public int VendorId { get; set; }
        //public string VendorName { get; set; }
        public string Invoice_No { get; set; }
        public string Remarks { get; set; }
        public decimal Sub_Total { get; set; }
        public decimal Disc_Amt { get; set; }
        public decimal Disc_Percentage { get; set; }
        public decimal Additional_Disc_Amt { get; set; }
        public decimal Total { get; set; }
        public double VAT_Per { get; set; }
        public decimal VAT_Amt { get; set; }
        public decimal Net_Amt { get; set; }
        public bool VatApplicable { get; set; }
        public bool VatClaimable { get; set; }
        public string Net_Amt_Words { get; set; }
        public decimal AdditionalCharges {  get; set; }
        public string VoucherNo { get; set; }
        public bool IsVoucherLinked { get; set; }
        public IList<VMPurchaseItemDetails> VMPurchaseItemDetails { get; set; }
        public IList<ViewChargeData> AdditionalChargeList { get; set; } = new List<ViewChargeData>();
    }

    public class VMPurchaseDetailWithReturnDetails
    {
        public VMPurchaseDetail VMPurchaseDetail { get; set; } // Main purchase details
        public IList<VMPurchaseReturnDetail> VMPurchaseReturnDetail { get; set; } // Purchase return details
        public List<GroupedReturnItem> GroupedReturnItems { get; set; } = new List<GroupedReturnItem>();
    }

    public class GroupedReturnItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal TotalQuantity { get; set; }
    }

    public partial class VMPurchaseItemDetails
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public string HsCode { get; set; }
        public double Ratio { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public decimal Rate { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Mgf_Date { get; set; }
        public DateTime? Exp_Date { get; set; }
        public string Batch_No { get; set; }
        public decimal Disc_Amt { get; set; }
        public decimal Net_Amt { get; set; }
    }
}
