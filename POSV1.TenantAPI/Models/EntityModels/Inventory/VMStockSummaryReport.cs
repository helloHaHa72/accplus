namespace POSV1.TenantAPI.Models
{
    public partial class VMStockSummaryReport
    {
        public int? CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Purchase_Qty { get; set; }
        public decimal Purchase_Amt { get; set;}
        public decimal Purchase_Return_Qty { get; set;}
        public decimal Purchase_Return_Amt { get; set;}
        public decimal Sale_Return_Qty { get; set; }
        public decimal Sale_Return_Amt { get; set; }
        public decimal Sale_Qty { get; set; }
        public decimal Sale_Amt { get; set; }
        public int TotalTransactionCount { get; set; }
        //public decimal Balance_Qty => Purchase_Qty - Sale_Qty ;
        //public decimal Balance_Amt => Purchase_Amt - Sale_Amt ;
        //public decimal Profit => Sale_Amt - Purchase_Amt;
        public decimal Balance_Qty => Purchase_Qty - Purchase_Return_Qty - (Sale_Qty - Sale_Return_Qty);
        public decimal Balance_Amt => (Purchase_Amt - Purchase_Return_Amt) - (Sale_Amt - Sale_Return_Amt);
        public decimal Profit => (Sale_Amt - Sale_Return_Amt) - (Purchase_Amt - Purchase_Return_Amt);
    }

    public class VMStockSummaryOverallReport
    {
        public decimal TotalPurchaseQty { get; set; }
        public decimal TotalPurchaseAmt { get; set; }
        public decimal TotalPurchaseReturnQty { get; set; }
        public decimal TotalPurchaseReturnAmt { get; set; }
        public decimal TotalSaleQty { get; set; }
        public decimal TotalSaleAmt { get; set; }
        public decimal TotalSaleReturnQty { get; set; }
        public decimal TotalSaleReturnAmt { get; set; }
        public int TotalTransactionCount { get; set; }
    }

    public class AllItemTransactionsResult
    {
        public List<ItemSummaryViewModel> Summaries { get; set; } = new List<ItemSummaryViewModel>();
        public List<TransactionDetailViewModel> Transactions { get; set; } = new List<TransactionDetailViewModel>();
        public int OverallTransactionCount { get; set; } = 0; // Make this mutable
    }
}


