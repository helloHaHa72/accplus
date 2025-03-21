using POSV1.TenantAPI.Models.EntityModels.Inventory;
using POSV1.TenantModel.Models;

namespace POSV1.TenantAPI.Models
{
    public partial class VMInventoryDayBook
    {
        public IList<VMItemWiseSales> ItemWiseSalesRecords { get; set; }
        public IList<VMItemWiseSales> ItemWiseSalesReturnRecord { get; set; }
        public IList<VMItemWisePurchase> ItemWisePurchaseRecords { get; set; }
        public IList<VMItemWisePurchase> ItemWisePurhcaseReturnRecord { get; set; }
        public IList<VMCustomerWiseSales> CustomerWiseSalesRecords { get; set; }
        public IList<VMVendorWisePurchase> VendorWisePurchaseRecords { get; set; }
        public IList<VMCashSettlementCustomerWise> UserWiseCashSettlementRecord { get; set; }
    }

    public partial class VMCustomerWiseSales
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Bill_No { get; set; }
        public decimal Total_Amt { get; set; }
        public string Remarks { get; set; }
    }

    public partial class VMVendorWisePurchase
    {
        public int Id { get; set; }
        public string VendorName { get; set; }
        public string Bill_No { get; set; }
        public decimal Total_Amt { get; set; }
        public string Remarks { get; set; }
    }

    public partial class VMItemWiseSales
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string Ref_No { get; set; }
        public decimal Rate { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public decimal Net_Amt { get; set; }
    }
    public partial class VMItemWisePurchase
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string Ref_No { get; set; }
        public decimal Rate { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public decimal Net_Amt { get; set; }
    }
   
}
