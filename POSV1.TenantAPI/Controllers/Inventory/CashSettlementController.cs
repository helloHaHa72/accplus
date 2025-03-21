using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Models.EntityModels.Inventory;
using POSV1.TenantModel;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Implementation;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantAPI.Controllers.Inventory
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashSettlementController : ControllerBase
    {
        private readonly ILogger<CashSettlementController> _logger;
        private readonly ICashSettlementRepo _cashSettlementRepo;
        private readonly IPurchaseRepo _purchaseRepo;
        private readonly ISalesRepo _salesRepo;
        public CashSettlementController(
            ILogger<CashSettlementController> logger,
            ICashSettlementRepo cashSettlementRepo,
            IPurchaseRepo purchaseRepo,
            ISalesRepo salesRepo)
        {
            _logger = logger;
            _cashSettlementRepo = cashSettlementRepo;
            _purchaseRepo = purchaseRepo;
            _salesRepo = salesRepo;
        }

        [HttpGet("GetPurchaseInvoiceNumber")]
        public async Task<IActionResult> GetPurchaseInvoiceNumber(int vendorId)
        {
            try
            {
                var data = await _purchaseRepo.GetList()
                    .Where(x => x.pur01ven01uin == vendorId)  // Filter by vendorId first
                    .Where(x => x.pur01tdspercentage == 0 || x.pur01tdspercentage == null)  // Separate filter for TDS percentage
                    .OrderByDescending(x => x.DateCreated)
                    .ToListAsync();


                var finalData = data.Select(c => new InvoiceList
                {
                    PurchaseId = c.pur01uin,
                    InvoiceDate = c.pur01date.Date,
                    InvoiceNumber = c.pur01invoice_no,
                    Sub_Total = c.pur01sub_total,
                    DiscountAmount = c.pur01disc_amt,
                    DiscountPercentage = c.pur01disc_percentage,
                    VatAmount = c.pur01vat_amt,
                    VatPercentage = c.pur01vat_per,
                    NetAmount = c.pur01net_amt
                }).ToList();

                return Ok(finalData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching data.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching data.");
            }
        }

        [HttpGet("UserWise")]
        public async Task<ActionResult<PageResult<VMCashSettlementCustomerWise>>> GetList(
            int? customerId,
            int? vendorId,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest("Invalid page number or page size.");
                }

                var query = _cashSettlementRepo.GetList()
                    .Include(x => x.cus01customers)
                    .Include(x => x.ven01vendors)
                    .OrderByDescending(x => x.DateCreated)
                    .AsQueryable();

                if (customerId.HasValue && vendorId.HasValue)
                {
                    query = query.Where(c =>
                        (c.cus01customers != null && c.cus01customers.cus01uin == customerId) ||
                        (c.ven01vendors != null && c.ven01vendors.ven01uin == vendorId)
                    );
                }
                else if (customerId.HasValue)
                {
                    query = query.Where(c =>
                        c.cus01customers != null && c.cus01customers.cus01uin == customerId
                    );
                }
                else if (vendorId.HasValue)
                {
                    query = query.Where(c =>
                        c.ven01vendors != null && c.ven01vendors.ven01uin == vendorId
                    );
                }

                var totalRecords = await query.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                var data = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                List<VMCashSettlementCustomerWise> finalData = MapToList(data);

                var pageResult = new PageResult<VMCashSettlementCustomerWise>
                {
                    Data = finalData,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    TotalData = totalRecords,
                    CurrentPage = pageNumber
                };

                return Ok(pageResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching filtered data.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching filtered data.");
            }
        }


        [HttpGet("GetList")]
        public async Task<ActionResult<PageResult<VMCashSettlementCustomerWise>>> GetList(
            EnumPaymentType? paymentType,
            string customerName,
            string vendorName,
            DateTime? startDate,
            DateTime? endDate,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest("Invalid page number or page size.");
                }

                var query = _cashSettlementRepo.GetList()
                    .Include(x => x.cus01customers)
                    .Include(x => x.ven01vendors)
                    .OrderByDescending(x => x.DateCreated)
                    .AsQueryable();

                if (paymentType != null)
                {
                    query = query.Where(c => c.cas01payment_type == paymentType);
                }

                if (!string.IsNullOrEmpty(customerName) && !string.IsNullOrEmpty(vendorName))
                {
                    query = query.Where(c =>
                        (c.cus01customers != null && c.cus01customers.cus01name_eng.Contains(customerName)) ||
                        (c.ven01vendors != null && c.ven01vendors.ven01name_eng.Contains(vendorName))
                    );
                }
                else if (!string.IsNullOrEmpty(customerName))
                {
                    query = query.Where(c =>
                        c.cus01customers != null && c.cus01customers.cus01name_eng.Contains(customerName)
                    );
                }
                else if (!string.IsNullOrEmpty(vendorName))
                {
                    query = query.Where(c =>
                        c.ven01vendors != null && c.ven01vendors.ven01name_eng.Contains(vendorName)
                    );
                }

                if (startDate.HasValue)
                {
                    query = query.Where(c => c.cas01transaction_date >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(c => c.cas01transaction_date <= endDate.Value);
                }

                var totalRecords = await query.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                var data = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                List<VMCashSettlementCustomerWise> finalData = MapToList(data);

                var pageResult = new PageResult<VMCashSettlementCustomerWise>
                {
                    Data = finalData,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    TotalData = totalRecords,
                    CurrentPage = pageNumber
                };

                return Ok(pageResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching filtered data.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching filtered data.");
            }
        }

        private static List<VMCashSettlementCustomerWise> MapToList(List<cas01cashsettlement> data)
        {
            return data.Select(c => new VMCashSettlementCustomerWise
            {
                Id = c.cas01uin,
                PaymentType = c.cas01payment_type.ToString(),
                CustomerId = c.cas01customeruin,
                CustomerName = c.cus01customers?.cus01name_eng,
                VendorName = c.ven01vendors?.ven01name_eng,
                VendorId = c.cas01vendoruin,
                Remarks = c.cas01remarks,
                TransactionDate = c.cas01transaction_date,
                Amount = c.cas01amount,
                BankName = c.cas01bank_ledname,
                IsBank = c.cas01isbank,
                ChqNumber = c.cas01chqnumber,
                VoucherNo = c.cas0101voucher_no,
                IsVoucherLinked = !string.IsNullOrEmpty(c.cas0101voucher_no)
            }).ToList();
        }

        [HttpGet("GetDetail/{id}")]
        public async Task<ActionResult<VMCashSettlementCustomerWise>> GetDetail(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid ID.");
                }

                var data = await _cashSettlementRepo.GetList()
                    .Include(x => x.cus01customers)
                    .Include(x => x.ven01vendors)
                    .FirstOrDefaultAsync(c => c.cas01uin == id);

                if (data == null)
                {
                    return NotFound("Record not found.");
                }

                var result = new VMCashSettlementCustomerWise
                {
                    Id = data.cas01uin,
                    PaymentType = data.cas01payment_type.ToString(),
                    CustomerId = data.cas01customeruin,
                    CustomerName = data.cus01customers?.cus01name_eng,
                    VendorName = data.ven01vendors?.ven01name_eng,
                    VendorId = data.cas01vendoruin,
                    Remarks = data.cas01remarks,
                    TransactionDate = data.cas01transaction_date,
                    Amount = data.cas01amount,
                    BankName = data.cas01bank_ledname,
                    IsBank = data.cas01isbank,
                    IsBillPayment = string.IsNullOrEmpty(data.cas01invoice_no),
                    Tds = data.cas01tdspercentage,
                    PurchaseId = data.cas01purchaseuin,
                    ChqNumber = data.cas01chqnumber,
                    VoucherNo = data.cas0101voucher_no,
                    IsVoucherLinked = !string.IsNullOrEmpty(data.cas0101voucher_no)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching record details.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching record details.");
            }
        }


        public class PageResult<T>
        {
            public List<T> Data { get; set; } = new List<T>();
            public int TotalPages { get; set; }
            public int PageSize { get; set; }
            public int TotalData { get; set; }
            public int CurrentPage { get; set; }
        }

        [HttpPost, Route("Create")]
        public async Task<IActionResult> Add([FromBody] CreateCashSettlementCustomerWise data)
        {
            try
            {
                if (data.CustomerId == 0 && data.VendorId == 0)
                {
                    throw new Exception("Invalid userid provided !!!");
                }

                if (data.IsBank)
                {
                    if (data.BankName == null || data.ChqNumber == null)
                    {
                        throw new Exception("Bank name and cheque number cannot be null !!!");
                    }
                }

                if (data.IsBillPayment)
                {
                    if (data.Tds == null || data.PurchaseId.FirstOrDefault() == 0)
                    {
                        throw new Exception("TDs cannot be null !!!");
                    }
                }

                var insertData = new cas01cashsettlement()
                {
                    cas01customeruin = data.CustomerId == 0 ? (int?)null : data.CustomerId,
                    cas01vendoruin = data.VendorId == 0 ? (int?)null : data.VendorId,
                    cas01amount = data.Amount,
                    cas01remarks = data.Remarks,
                    cas01payment_type = data.CustomerId != 0 ? EnumPaymentType.Received : EnumPaymentType.Paid,
                    cas01transaction_date = data.Date,
                    cas01bank_ledname = data.BankName,
                    cas01chqnumber = data.ChqNumber,
                    cas01isbank = data.IsBank,
                    cas01purchaseuin = data.PurchaseId.FirstOrDefault() == 0 || data.PurchaseId == null ? (int?)null : data.PurchaseId.FirstOrDefault(),
                    //cas01invoice_no = data.InvoiceNumber,
                    cas01tdspercentage = data.Tds,
                    CreatedName = "Admin",
                    DateCreated = DateTime.UtcNow,
                };

                _cashSettlementRepo.Insert(insertData);
                await _cashSettlementRepo.SaveAsync();

                await UpdateInvoiceData(data);

                return Ok("Payment data inserted !!!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to add data. {ex.Message}");
            }
        }

        private async Task UpdateInvoiceData(CreateCashSettlementCustomerWise data)
        {
            int purchaseId = data.PurchaseId.FirstOrDefault();
            if (purchaseId != 0)
            {
                if (data.PurchaseId.FirstOrDefault() != 0)
                {
                    var puchaseData = await _purchaseRepo.GetDetailAsync(purchaseId);

                    puchaseData.pur01tdspercentage = data.Tds;

                    _purchaseRepo.Update(puchaseData);
                    await _purchaseRepo.SaveAsync();
                }
            }
        }

        [HttpPut, Route("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateCashSettlementCustomerWise data)
        {
            try
            {
                var existingData = await _cashSettlementRepo.GetDetailAsync(data.Id);
                if (existingData == null)
                {
                    return NotFound("Record not found !!!");
                }

                if (data.CustomerId == 0 && data.VendorId == 0)
                {
                    throw new Exception("Invalid userid provided !!!");
                }

                existingData.cas01customeruin = data.CustomerId == 0 ? (int?)null : data.CustomerId;
                existingData.cas01vendoruin = data.VendorId == 0 ? (int?)null : data.VendorId;
                existingData.cas01amount = data.Amount;
                existingData.cas01remarks = data.Remarks;
                existingData.cas01payment_type = data.CustomerId != 0 ? EnumPaymentType.Received : EnumPaymentType.Paid;
                existingData.cas01transaction_date = data.Date;
                existingData.UpdatedName = "Mod";
                existingData.DateUpdated = DateTime.UtcNow;

                _cashSettlementRepo.Update(existingData);
                await _cashSettlementRepo.SaveAsync();

                return Ok("Payment data updated successfully !!!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update data. {ex.Message}");
            }
        }

        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> Add([FromBody] CreateCashSettlement data)
        {
            try
            {
                decimal amount = 0;
                EnumPaymentStatus status = EnumPaymentStatus.FullCredit;
                if (data.PurchaseId != 0)
                {
                    var purchaseData = await _purchaseRepo.GetDetailAsync(data.PurchaseId);
                    amount = purchaseData.pur01total;
                }
                else if (data.SaleId != 0)
                {
                    var saleData = await _salesRepo.GetDetailAsync(data.SaleId);
                    amount = saleData.sal01total;
                }
                else
                {
                    throw new Exception("Invalid id prodided !!!");
                }

                status = FetchProcessStatus(data, amount, status);

                var insertData = new cas01cashsettlement()
                {
                    cas01purchaseuin = data.PurchaseId,
                    cas01saleuin = data.SaleId,
                    cas01invoice_no = data.InvoiceNumber,
                    cas01payment_status = status,
                    cas01amount = amount,
                    CreatedName = "Admin",
                    DateCreated = DateTime.UtcNow,
                };

                _cashSettlementRepo.Insert(insertData);
                await _cashSettlementRepo.SaveAsync();

                return Ok("Payment data inserted !!!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to add data. {ex.Message}");
            }
        }

        private static EnumPaymentStatus FetchProcessStatus(CreateCashSettlement data, decimal amount, EnumPaymentStatus status)
        {
            if (amount == data.Amount)
            {
                status = EnumPaymentStatus.Paid;
            }
            else if (data.Amount != 0 && data.Amount < amount)
            {
                status = EnumPaymentStatus.Credit;
            }
            else if (data.Amount > amount)
            {
                throw new Exception("amount cannot be greter than transaction amount");
            }

            return status;
        }
    }
}
