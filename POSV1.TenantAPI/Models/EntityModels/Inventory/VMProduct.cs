using POSV1.TenantAPI.Models.EntityModels.Inventory;

namespace POSV1.TenantAPI.Models
{ 
    public partial class VMProduct
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string HsCode { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set;}
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int? UnitID { get; set; }
        public string UnitName { get; set; }
        public decimal Last_CP { get; set; }
        public decimal Last_SP { get; set; }
        public double Opening_Qty { get; set; }
        public string Ledger_Code { get; set; }
        //public IFormFile file { get; set; }
        public bool Status { get; set; }
        public bool Enable_Stock { get; set; }
        public bool Is_Taxable { get; set; }
        public bool? HasMultipleUnits { get; set; }
        public IList<VMViewUnit> Units {  get; set; }
    }

    public partial class CreateVMProduct
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string HsCode { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int? UnitID { get; set; }
        public string UnitName { get; set; }
        public decimal Last_CP { get; set; }
        public decimal Last_SP { get; set; }
        public double Opening_Qty { get; set; }
        //public string Ledger_Code { get; set; }
        //public IFormFile file { get; set; }
        public bool Status { get; set; }
        public bool Enable_Stock { get; set; }
        public bool Is_Taxable { get; set; }
        public bool hasMultipleUnit {  get; set; }
        public IList<VMUnitRelation>? Units {  get; set; }
    }

    public class TransactionDetailViewModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate {  get; set; }
        public string TransactionType { get; set; }
        public DateTimeOffset Date { get; set; }
        public string VendorName { get; set; }
        public int VendorId { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public string VoucherNumber { get; set; } = null;
    }

    public class ItemSummaryViewModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal TotalPurchaseQuantity { get; set; }
        public decimal TotalPurchaseReturnQuantity { get; set; }
        public decimal TotalSalesQuantity { get; set; }
        public decimal TotalSalesReturnQuantity { get; set; }
        public int TotalTransactionCount { get; set; }
        public decimal TotalAmount { get; set; }
    }


    public class ItemTransactionSummaryViewModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal TotalPurchaseQuantity { get; set; }
        public decimal TotalPurchaseReturnQuantity { get; set; }
        public decimal TotalSalesQuantity { get; set; }
        public decimal TotalSalesReturnQuantity { get; set; }
        public decimal BalanceQuantity => TotalPurchaseQuantity - TotalPurchaseReturnQuantity - TotalSalesQuantity + TotalSalesReturnQuantity;
        public List<TransactionDetailViewModel> Transactions { get; set; } = new List<TransactionDetailViewModel>();
    }
}
