using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantAPI.Models.EntityModels.ERP;
using POSV1.TenantModel;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantAPI.Controllers.Inventory
{
    [Route("api/[controller]")]
    [ApiController]
    public class VatReportController : ControllerBase
    {
        private readonly ILogger<VatReportController> _logger;
        private readonly IPurchaseRepo _purchaseRepo;
        private readonly ISalesRepo _salesRepo;
        public VatReportController(
            ILogger<VatReportController> logger,
            IPurchaseRepo purchaseRepo,
            ISalesRepo salesRepo)
        {
            _logger = logger;
            _purchaseRepo = purchaseRepo;
            _salesRepo = salesRepo;
        }

        [HttpGet("VatReport")]
        public async Task<IActionResult> GetVatReport(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                if(fromDate == null)
                {
                    fromDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
                }

                if (toDate == null)
                {
                    toDate = DateTime.UtcNow;
                }
                // Fetch purchases within the date range
                var purchaseData = await _purchaseRepo.GetList()
                    .Include(x => x.ven01vendors)
                    .Where(p => p.pur01date >= fromDate && p.pur01date <= toDate) // Date filter
                    .Select(p => new PurchaseDto
                    {
                        Id = p.pur01uin,
                        Date = p.pur01date,
                        InvoiceNo = p.pur01invoice_no,
                        VendorName = p.ven01vendors.ven01name_eng,
                        VendorId = p.ven01vendors.ven01uin,
                        VatPan = p.ven01vendors.ven01reg_no,
                        TotalAmount = p.pur01total,
                        VatApplicable = p.pur01vatapplicable,
                        VatAmount = p.pur01vatapplicable ? p.pur01total * 0.13m : 0,
                        TotalAmountWithVat = p.pur01total + (p.pur01vatapplicable ? p.pur01total * 0.13m : 0),
                        VatClaimable = p.pur01vatclamable
                    })
                    .ToListAsync();

                // Fetch sales within the date range
                var salesData = await _salesRepo.GetList()
                    .Include(x => x.cus01customers)
                    .Where(s => s.sal01date_eng >= fromDate && s.sal01date_eng <= toDate) // Date filter
                    .Select(s => new SalesDto
                    {
                        Id = s.sal01uin,
                        Date = s.sal01date_eng,
                        InvoiceNo = s.sal01invoice_no,
                        CustomerName = s.cus01customers.cus01name_eng,
                        CustomerId = s.cus01customers.cus01uin,
                        VatPan = s.cus01customers.cus01reg_no,
                        TotalAmount = s.sal01total,
                        VatApplicable = s.sal01vatapplicable,
                        VatAmount = s.sal01vatapplicable ? s.sal01total * 0.13m : 0,
                        TotalAmountWithVat = s.sal01total + (s.sal01vatapplicable ? s.sal01total * 0.13m : 0),
                        VatClaimable = s.sal01vatclamable
                    })
                    .ToListAsync();

                // Extract non-claimable purchases
                var nonClaimablePurchases = purchaseData
                    .Where(p => !p.VatClaimable)
                    .Select(p => new NonClaimableDto
                    {
                        Id = p.Id,
                        Date = p.Date,
                        InvoiceNo = p.InvoiceNo,
                        TotalAmount = p.TotalAmount,
                        VatAmount = p.VatAmount,
                        TotalAmountWithVat = p.TotalAmountWithVat
                    })
                    .ToList();

                // Extract non-claimable sales
                var nonClaimableSales = salesData
                    .Where(s => !s.VatClaimable)
                    .Select(s => new NonClaimableDto
                    {
                        Id = s.Id,
                        Date = s.Date,
                        InvoiceNo = s.InvoiceNo,
                        TotalAmount = s.TotalAmount,
                        VatAmount = s.VatAmount,
                        TotalAmountWithVat = s.TotalAmountWithVat
                    })
                    .ToList();

                // Prepare VAT summary
                var vatReport = new VatReportDto
                {
                    PurchaseDetails = purchaseData,
                    SalesDetails = salesData,
                    NonClaimableVatDetails = new NonClaimableVatDto
                    {
                        NonClaimablePurchases = nonClaimablePurchases,
                        NonClaimableSales = nonClaimableSales
                    },
                    Summary = new VatSummaryDto
                    {
                        TotalSalesVat = salesData.Sum(s => s.VatAmount),
                        TotalPurchaseVat = purchaseData.Sum(p => p.VatAmount),
                        TotalNonClaimableVat = nonClaimablePurchases.Sum(p => p.VatAmount) + nonClaimableSales.Sum(s => s.VatAmount),
                        NetPayableVat = salesData.Sum(s => s.VatAmount) -
                                        (purchaseData.Sum(p => p.VatAmount) - nonClaimablePurchases.Sum(p => p.VatAmount))
                    }
                };

                return Ok(vatReport);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpGet("ItemWiseVatReport")]
        public async Task<IActionResult> GetItemWiseVatReport(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                if (fromDate == null)
                {
                    fromDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
                }

                if (toDate == null)
                {
                    toDate = DateTime.UtcNow;
                }

                var purchaseItems = await _purchaseRepo.GetList()
                    .Where(p => p.pur01date >= fromDate && p.pur01date <= toDate)
                    .Include(p => p.pur02items)
                        .ThenInclude(x => x.pro02products)
                    .SelectMany(p => p.pur02items.Select(i => new
                    {
                        ItemName = i.pro02products.pro02name_eng,
                        ItemId = i.pro02products.pro02uin,
                        UnitPrice = i.pur02rate,
                        Amount = i.pur02qty * i.pur02rate,
                        VatAmount = p.pur01vatapplicable ? (i.pur02qty * i.pur02rate) * 0.13m : 0
                    }))
                    .ToListAsync();

                var salesItems = await _salesRepo.GetList()
                    .Where(s => s.sal01date_eng >= fromDate && s.sal01date_eng <= toDate)
                    .Include(s => s.sal02items)
                        .ThenInclude(x => x.pro02products)
                    .SelectMany(s => s.sal02items.Select(i => new
                    {
                        ItemName = i.pro02products.pro02name_eng,
                        ItemId = i.pro02products.pro02uin,
                        UnitPrice = i.sal02rate,
                        Amount = (decimal)i.sal02qty * (decimal)i.sal02rate,
                        VatAmount = s.sal01vatapplicable ? (decimal)(i.sal02qty * (double)i.sal02rate) * 0.13m : 0
                    }))
                    .ToListAsync();

                var vatReport = purchaseItems.Concat(salesItems)
                    .GroupBy(i => new { i.ItemId, i.ItemName })
                    .Select(g => new ItemVatSummaryDto
                    {
                        ItemName = g.Key.ItemName,
                        ItemId = g.Key.ItemId,
                        UnitPrice = g.First().UnitPrice,
                        TransactionCount = g.Count(),
                        PayableAmount = purchaseItems.Where(x => x.ItemId == g.Key.ItemId).Sum(x => x.Amount),
                        ReceivableAmount = salesItems.Where(x => x.ItemId == g.Key.ItemId).Sum(x => x.Amount),
                        PayableVat = salesItems.Where(x => x.ItemId == g.Key.ItemId).Sum(x => x.VatAmount),
                        ReceivableVat = purchaseItems.Where(x => x.ItemId == g.Key.ItemId).Sum(x => x.VatAmount)
                    })
                    .OrderBy(x => x.ItemName)
                    .ToList();

                return Ok(vatReport);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
