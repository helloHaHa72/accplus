using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Models.EntityModels.Inventory;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;
using POSV1.TenantModel.Repo.Interface.Accounting;

namespace POSV1.TenantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockSummaryReportController : ControllerBase
    {
        private readonly ISaleItemsRepo _saleItemsRepo;
        private readonly ISalesItemReturnRepo _salesItemReturnRepo;
        private readonly IPurchaseItemsRepo _purchaseItemsRepo;
        private readonly IPurchasereturnitemsRepo _purchaseReturnitemsRepo;
        private readonly IProductsRepo _productsRepo;
        private readonly IProductCategoriesRepo _productCategoriesRepo;
        private readonly ICashSettlementRepo _cashSettlementRepo;
        private readonly ILedgersRepo _ledgersRepo;
        private readonly IConfiguration _configuration;
        public StockSummaryReportController(
            ISaleItemsRepo saleItemsRepo,
            ISalesItemReturnRepo salesItemReturnRepo,
            IPurchaseItemsRepo purchaseItemsRepo,
            IPurchasereturnitemsRepo purchasereturnitemsRepo,
            IProductsRepo productsRepo,
            IProductCategoriesRepo productCategoriesRepo,
            ICashSettlementRepo cashSettlementRepo,
            ILedgersRepo ledgersRepo,
            IConfiguration configuration)
        {
            _saleItemsRepo = saleItemsRepo;
            _salesItemReturnRepo = salesItemReturnRepo;
            _purchaseItemsRepo = purchaseItemsRepo;
            _purchaseReturnitemsRepo = purchasereturnitemsRepo;
            _productsRepo = productsRepo;
            _productCategoriesRepo = productCategoriesRepo;
            _cashSettlementRepo = cashSettlementRepo;
            _ledgersRepo = ledgersRepo;
            _configuration = configuration;
        }

        [HttpGet("GetStockSummary")]
        public async  Task<IActionResult> GetStockSummary()
        {
            var productDetails =await _productsRepo.GetList().ToListAsync();

            if (productDetails == null || !productDetails.Any())
            {
                return NotFound("No products found");
            }

            var stockSummaries = new List<VMStockSummaryReport>();

            foreach (var productDetail in productDetails)
            {
                var categoryDetail =await  _productCategoriesRepo.GetDetailAsync(productDetail.pro02pro01uin);             

                var purchaseItems = _purchaseItemsRepo.GetList()
                    .Where(item => item.pur02pro02uin == productDetail.pro02uin)
                    .ToList();

                var purchaseReturnItems = _purchaseReturnitemsRepo.GetList()
                    .Where(item => item.pur02returnpro02uin == productDetail.pro02uin)
                    .ToList();

                var saleItems = _saleItemsRepo.GetList()
                    .Where(item => item.sal02pro02uin == productDetail.pro02uin)
                    .ToList();

                var saleReturnItems = _salesItemReturnRepo.GetList()
                    .Where(item => item.sal02pro02uin == productDetail.pro02uin)
                    .ToList();

                var stockSummary = new VMStockSummaryReport
                {
                    CategoryID = categoryDetail.pro01uin,
                    CategoryName = categoryDetail.pro01name_eng,
                    ProductId = productDetail.pro02uin,
                    ProductName = productDetail.pro02name_eng,

                    Purchase_Qty = purchaseItems.Sum(item => item.pur02qty),
                    Purchase_Amt = purchaseItems.Sum(item => item.pur02net_amt),

                    Purchase_Return_Qty = purchaseReturnItems.Sum(item => item.pur02returnqty),
                    Purchase_Return_Amt = purchaseReturnItems.Sum(item => item.pur02net_amt),

                    Sale_Qty = (decimal)saleItems.Sum(item => item.sal02qty),
                    Sale_Amt = (decimal)saleItems.Sum(item => item.sal02net_amt),

                    Sale_Return_Qty = saleReturnItems.Sum(item => (decimal)item.sal02qty),
                    Sale_Return_Amt = saleReturnItems.Sum(item => (decimal)item.sal02net_amt),
                };

                stockSummaries.Add(stockSummary);
            }

            return Ok(stockSummaries);
        }

        [HttpGet("DayBookSummary")]
        public async Task<IActionResult> DayBookSummary(DateTime? startDate, DateTime? endDate)
        {
            // Use today's date if no date range is provided
            startDate ??= DateTime.Today;
            endDate ??= DateTime.Today;

            // Fetch all transactions within the date range
            var purchaseItems = await _purchaseItemsRepo.GetList()
                .Where(item => item.DateCreated >= startDate && item.DateCreated <= endDate)
                .ToListAsync();

            var purchaseReturnItems = await _purchaseReturnitemsRepo.GetList()
                .Where(item => item.DateCreated >= startDate && item.DateCreated <= endDate)
                .ToListAsync();

            var saleItems = await _saleItemsRepo.GetList()
                .Where(item => item.DateCreated >= startDate && item.DateCreated <= endDate)
                .ToListAsync();

            var saleReturnItems = await _salesItemReturnRepo.GetList()
                .Where(item => item.DateCreated >= startDate && item.DateCreated <= endDate)
                .ToListAsync();

            var receivedCash = await _cashSettlementRepo.GetList()
                .Where(item => item.cas01payment_type == TenantModel.EnumPaymentType.Received && item.DateCreated >= startDate && item.DateCreated <= endDate)
                .ToListAsync();

            var paidCash = await _cashSettlementRepo.GetList()
                .Where(item => item.cas01payment_type == TenantModel.EnumPaymentType.Paid && item.DateCreated >= startDate && item.DateCreated <= endDate)
                .ToListAsync();

            var gLedgerBank = _configuration["GeneralLedgerConfigurations:DefaultLedgerForBank"];
            decimal totalBankAmount = await _ledgersRepo
                .GetList()
                .Include(x => x.led03general_ledgers)
                .Where(x => x.led03general_ledgers.led03title == gLedgerBank)
                .SumAsync(x => x.led01balance);

            var gLedgerCash = _configuration["GeneralLedgerConfigurations:DefaultLedgerForCash"];
            decimal totalCashAmount = await _ledgersRepo
                .GetList()
                .Include(x => x.led03general_ledgers)
                .Where(x => x.led03general_ledgers.led03title == gLedgerCash)
                .SumAsync(x => x.led01balance);

            int totalTransactionCount = purchaseItems.Count + purchaseReturnItems.Count + saleItems.Count +
                                saleReturnItems.Count + receivedCash.Count + paidCash.Count;

            decimal CalculatePercentage(int count) =>
                totalTransactionCount == 0 ? 0 : Math.Round((decimal)count / totalTransactionCount * 100, 1);

            var overallSummary = new
            {
                Purchase = new TransactionSummaryDTO()
                {
                    TotalQuantity = purchaseItems.Sum(item => item.pur02qty),
                    TotalAmount = purchaseItems.Sum(item => item.pur02net_amt),
                    TransactionCount = purchaseItems.Count,
                    TransactionPercentage = CalculatePercentage(purchaseItems.Count)
                },
                PurchaseReturn = new TransactionSummaryDTO()
                {
                    TotalQuantity = purchaseReturnItems.Sum(item => item.pur02returnqty),
                    TotalAmount = purchaseReturnItems.Sum(item => item.pur02net_amt),
                    TransactionCount = purchaseReturnItems.Count,
                    TransactionPercentage = CalculatePercentage(purchaseReturnItems.Count)
                },
                Sale = new TransactionSummaryDTO()
                {
                    TotalQuantity = saleItems.Sum(item => (decimal)item.sal02qty),
                    TotalAmount = saleItems.Sum(item => (decimal)item.sal02net_amt),
                    TransactionCount = saleItems.Count,
                    TransactionPercentage = CalculatePercentage(saleItems.Count)
                },
                SaleReturn = new TransactionSummaryDTO()
                {
                    TotalQuantity = saleReturnItems.Sum(item => (decimal)item.sal02qty),
                    TotalAmount = saleReturnItems.Sum(item => (decimal)item.sal02net_amt),
                    TransactionCount = saleReturnItems.Count,
                    TransactionPercentage = CalculatePercentage(saleReturnItems.Count)
                },
                ReceivedCash = new TransactionSummaryDTO()
                {
                    TotalQuantity = 0,
                    TotalAmount = receivedCash.Sum(item => item.cas01amount),
                    TransactionCount = receivedCash.Count,
                    TransactionPercentage = CalculatePercentage(receivedCash.Count)
                },
                PaidCash = new TransactionSummaryDTO()
                {
                    TotalQuantity = 0,
                    TotalAmount = paidCash.Sum(item => item.cas01amount),
                    TransactionCount = paidCash.Count,
                    TransactionPercentage = CalculatePercentage(paidCash.Count)
                },
                AmountInBank = totalBankAmount,
                AmountInCash = totalCashAmount,
                TotalTransactionCount = purchaseItems.Count + purchaseReturnItems.Count + saleItems.Count + saleReturnItems.Count + receivedCash.Count + paidCash.Count
            };

            return Ok(overallSummary);
        }




        [HttpGet("GetStockDetailSummary")]
        public async Task<IActionResult> GetStockDetailSummary(DateTime? startDate, DateTime? endDate)
        {
            // Use today's date if no date range is provided
            if (startDate == null)
            {
                startDate = DateTime.Today;
            }

            if (endDate == null)
            {
                endDate = DateTime.Today;
            }

            // Fetch product details and ensure there is data available
            var productDetails = await _productsRepo.GetList().ToListAsync();

            if (productDetails == null || !productDetails.Any())
            {
                return NotFound("No products found");
            }

            var stockSummaries = new List<VMStockSummaryReport>();

            decimal totalPurchaseQty = 0;
            decimal totalPurchaseAmt = 0;
            decimal totalPurchaseReturnQty = 0;
            decimal totalPurchaseReturnAmt = 0;
            decimal totalSaleQty = 0;
            decimal totalSaleAmt = 0;
            decimal totalSaleReturnQty = 0;
            decimal totalSaleReturnAmt = 0;
            int overallTransactionCount = 0; // Overall transaction count

            foreach (var productDetail in productDetails)
            {
                var categoryDetail = await _productCategoriesRepo.GetDetailAsync(productDetail.pro02pro01uin);

                // Filter purchase items by date range
                var purchaseItems = _purchaseItemsRepo.GetList()
                    .Where(item => item.pur02pro02uin == productDetail.pro02uin
                                   && item.DateCreated >= startDate
                                   && item.DateCreated <= endDate)
                    .ToList();

                // Filter purchase return items by date range
                var purchaseReturnItems = _purchaseReturnitemsRepo.GetList()
                    .Where(item => item.pur02returnpro02uin == productDetail.pro02uin
                                   && item.DateCreated >= startDate
                                   && item.DateCreated <= endDate)
                    .ToList();

                // Filter sale items by date range
                var saleItems = _saleItemsRepo.GetList()
                    .Where(item => item.sal02pro02uin == productDetail.pro02uin
                                   && item.DateCreated >= startDate
                                   && item.DateCreated <= endDate)
                    .ToList();

                // Filter sale return items by date range
                var saleReturnItems = _salesItemReturnRepo.GetList()
                    .Where(item => item.sal02pro02uin == productDetail.pro02uin
                                   && item.DateCreated >= startDate
                                   && item.DateCreated <= endDate)
                    .ToList();

                // Check if the product has any transaction within the date range
                if (!purchaseItems.Any() && !purchaseReturnItems.Any() && !saleItems.Any() && !saleReturnItems.Any())
                {
                    continue;
                }

                // Calculate transaction counts
                int productTransactionCount = purchaseItems.Count
                                             + purchaseReturnItems.Count
                                             + saleItems.Count
                                             + saleReturnItems.Count;

                overallTransactionCount += productTransactionCount; // Add to overall transaction count

                var stockSummary = new VMStockSummaryReport
                {
                    CategoryID = categoryDetail.pro01uin,
                    CategoryName = categoryDetail.pro01name_eng,
                    ProductId = productDetail.pro02uin,
                    ProductName = productDetail.pro02name_eng,

                    Purchase_Qty = purchaseItems.Sum(item => item.pur02qty),
                    Purchase_Amt = purchaseItems.Sum(item => item.pur02net_amt),

                    Purchase_Return_Qty = purchaseReturnItems.Sum(item => item.pur02returnqty),
                    Purchase_Return_Amt = purchaseReturnItems.Sum(item => item.pur02net_amt),

                    Sale_Qty = (decimal)saleItems.Sum(item => item.sal02qty),
                    Sale_Amt = (decimal)saleItems.Sum(item => item.sal02net_amt),

                    Sale_Return_Qty = saleReturnItems.Sum(item => (decimal)item.sal02qty),
                    Sale_Return_Amt = saleReturnItems.Sum(item => (decimal)item.sal02net_amt),

                    TotalTransactionCount = productTransactionCount // Add total transaction count for this product
                };

                // Update overall totals
                totalPurchaseQty += stockSummary.Purchase_Qty;
                totalPurchaseAmt += stockSummary.Purchase_Amt;
                totalPurchaseReturnQty += stockSummary.Purchase_Return_Qty;
                totalPurchaseReturnAmt += stockSummary.Purchase_Return_Amt;
                totalSaleQty += stockSummary.Sale_Qty;
                totalSaleAmt += stockSummary.Sale_Amt;
                totalSaleReturnQty += stockSummary.Sale_Return_Qty;
                totalSaleReturnAmt += stockSummary.Sale_Return_Amt;

                stockSummaries.Add(stockSummary);
            }

            var overallSummary = new VMStockSummaryOverallReport
            {
                TotalPurchaseQty = totalPurchaseQty,
                TotalPurchaseAmt = totalPurchaseAmt,
                TotalPurchaseReturnQty = totalPurchaseReturnQty,
                TotalPurchaseReturnAmt = totalPurchaseReturnAmt,
                TotalSaleQty = totalSaleQty,
                TotalSaleAmt = totalSaleAmt,
                TotalSaleReturnQty = totalSaleReturnQty,
                TotalSaleReturnAmt = totalSaleReturnAmt,
                TotalTransactionCount = overallTransactionCount // Add overall transaction count
            };

            return Ok(new { StockSummaries = stockSummaries, OverallSummary = overallSummary });
        }


        [HttpGet("GetAllItemTransactions")]
        public async Task<IActionResult> GetAllItemTransactions(DateTime? startDate, DateTime? endDate)
        {
            // Fetch all products
            var allProducts = await _productsRepo.GetList().ToListAsync();
            if (allProducts == null || !allProducts.Any())
            {
                return NotFound("No products found.");
            }

            // Initialize result object
            var result = new AllItemTransactionsResult();

            foreach (var product in allProducts)
            {
                int itemId = product.pro02uin;

                // Fetch purchase transactions
                var purchaseItems = await _purchaseItemsRepo
                    .GetList()
                    .Include(x => x.pur01purchases)
                        .ThenInclude(x => x.ven01vendors)
                    .Where(p => p.pur02pro02uin == itemId
                                && (!startDate.HasValue || p.DateCreated >= startDate)
                                && (!endDate.HasValue || p.DateCreated <= endDate))
                    .ToListAsync();

                // Fetch purchase return transactions
                var purchaseReturnItems = await _purchaseReturnitemsRepo
                    .GetList()
                    .Include(x => x.pur01purchasereturns)
                        .ThenInclude(x => x.ven01vendors)
                    .Where(pr => pr.pur02returnpro02uin == itemId
                                && (!startDate.HasValue || pr.DateCreated >= startDate)
                                && (!endDate.HasValue || pr.DateCreated <= endDate))
                    .ToListAsync();

                // Fetch sale transactions
                var saleItems = await _saleItemsRepo
                    .GetList()
                    .Include(x => x.sal01sales)
                        .ThenInclude(x => x.cus01customers)
                    .Where(s => s.sal02pro02uin == itemId
                                && (!startDate.HasValue || s.DateCreated >= startDate)
                                && (!endDate.HasValue || s.DateCreated <= endDate))
                    .ToListAsync();

                // Fetch sale return transactions
                var saleReturnItems = await _salesItemReturnRepo
                    .GetList()
                    .Include(x => x.sal01salesreturn)
                        .ThenInclude(x => x.cus01customers)
                    .Where(sr => sr.sal02pro02uin == itemId
                                && (!startDate.HasValue || sr.DateCreated >= startDate)
                                && (!endDate.HasValue || sr.DateCreated <= endDate))
                    .ToListAsync();

                // Skip if no transactions exist for the product
                if (!purchaseItems.Any() && !purchaseReturnItems.Any() && !saleItems.Any() && !saleReturnItems.Any())
                {
                    continue;
                }

                int productTransactionCount = purchaseItems.Count
                                     + purchaseReturnItems.Count
                                     + saleItems.Count
                                     + saleReturnItems.Count;

                // Add to overall transaction count
                result.OverallTransactionCount += productTransactionCount;

                // Initialize the summary object for the current item
                var itemSummary = new ItemSummaryViewModel
                {
                    ItemId = itemId,
                    ItemName = product.pro02name_eng,
                    TotalPurchaseQuantity = purchaseItems.Sum(p => p.pur02qty),
                    TotalPurchaseReturnQuantity = purchaseReturnItems.Sum(pr => pr.pur02returnqty),
                    TotalSalesQuantity = (decimal)saleItems.Sum(s => s.sal02qty),
                    TotalSalesReturnQuantity = (decimal)saleReturnItems.Sum(sr => sr.sal02qty),
                    TotalTransactionCount = purchaseItems.Count
                                            + purchaseReturnItems.Count
                                            + saleItems.Count
                                            + saleReturnItems.Count,
                    TotalAmount = purchaseItems.Sum(p => p.pur02qty * p.pur02rate)
                                 + saleItems.Sum(s => (decimal)s.sal02qty * s.sal02rate)
                };

                // Add transactions to the result
                result.Transactions.AddRange(purchaseItems.Select(p => new TransactionDetailViewModel
                {
                    ItemId = itemId,
                    ItemName = product.pro02name_eng,
                    Quantity = p.pur02qty,
                    Rate = p.pur02rate,
                    TransactionType = TransactionType.Purchase.ToString(),
                    Date = p.DateCreated,
                    VendorName = FormatVendorName(p.pur01purchases.ven01vendors),
                    VendorId = p.pur01purchases.pur01ven01uin
                }));

                result.Transactions.AddRange(purchaseReturnItems.Select(pr => new TransactionDetailViewModel
                {
                    ItemId = itemId,
                    ItemName = product.pro02name_eng,
                    Quantity = pr.pur02returnqty,
                    Rate = pr.pur02returnrate,
                    TransactionType = TransactionType.PurchaseReturn.ToString(),
                    Date = pr.DateCreated,
                    VendorName = FormatVendorName(pr.pur01purchasereturns.ven01vendors),
                    VendorId = pr.pur01purchasereturns.pur01ven01uin
                }));

                result.Transactions.AddRange(saleItems.Select(s => new TransactionDetailViewModel
                {
                    ItemId = itemId,
                    ItemName = product.pro02name_eng,
                    Quantity = (decimal)s.sal02qty,
                    Rate = s.sal02rate,
                    TransactionType = TransactionType.Sale.ToString(),
                    Date = s.DateCreated,
                    CustomerName = FormatCustomerName(s.sal01sales.cus01customers),
                    CustomerId = s.sal01sales.sal01cus01uin
                }));

                result.Transactions.AddRange(saleReturnItems.Select(sr => new TransactionDetailViewModel
                {
                    ItemId = itemId,
                    ItemName = product.pro02name_eng,
                    Quantity = (decimal)sr.sal02qty,
                    Rate = sr.sal02rate,
                    TransactionType = TransactionType.SaleReturn.ToString(),
                    Date = sr.DateCreated,
                    CustomerName = FormatCustomerName(sr.sal01salesreturn.cus01customers),
                    CustomerId = sr.sal01salesreturn.sal01cus01uin
                }));

                // Add the summary to the result
                result.Summaries.Add(itemSummary);
            }

            // Return the response
            return Ok(result);
        }

        [HttpGet("GetCustomerTransactions")]
        public async Task<IActionResult> GetCustomerTransactions(int customerId, DateTime? startDate, DateTime? endDate, int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("PageNumber and PageSize must be greater than 0.");
            }

            // Fetch all products
            var allProducts = await _productsRepo.GetList().ToListAsync();
            if (allProducts == null || !allProducts.Any())
            {
                return NotFound("No products found.");
            }

            var allTransactions = new List<TransactionDetailViewModel>();

            // Iterate through all products to gather customer-specific transactions
            foreach (var product in allProducts)
            {
                int itemId = product.pro02uin;

                // Fetch sale transactions for the given customerId
                var saleItems = await _saleItemsRepo
                    .GetList()
                    .Include(x => x.sal01sales)
                        .ThenInclude(x => x.cus01customers)
                    .Where(s => s.sal02pro02uin == itemId
                                && s.sal01sales.cus01customers.cus01uin == customerId
                                && (!startDate.HasValue || s.DateCreated >= startDate)
                                && (!endDate.HasValue || s.DateCreated <= endDate))
                    .ToListAsync();

                // Fetch sale return transactions for the given customerId
                var saleReturnItems = await _salesItemReturnRepo
                    .GetList()
                    .Include(x => x.sal01salesreturn)
                        .ThenInclude(x => x.cus01customers)
                    .Where(sr => sr.sal02pro02uin == itemId
                                && sr.sal01salesreturn.cus01customers.cus01uin == customerId
                                && (!startDate.HasValue || sr.DateCreated >= startDate)
                                && (!endDate.HasValue || sr.DateCreated <= endDate))
                    .ToListAsync();

                // Combine and sort transactions by date
                var combinedTransactions = saleItems.Select(s => new TransactionDetailViewModel
                {
                    ItemId = itemId,
                    ItemName = product.pro02name_eng,
                    Quantity = (decimal)s.sal02qty,
                    TransactionType = TransactionType.Sale.ToString(),
                    Date = s.DateCreated,
                    CustomerName = FormatCustomerName(s.sal01sales.cus01customers)
                })
                .Concat(saleReturnItems.Select(sr => new TransactionDetailViewModel
                {
                    ItemId = itemId,
                    ItemName = product.pro02name_eng,
                    Quantity = (decimal)sr.sal02qty,
                    TransactionType = TransactionType.SaleReturn.ToString(),
                    Date = sr.DateCreated,
                    CustomerName = FormatCustomerName(sr.sal01salesreturn.cus01customers)
                }))
                .OrderBy(t => t.Date) // Sort by DateCreated
                .ToList();

                // Add combined transactions to the overall list
                allTransactions.AddRange(combinedTransactions);
            }

            // Apply pagination
            int totalRecords = allTransactions.Count;
            var paginatedTransactions = allTransactions
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Create the paginated result
            var result = new PaginatedResult<TransactionDetailViewModel>
            {
                Data = paginatedTransactions,
                Pagination = new PaginationInfo
                {
                    TotalRecords = totalRecords,
                    TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                }
            };

            // Return the response
            return Ok(result);
        }

        [HttpGet("GetVendorTransactions")]
        public async Task<IActionResult> GetVendorTransactions(int vendorId, DateTime? startDate, DateTime? endDate, int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("PageNumber and PageSize must be greater than 0.");
            }

            // Fetch all products
            var allProducts = await _productsRepo.GetList().ToListAsync();
            if (allProducts == null || !allProducts.Any())
            {
                return NotFound("No products found.");
            }

            var allTransactions = new List<TransactionDetailViewModel>();

            // Iterate through all products to gather vendor-specific transactions
            foreach (var product in allProducts)
            {
                int itemId = product.pro02uin;

                // Fetch purchase transactions for the given vendorId
                var purchaseItems = await _purchaseItemsRepo
                    .GetList()
                    .Include(x => x.pur01purchases)
                        .ThenInclude(x => x.ven01vendors)
                    .Where(p => p.pur02pro02uin == itemId
                                && p.pur01purchases.ven01vendors.ven01uin == vendorId
                                && (!startDate.HasValue || p.DateCreated >= startDate)
                                && (!endDate.HasValue || p.DateCreated <= endDate))
                    .ToListAsync();

                // Fetch purchase return transactions for the given vendorId
                var purchaseReturnItems = await _purchaseReturnitemsRepo
                    .GetList()
                    .Include(x => x.pur01purchasereturns)
                        .ThenInclude(x => x.ven01vendors)
                    .Where(pr => pr.pur02returnpro02uin == itemId
                                && pr.pur01purchasereturns.ven01vendors.ven01uin == vendorId
                                && (!startDate.HasValue || pr.DateCreated >= startDate)
                                && (!endDate.HasValue || pr.DateCreated <= endDate))
                    .ToListAsync();

                // Add purchase transactions to the list
                allTransactions.AddRange(purchaseItems.Select(p => new TransactionDetailViewModel
                {
                    ItemId = itemId,
                    ItemName = product.pro02name_eng,
                    Quantity = p.pur02qty,
                    TransactionType = TransactionType.Purchase.ToString(),
                    Date = p.DateCreated,
                    VendorName = FormatVendorName(p.pur01purchases.ven01vendors)
                }));

                // Add purchase return transactions to the list
                allTransactions.AddRange(purchaseReturnItems.Select(pr => new TransactionDetailViewModel
                {
                    ItemId = itemId,
                    ItemName = product.pro02name_eng,
                    Quantity = pr.pur02returnqty,
                    TransactionType = TransactionType.PurchaseReturn.ToString(),
                    Date = pr.DateCreated,
                    VendorName = FormatVendorName(pr.pur01purchasereturns.ven01vendors)
                }));
            }

            // Sort transactions by date
            allTransactions = allTransactions.OrderBy(t => t.Date).ToList();

            // Apply pagination
            int totalRecords = allTransactions.Count;
            var paginatedTransactions = allTransactions
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Create the paginated result
            var result = new PaginatedResult<TransactionDetailViewModel>
            {
                Data = paginatedTransactions,
                Pagination = new PaginationInfo
                {
                    TotalRecords = totalRecords,
                    TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                }
            };

            // Return the response
            return Ok(result);
        }



        private string FormatVendorName(ven01vendors vendor)
        {
            if (vendor == null)
                return null;

            return $"{vendor.ven01name_eng} ({vendor.ven01led_code})";
        }

        private string FormatCustomerName(cus01customers customer)
        {
            if (customer == null)
                return null;

            return $"{customer.cus01name_eng} ({customer.cus01led_code})";
        }

        public class PaginatedResult<T>
        {
            public List<T> Data { get; set; }
            public PaginationInfo Pagination { get; set; }
        }

        public class PaginationInfo
        {
            public int TotalRecords { get; set; }
            public int TotalPages { get; set; }
            public int CurrentPage { get; set; }
            public int PageSize { get; set; }
        }
    }
}
