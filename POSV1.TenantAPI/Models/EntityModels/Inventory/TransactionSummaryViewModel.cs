namespace POSV1.TenantAPI.Models.EntityModels.Inventory
{
    public class TransactionSummaryViewModel
    {
        // Totals
        public int TotalSalesCount { get; set; }
        public decimal TotalSalesAmount { get; set; }
        public int TotalPurchaseCount { get; set; }
        public decimal TotalPurchaseAmount { get; set; }
        public int TotalSalesReturnCount { get; set; }
        public decimal TotalSalesReturnAmount { get; set; }
        public int TotalPurchaseReturnCount { get; set; }
        public decimal TotalPurchaseReturnAmount { get; set; }

        // Top Selling Items by Count
        public List<ItemSummaryViewModelV1> TopSellingItemsByCount { get; set; } = new List<ItemSummaryViewModelV1>();

        // Top Selling Items by Amount
        public List<ItemSummaryViewModelV1> TopSellingItemsByAmount { get; set; } = new List<ItemSummaryViewModelV1>();

        // Top Customers by Sales Count
        public List<CustomerSummaryViewModel> TopCustomersBySalesCount { get; set; } = new List<CustomerSummaryViewModel>();

        // Top Customers by Sales Volume
        public List<CustomerSummaryViewModel> TopCustomersBySalesVolume { get; set; } = new List<CustomerSummaryViewModel>();

        // Monthly Sales Chart Data (12-Month Trend)
        public List<SalesChartViewModel> SalesChart { get; set; } = new List<SalesChartViewModel>();
    }

    public class ItemSummaryViewModelV1
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public decimal TotalSalesQuantity { get; set; }
        public decimal TotalSalesAmount { get; set; }
    }

    public class CustomerSummaryViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int SalesCount { get; set; }
        public decimal SalesVolume { get; set; }
    }

    public class SalesChartViewModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalSalesAmount { get; set; }
    }
}
