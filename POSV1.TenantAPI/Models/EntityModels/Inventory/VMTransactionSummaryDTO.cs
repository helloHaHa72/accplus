namespace POSV1.TenantAPI.Models.EntityModels.Inventory
{
    public class VMTransactionSummaryDTO
    {
    }

    public class TransactionSummaryDTO
    {
        public decimal TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public int TransactionCount { get; set; }
        public decimal TransactionPercentage { get; set; }
    }

    public class TransactionTypeSummaryDTO
    {
        public TransactionSummaryDTO Purchase { get; set; }
        public TransactionSummaryDTO PurchaseReturn { get; set; }
        public TransactionSummaryDTO Sale { get; set; }
        public TransactionSummaryDTO SaleReturn { get; set; }
        public TransactionSummaryDTO ReceivedCash { get; set; }  // New
        public TransactionSummaryDTO PaidCash { get; set; }
    }
}
