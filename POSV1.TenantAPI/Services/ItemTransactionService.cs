using Microsoft.EntityFrameworkCore;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantAPI.Services
{
    public interface IItemTransactionService
    {
        Task<TransactionSummaryViewModel> GetTransactionSummary(DateTime? startDate, DateTime? endDate);
    }
    public class ItemTransactionService : IItemTransactionService
    {
        private readonly IProductsRepo _productsRepo;
        private readonly IPurchaseItemsRepo _purchaseItemsRepo;
        private readonly IPurchasereturnitemsRepo _purchaseReturnitemsRepo;
        private readonly ISaleItemsRepo _saleItemsRepo;
        private readonly ISalesItemReturnRepo _salesItemReturnRepo;

        public ItemTransactionService(
            IProductsRepo productsRepo,
            IPurchaseItemsRepo purchaseItemsRepo,
            IPurchasereturnitemsRepo purchaseReturnitemsRepo,
            ISaleItemsRepo saleItemsRepo,
            ISalesItemReturnRepo salesItemReturnRepo)
        {
            _productsRepo = productsRepo;
            _purchaseItemsRepo = purchaseItemsRepo;
            _purchaseReturnitemsRepo = purchaseReturnitemsRepo;
            _saleItemsRepo = saleItemsRepo;
            _salesItemReturnRepo = salesItemReturnRepo;
        }

        public async Task<TransactionSummaryViewModel> GetTransactionSummary(DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && !endDate.HasValue)
            {
                endDate = startDate.Value.Date.AddDays(1).AddTicks(-1);
            }

            // Fetch products
            var allProducts = await _productsRepo.GetList().ToListAsync();
            if (!allProducts.Any())
                throw new Exception("No products found.");

            // Fetch all sales, purchases, and returns within the date range
            var salesData = await _saleItemsRepo
                .GetList()
                .Include(x => x.un01units)
                .Include(x => x.sal01sales)
                    .ThenInclude(x => x.cus01customers)
                .Where(s => (!startDate.HasValue || s.DateCreated >= startDate)
                            && (!endDate.HasValue || s.DateCreated <= endDate))
                .ToListAsync();

            var purchaseData = await _purchaseItemsRepo
                .GetList()
                .Include(x => x.pur01purchases)
                    .ThenInclude(x => x.ven01vendors)
                .Where(p => (!startDate.HasValue || p.DateCreated >= startDate)
                            && (!endDate.HasValue || p.DateCreated <= endDate))
                .ToListAsync();

            var salesReturnData = await _salesItemReturnRepo
                .GetList()
                .Include(x => x.sal01salesreturn)
                    .ThenInclude(x => x.cus01customers)
                .Where(sr => (!startDate.HasValue || sr.DateCreated >= startDate)
                            && (!endDate.HasValue || sr.DateCreated <= endDate))
                .ToListAsync();

            var purchaseReturnData = await _purchaseReturnitemsRepo
                .GetList()
                .Include(x => x.pur01purchasereturns)
                    .ThenInclude(x => x.ven01vendors)
                .Where(pr => (!startDate.HasValue || pr.DateCreated >= startDate)
                            && (!endDate.HasValue || pr.DateCreated <= endDate))
                .ToListAsync();

            // Aggregate data
            var summary = new TransactionSummaryViewModel
            {
                TotalSalesCount = salesData.Count,
                TotalSalesAmount = salesData.Sum(s => (decimal)s.sal02qty * s.sal02rate),
                TotalPurchaseCount = purchaseData.Count,
                TotalPurchaseAmount = purchaseData.Sum(p => p.pur02qty * p.pur02rate),
                TotalSalesReturnCount = salesReturnData.Count,
                TotalSalesReturnAmount = salesReturnData.Sum(sr => (decimal)sr.sal02qty * sr.sal02rate),
                TotalPurchaseReturnCount = purchaseReturnData.Count,
                TotalPurchaseReturnAmount = purchaseReturnData.Sum(pr => pr.pur02returnqty * pr.pur02returnrate)
            };

            // Top 5 max selling items by quantity
            summary.TopSellingItemsByCount = salesData
                .GroupBy(s => s.sal02pro02uin)
                .Select(g => new ItemSummaryViewModelV1
                {
                    ItemId = g.Key,
                    ItemName = allProducts.FirstOrDefault(p => p.pro02uin == g.Key)?.pro02name_eng ?? "Unknown Product",
                    Unit = g.FirstOrDefault()?.un01units?.un01name_eng ?? "Unknown Unit",
                    TotalSalesQuantity = g.Sum(x => (decimal)x.sal02qty)
                })
                .OrderByDescending(x => x.TotalSalesQuantity)
                .Take(5)
                .ToList();

            // Top 5 max selling items by amount
            summary.TopSellingItemsByAmount = salesData
                .GroupBy(s => s.sal02pro02uin)
                .Select(g => new ItemSummaryViewModelV1
                {
                    ItemId = g.Key,
                    ItemName = allProducts.FirstOrDefault(p => p.pro02uin == g.Key)?.pro02name_eng ?? "Unknown Product",
                    Unit = g.FirstOrDefault()?.un01units?.un01name_eng ?? "Unknown Unit",
                    TotalSalesAmount = g.Sum(x => (decimal)x.sal02qty * x.sal02rate)
                })
                .OrderByDescending(x => x.TotalSalesAmount)
                .Take(5)
                .ToList();


            // Top 5 customers by sales count in a week
            var oneWeekAgo = DateTime.Now.AddDays(-7);
            summary.TopCustomersBySalesCount = salesData
                .Where(s => s.DateCreated >= oneWeekAgo)
                .GroupBy(s => s.sal01sales.cus01customers.cus01uin)
                .Select(g => new CustomerSummaryViewModel
                {
                    CustomerId = g.Key,
                    CustomerName = g.First().sal01sales.cus01customers.cus01name_eng,
                    SalesCount = g.Count()
                })
                .OrderByDescending(x => x.SalesCount)
                .Take(5)
                .ToList();

            // Top 5 customers by sales volume in a week
            summary.TopCustomersBySalesVolume = salesData
                .Where(s => s.DateCreated >= oneWeekAgo)
                .GroupBy(s => s.sal01sales.cus01customers.cus01uin)
                .Select(g => new CustomerSummaryViewModel
                {
                    CustomerId = g.Key,
                    CustomerName = g.First().sal01sales.cus01customers.cus01name_eng,
                    SalesVolume = g.Sum(x => (decimal)x.sal02qty * x.sal02rate)
                })
                .OrderByDescending(x => x.SalesVolume)
                .Take(5)
                .ToList();

            // 12-Month Sales Chart
            summary.SalesChart = salesData
                .GroupBy(s => new { s.DateCreated.Year, s.DateCreated.Month })
                .Select(g => new SalesChartViewModel
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalSalesAmount = g.Sum(x => (decimal)x.sal02qty * x.sal02rate)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            return summary;
        }
    }

}
