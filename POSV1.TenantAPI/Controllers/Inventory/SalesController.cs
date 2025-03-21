
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Models.EntityModels.Inventory;
using POSV1.TenantAPI.Services;
using POSV1.TenantModel;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;
using POSV1.TenantModel.Repo.Interface.Accounting;
using POSV1.TenantModel.Repo.Interface.ERP;

namespace POSV1.TenantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController :
        _AbsCRUDWithDiffInputModelController<SalesController, ISalesRepo, sal01sales, VMSale, VMSaleList, VMSaleDetail, int>
    {
        private readonly ICustomersRepo _customersRepo;
        private readonly IProductsRepo _productsRepo;
        private readonly IUnitsRepo _unitsRepo;
        private readonly ISaleItemsRepo _saleItemsRepo;
        private readonly ILedgersRepo _ledgerRepo;
        private readonly IledgerService _ledgerService;
        private readonly IConfigurationSettings _configurationSettings;
        private readonly ITaxSettlementRepo _taxSettlementRepo;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        public SalesController(
            ILogger<SalesController> logger,
            ISalesRepo SaleRepo,
            IMapper mapper,
            ICustomersRepo customersRepo,
            IProductsRepo productsRepo,
            IUnitsRepo unitsRepo,
            ISaleItemsRepo saleItemsRepo,
            IledgerService ledgerService,
            IConfigurationSettings configurationSettings,
            ITaxSettlementRepo taxSettlementRepo,
            IHttpContextAccessor httpContextAccessor)
            : base(logger, SaleRepo, mapper)
        {
            _customersRepo = customersRepo;
            _productsRepo = productsRepo;
            _unitsRepo = unitsRepo;
            _saleItemsRepo = saleItemsRepo;
            _ledgerService = ledgerService;
            _configurationSettings = configurationSettings;
            _taxSettlementRepo = taxSettlementRepo;
            _HttpContextAccessor = httpContextAccessor;
        }

        [HttpGet("GetDetail/{InvoiceNo}")]
        public virtual async Task<VMSalesDetailWithReturnDetails> GetDetail(string InvoiceNo)
        {
            var _que = await _MainRepo.GetList()
                .Include(x => x.sal01salesreturns)
                    .ThenInclude(x => x.sal02itemsreturn)
                .Where(x => x.sal01invoice_no == InvoiceNo).FirstOrDefaultAsync();  //get the data from db

            if (_que == null)
            {
                throw new Exception("Data not found !!!");
            }
            var result = ProcessDetailDataWithReturn(_que); // proecss the data as per the view model

            return result;

        }

        [HttpGet("GetRecivablesData")]
        public virtual async Task<IActionResult> GetRecivablesData()
        {
            try
            {
                var salesData = await _MainRepo
                    .GetList()
                    .Include(x => x.sal01salesreturns)
                    .Include(x => x.cus01customers)
                    .Where(x => x.sal01payment_status == TenantModel.EnumPaymentStatus.FullCredit)
                    .ToListAsync();

                var receivablesReport = salesData
                    .GroupBy(x => x.cus01customers)
                    .Select(group => new
                    {
                        CustomerId = group.Key.cus01uin,
                        CustomerName = group.Key.cus01name_eng,
                        TotalSalesAmount = group.Sum(x => x.sal01net_amt),
                        TotalReturnAmount = group.Sum(x => x.sal01salesreturns.Sum(r => r.sal01net_amt)),
                        TotalSalesTransactions = group.Count(),
                        TotalReturnTransactions = group.Sum(x => x.sal01salesreturns.Count),
                        NetReceivable = group.Sum(x => x.sal01net_amt) - group.Sum(x => x.sal01salesreturns.Sum(r => r.sal01net_amt))
                    })
                    .OrderBy(x => x.CustomerName)
                    .ToList();

                var overallSummary = new
                {
                    TotalSalesAmount = receivablesReport.Sum(x => x.TotalSalesAmount),
                    TotalReturnAmount = receivablesReport.Sum(x => x.TotalReturnAmount),
                    TotalSalesTransactions = receivablesReport.Sum(x => x.TotalSalesTransactions),
                    TotalReturnTransactions = receivablesReport.Sum(x => x.TotalReturnTransactions),
                    NetReceivable = receivablesReport.Sum(x => x.NetReceivable)
                };

                return Ok(new
                {
                    CustomerWiseReport = receivablesReport,
                    OverallSummary = overallSummary
                });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, new
                {
                    Message = "An error occurred while fetching the receivables data.",
                    Details = ex.Message
                });
            }
        }


        [HttpGet("GetFilteredList")]
        public virtual async Task<ActionResult<PageResult<VMSaleList>>> GetFilteredList(
            int? customerId,
            string customerName,
            string invoiceNo,
            DateTime? startDate,
            DateTime? endDate,
            int? pageNumber,
            int? pageSize)
        {
            try
            {
                pageNumber ??= 1;
                pageSize ??= 10;

                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest("Invalid page number or page size.");
                }

                var query = _MainRepo.GetList()
                    .Include(x => x.cus01customers)
                    .AsNoTracking();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(customerName))
                {
                    query = query.Where(x => x.cus01customers.cus01name_eng.Contains(customerName));
                }

                if (customerId.HasValue)
                {
                    query = query.Where(x => x.sal01cus01uin == customerId);
                }

                if (!string.IsNullOrWhiteSpace(invoiceNo))
                {
                    query = query.Where(x => x.sal01invoice_no.Contains(invoiceNo));
                }

                if (startDate.HasValue && endDate.HasValue)
                {
                    query = query.Where(x => x.sal01date_eng >= startDate.Value && x.sal01date_eng <= endDate.Value);
                }

                var totalCount = await query.CountAsync();

                var processedQuery = ProcessListData(query);

                var resultList = await processedQuery
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToListAsync();

                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize.Value);

                var pageResult = new PageResult<VMSaleList>
                {
                    Data = resultList,
                    TotalPages = totalPages,
                    PageSize = pageSize.Value,
                    TotalData = totalCount,
                    CurrentPage = pageNumber.Value
                };

                return Ok(pageResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching filtered data.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching filtered data.");
            }
        }


        [HttpGet("GetDetailedFilteredList")]
        public virtual async Task<ActionResult<PageResult<VMSaleDetailList>>> GetDetaailedFilteredList(
            int? customerId,
            string customerName,
            string invoiceNo,
            string itemName,
            DateTime? startDate,
            DateTime? endDate,
            int? pageNumber,
            int? pageSize)
        {
            try
            {
                pageNumber ??= 1;
                pageSize ??= 10;

                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest("Invalid page number or page size.");
                }

                if (startDate.HasValue && endDate.HasValue && startDate > endDate)
                {
                    return BadRequest("Start date cannot be later than end date.");
                }

                var query = _saleItemsRepo.GetList()
                    .OrderByDescending(x => x.DateCreated)
                    .Include(x => x.pro02products)
                    .Include(x => x.un01units)
                    .Include(x => x.sal01sales)
                        .ThenInclude(x => x.cus01customers)
                    .AsNoTracking();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(customerName))
                {
                    query = query.Where(x => x.sal01sales.cus01customers.cus01name_eng.Contains(customerName));
                }

                if (customerId.HasValue)
                {
                    query = query.Where(x => x.sal01sales.sal01cus01uin == customerId);
                }

                if (!string.IsNullOrWhiteSpace(invoiceNo))
                {
                    query = query.Where(x => x.sal01sales.sal01invoice_no.Contains(invoiceNo));
                }

                if (!string.IsNullOrWhiteSpace(itemName))
                {
                    query = query.Where(x => x.pro02products.pro02name_eng.Contains(itemName));
                }

                if (startDate.HasValue && endDate.HasValue)
                {
                    query = query.Where(x => x.sal01sales.sal01date_eng >= startDate.Value && x.sal01sales.sal01date_eng <= endDate.Value);
                }

                var totalCount = await query.CountAsync();

                var pagedQuery = query.Skip((pageNumber.Value - 1) * pageSize.Value)
                                      .Take(pageSize.Value);

                var resultList = await MapToSaleDetailList(pagedQuery);

                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize.Value);

                var pageResult = new PageResult<VMSaleDetailList>
                {
                    Data = resultList,
                    TotalPages = totalPages,
                    PageSize = pageSize.Value,
                    TotalData = totalCount,
                    CurrentPage = pageNumber.Value
                };

                return Ok(pageResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching filtered data.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching filtered data.");
            }
        }

        private async Task<List<VMSaleDetailList>> MapToSaleDetailList(IQueryable<sal02items> data)
        {
            return await data.Select(x => new VMSaleDetailList
            {
                Id = x.sal01sales.sal01uin,
                CustomerId = x.sal01sales.sal01cus01uin,
                CustomerName = x.sal01sales.cus01customers.cus01name_eng,
                Date = x.sal01sales.sal01date_eng,
                Invoice_No = x.sal01sales.sal01invoice_no,
                ItemName = x.pro02products.pro02name_eng,
                itemId = x.pro02products.pro02uin,
                UnitName = x.un01units.un01name_eng,
                Quantity = x.sal02qty,
                Rate = x.sal02rate,
                Sub_Total = x.sal02rate * (decimal)x.sal02qty,
                Disc_Amt = x.sal02disc_amt,
                Total = (x.sal02rate * (decimal)x.sal02qty) - x.sal02disc_amt,
                VoucherNo = x.sal01sales.sal01voucher_no,
                IsVoucherLinked = !string.IsNullOrEmpty(x.sal01sales.sal01voucher_no)
            }).ToListAsync();
        }


        protected override IQueryable<VMSaleList> ProcessListData(IQueryable<sal01sales> data)
        {
            data.Include(x => x.cus01customers);

            return data.Select(sale => new VMSaleList
            {
                Id = sale.sal01uin,
                CustomerId = sale.sal01cus01uin,
                CustomerName = sale.cus01customers.cus01name_eng,
                Date = sale.sal01date_eng,
                Invoice_No = sale.sal01invoice_no,
                Remarks = sale.sal01remarks,
                Sub_Total = sale.sal01sub_total,
                Disc_Amt = sale.sal01disc_amt,
                Disc_Percentage = sale.sal01disc_percentage,
                Total = sale.sal01total,
                VAT_Per = sale.sal01vat_per,
                VAT_Amt = sale.sal01vat_amt,
                Net_Amt = sale.sal01net_amt,
                VoucherNo = sale.sal01voucher_no,
                IsVoucherLinked = !string.IsNullOrEmpty(sale.sal01voucher_no)
            });
        }

        protected VMSalesDetailWithReturnDetails ProcessDetailDataWithReturn(sal01sales data)
        {
            if (data == null)
            {
                throw new Exception("Sale not found");
            }

            var customer = _customersRepo.GetDetail(data.sal01cus01uin);

            var saleDetail = ProcessDetailData(data);

            // Use the data from the already included _que result
            var saleReturns = data.sal01salesreturns.ToList();

            var saleReturnDetails = saleReturns.Select(sr => new VMSalesReturnDetail
            {
                Id = sr.sal01uin,
                CustomerId = sr.sal01cus01uin,
                CustomerName = _customersRepo.GetDetail(sr.sal01cus01uin)?.cus01name_eng,
                Date = sr.sal01date_eng,
                Invoice_No = sr.sal01invoice_no,
                Remarks = sr.sal01remarks,
                Sub_Total = sr.sal01sub_total,
                Disc_Amt = sr.sal01disc_amt,
                Total = sr.sal01total,
                VAT_Per = sr.sal01vat_per,
                VAT_Amt = sr.sal01vat_amt,
                Net_Amt = sr.sal01net_amt,
                VMSalesReturnItemDetails = sr.sal02itemsreturn.Select(ri => new VMSalesReturnItemDetails
                {
                    ID = ri.sal02uin,
                    ProductID = ri.sal02pro02uin,
                    ProductName = ri.pro02products?.pro02name_eng,
                    Quantity = ri.sal02qty,
                    UnitID = ri.sal02un01uin,
                    UnitName = ri.un01units?.un01name_eng,
                    Rate = ri.sal02rate,
                    Sub_Total = ri.sal02sub_total,
                    Disc_Amt = ri.sal02disc_amt,
                    Net_Amt = ri.sal02net_amt
                }).ToList()
            }).ToList();

            var groupedReturnItems = saleReturnDetails
                .SelectMany(sr => sr.VMSalesReturnItemDetails)
                .GroupBy(ri => ri.ProductID)
                .Select(group => new GroupedReturnItem
                {
                    ProductID = group.Key,
                    ProductName = group.FirstOrDefault()?.ProductName,
                    TotalQuantity = (decimal)group.Sum(ri => ri.Quantity)
                }).ToList();

            return new VMSalesDetailWithReturnDetails
            {
                VMSaleDetail = saleDetail,
                VMSalesReturnDetail = saleReturnDetails,
                GroupedReturnItem = groupedReturnItems
            };
        }



        protected override VMSaleDetail ProcessDetailData(sal01sales data)
        {
            if (data == null)
            {
                throw new Exception("Sale not found");
            }
            var customer = _customersRepo.GetDetail(data.sal01cus01uin);

            var viewCustomer = new VMViewCustomerData()
            {
                ID = customer.cus01uin,
                Name = customer.cus01name_eng,
                Address = customer.cus01address,
                TelePhone_No = customer.cus01tel,
                Registration_No = customer.cus01reg_no,
                CustomerTypeId = customer.cus01customerTypeId,
                Registered_Date = customer.cus01registered_date
            };

            VMSaleDetail Result = new VMSaleDetail()
            {
                Id = data.sal01uin,
                CustomerData = viewCustomer,
                //CustomerId = data.sal01cus01uin,
                //CustomerName = customer.cus01name_eng,
                Date = data.sal01date_eng,
                Invoice_No = data.sal01invoice_no,
                Remarks = data.sal01remarks,
                Sub_Total = data.sal01sub_total,
                Disc_Amt = data.sal01disc_amt,
                Disc_Percentage = data.sal01disc_percentage,
                Total = data.sal01total,
                VAT_Per = data.sal01vat_per,
                VAT_Amt = data.sal01vat_amt,
                Net_Amt = data.sal01net_amt,
                VoucherNo = data.sal01voucher_no,
                Net_Amt_Words = EnglishNepaliNumberConverter.ConvertToWords(data.sal01net_amt),
                IsVoucherLinked = !string.IsNullOrEmpty(data.sal01voucher_no)
            };

            IQueryable<sal02items> _Que = _saleItemsRepo.GetList().Where(x => x.sal02sal01uin == data.sal01uin);

            var query = _Que.ToQueryString();
            if (_Que == null)
            {
                throw new Exception("sales item not found");
            }
            var _data = _Que.Include(x => x.pro02products)
                .Include(x => x.un01units)
                .ToList();

            Result.VMSaleItemDetails = _data.Select(x => new VMSaleItemDetails()
            {
                ID = x.sal02uin,
                HsCode = x.pro02products.pro02hscode,
                Ratio = x.un01units.un01ratio,
                ProductID = x.sal02pro02uin,
                ProductName = x.pro02products.pro02name_eng,
                Quantity = x.sal02qty,
                UnitID = x.sal02un01uin,
                UnitName = x.un01units.un01name_eng,
                Rate = x.sal02rate,
                Sub_Total = x.sal02sub_total,
                Disc_Amt = x.sal02disc_amt,
                Net_Amt = x.sal02net_amt,
                Driver_Id = x.sal02emp01uin,
                Vehicle_Id = x.sal02vec02uin,
                Destination = x.sal02destination,
                Chalan_No = x.sal02ref_no,
                transportation_Fee = x.sal02transportationfee,
                //VatAmt = (x.sal02net_amt * x.sal02vatper) / 100,
                VatPer = x.sal02vatper,
                IsVatApplied = x.sal02vatper.HasValue
            })
                .ToList();
            return Result;
        }

        //[HttpPost("CreateBulkSale")]
        //public async Task<ActionResult> BulkCreateSales(VMBulkSaleInformation Data)
        //{
        //    try
        //    {
        //        List<sal01sales> salesEntities = new List<sal01sales>();
        //        IList<int> customerIds = Data.Sales.Select(x => x.CustomerId).Distinct().ToList();

        //        foreach (var customerId in customerIds)
        //        {
        //            VMBulkSale Sale = Data.Sales.FirstOrDefault(x=>x.CustomerId == customerId);
        //            var customerDetail = await _customersRepo.GetDetailAsync(Sale.CustomerId);
        //            if (customerDetail == null)
        //            {
        //                return BadRequest($"Customer with ID {customerId} not found");
        //            }
        //            Dictionary<int, pro02products> productCache = new Dictionary<int, pro02products>();

        //            sal01sales Raw_sal01sales = new sal01sales();

        //            Raw_sal01sales.sal02items = new List<sal02items>();

        //            //List<sal02items> salesItems = new List<sal02items>();

        //            pro02products product;
        //            if (productCache.ContainsKey(Sale.ProductID))
        //            {
        //                product = productCache[Sale.ProductID];
        //            }
        //            else
        //            {
        //                product = await _productsRepo.GetDetailAsync(Sale.ProductID);
        //                productCache.Add(Sale.ProductID, product);
        //            }
        //            if (product == null)
        //            {
        //                throw new Exception("Product not found");
        //            }

        //            var unit = await _unitsRepo.GetDetailAsync(Sale.UnitID);

        //            if (unit == null)
        //            {
        //                throw new Exception("Unit not found");
        //            }

        //            Raw_sal01sales = new sal01sales
        //            {
        //                sal01cus01uin = Sale.CustomerId,
        //                sal01date_nep = "2080-06-06 ",
        //                sal01date_eng = Data.TransactionDate,
        //                sal01invoice_no = Sale.Invoice_No,
        //                sal01remarks = Data.Remarks,
        //                sal01sub_total = Sale.Sub_Total,
        //                sal01disc_amt = 0,
        //                sal01total = Sale.Total,
        //                sal01vat_per = (double)Sale.VatPercent,
        //                sal01vat_amt = Sale.VAT_Amt,
        //                sal01net_amt = Sale.Net_Amt,
        //                //sal02items = salesItems,
        //            };

        //            foreach (VMBulkSale _SaleLIst in Data.Sales.Where(x => x.CustomerId == Sale.CustomerId))
        //            {
        //                sal02items currentSaleItem = new sal02items()
        //                {
        //                    sal02pro02uin = product.pro02uin,
        //                    sal02un01uin = unit.un01uin,
        //                    sal02qty = Sale.Quantity,
        //                    sal02rate = Sale.Rate,
        //                    sal02sub_total = (double)Sale.subTotal,
        //                    sal02disc_amt = 0,
        //                    sal02net_amt = (double)Sale.netAmt,
        //                    sal02transportationfee = Sale.TransactionFee,
        //                    sal02emp01uin = Sale.DriverId,
        //                    sal02vec02uin = Sale.VechileId,
        //                    sal02destination = Sale.DestinationAddress,
        //                    sal02ref_no = "ref1"

        //                };
        //                Raw_sal01sales.sal02items.Add(currentSaleItem);
        //            }

        //            salesEntities.Add(Raw_sal01sales);
        //        }
        //        await _MainRepo.InsertBulkAsync(salesEntities);
        //        await _MainRepo.SaveAsync();
        //        return Ok("Created Successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


        [HttpPost("CreateBulkSale")]
        public async Task<ActionResult> BulkCreateSales(VMBulkSaleInformation Data)
        {
            var branchCode = _HttpContextAccessor.HttpContext.User.FindFirst("BranchCode")?.Value;

            try
            {

                if (branchCode == null)
                {
                    throw new Exception("Invalid login !!!");
                }

                List<sal01sales> salesEntities = new List<sal01sales>();

                // Extract unique customer IDs
                IList<int> customerIds = Data.Sales.Select(x => x.CustomerId).Distinct().ToList();

                foreach (var customerId in customerIds)
                {
                    // Get the first sale for the current customer
                    VMBulkSale sale = Data.Sales.FirstOrDefault(x => x.CustomerId == customerId);

                    var customerDetail = await _customersRepo.GetDetailAsync(sale.CustomerId);
                    if (customerDetail == null)
                    {
                        return BadRequest($"Customer with ID {customerId} not found");
                    }

                    // Use a dictionary to cache product details
                    Dictionary<int, pro02products> productCache = new Dictionary<int, pro02products>();

                    sal01sales newSale = new sal01sales
                    {
                        sal01cus01uin = sale.CustomerId,
                        sal01date_nep = "2080-06-06", 
                        sal01date_eng = Data.TransactionDate,
                        sal01invoice_no = sale.Invoice_No,
                        sal01remarks = Data.Remarks,
                        sal01sub_total = sale.Sub_Total,
                        sal01disc_amt = 0,
                        sal01total = sale.Total,
                        sal01vat_per = (double)sale.VatPercent,
                        sal01vat_amt = sale.VAT_Amt,
                        sal01net_amt = sale.Net_Amt,
                        sal02items = new List<sal02items>(),
                        BranchCode = branchCode
                    };

                    decimal vatPercentage = GetVatData();

                    foreach (VMBulkSale saleItem in Data.Sales.Where(x => x.CustomerId == customerId))
                    {
                        pro02products product;
                        if (productCache.ContainsKey(saleItem.ProductID))
                        {
                            product = productCache[saleItem.ProductID];
                        }
                        else
                        {
                            product = await _productsRepo.GetDetailAsync(saleItem.ProductID);
                            if (product == null)
                            {
                                return BadRequest($"Product with ID {saleItem.ProductID} not found");
                            }
                            productCache.Add(saleItem.ProductID, product);
                        }

                        var unit = await _unitsRepo.GetDetailAsync(saleItem.UnitID);
                        if (unit == null)
                        {
                            return BadRequest($"Unit with ID {saleItem.UnitID} not found");
                        }

                        sal02items currentSaleItem = new sal02items
                        {
                            sal02pro02uin = product.pro02uin,
                            sal02un01uin = unit.un01uin,
                            sal02qty = saleItem.Quantity,
                            sal02rate = saleItem.Rate,
                            sal02sub_total = (double)saleItem.subTotal,
                            sal02disc_amt = 0,
                            sal02net_amt = (double)saleItem.netAmt,
                            sal02transportationfee = saleItem.TransactionFee,
                            sal02emp01uin = saleItem.DriverId,
                            sal02vec02uin = saleItem.VechileId,
                            sal02destination = saleItem.DestinationAddress,
                            sal02ref_no = saleItem.Chalan_Number,
                            sal02vatper = saleItem.IsVatApplied ? (double?)vatPercentage : null,

                            DateCreated = DateTime.Now,
                            CreatedName = _ActiveUserName,
                        };
                        newSale.sal02items.Add(currentSaleItem);
                    }
                    salesEntities.Add(newSale);
                }
                await _MainRepo.InsertBulkAsync(salesEntities);
                await _MainRepo.SaveAsync();

                return Ok("Created Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        protected override sal01sales AssignValues(VMSale Data)
        {
            var branchCode = _HttpContextAccessor.HttpContext.User.FindFirst("BranchCode")?.Value;

            // Check if BranchCode exists
            if (string.IsNullOrEmpty(branchCode))
            {
                throw new Exception("BranchCode is missing or invalid in the token");
            }

            try
            {
                var customerDetail = _customersRepo.GetDetail(Data.CustomerId);
                if (customerDetail == null)
                {
                    throw new Exception("Customer not found");
                }

                if (Data.Disc_Percentage != 0)
                {
                    Data.Disc_Amt = (Data.Sub_Total * Data.Disc_Percentage) / 100;
                }

                decimal vatPercentage = GetVatData();

                double vatApplicableTotal = Data.VMSaleItem  
                    ?.Where(item => item.IsVatApplied)
                    .Sum(item => item.Net_Amt) ?? 0;

                if(vatApplicableTotal != 0)
                {
                    vatApplicableTotal -= (double)Data.Disc_Amt;
                }

                decimal vatAmount = ((decimal)vatApplicableTotal * vatPercentage) / 100;

                var salesEntity = new sal01sales
                {
                    sal01cus01uin = Data.CustomerId,
                    sal01date_nep = "2080-06-06 ",
                    sal01date_eng = Data.Date,
                    sal01invoice_no = Data.Invoice_No,
                    sal01remarks = Data.Remarks,
                    sal01sub_total = Data.Sub_Total,
                    sal01disc_amt = Data.Disc_Amt,
                    sal01disc_percentage = Data.Disc_Percentage,
                    sal01total = Data.Total,
                    sal01vat_per = (double)vatPercentage,
                    //sal01vat_amt = (Data.Total * vatPercentage) / 100,
                    sal01vat_amt = vatAmount,
                    //sal01net_amt = Data.Total + ((Data.Total * vatPercentage) / 100),
                    sal01net_amt = Data.Total + vatAmount,
                    BranchCode = branchCode,
                    //sal01vat_per = Data.VAT_Per,
                    //sal01vat_amt = Data.VAT_Amt,
                    //sal01net_amt = Data.Net_Amt,

                    DateCreated = DateTime.Now,
                    CreatedName = _ActiveUserName,
                    sal02items = new List<sal02items>(),
                };

                if (Data.VMSaleItem != null)
                {
                    foreach (var saleItems in Data.VMSaleItem)
                    {
                        var product = _productsRepo.GetDetail(saleItems.ProductID);

                        if (product == null)
                        {
                            throw new Exception("Product not found");
                        }

                        var unit = _unitsRepo.GetDetail(saleItems.UnitID);

                        if (unit == null)
                        {
                            throw new Exception("Unit not found");
                        }

                        var saleItemsEntity = new sal02items
                        {
                            sal02pro02uin = product.pro02uin,
                            sal02sal01uin = salesEntity.sal01uin,
                            sal02un01uin = unit.un01uin,
                            sal02qty = saleItems.Quantity,
                            sal02rate = saleItems.Rate,
                            sal02sub_total = saleItems.Sub_Total,
                            sal02disc_amt = saleItems.Disc_Amt,
                            sal02net_amt = saleItems.Net_Amt,
                            sal02emp01uin = saleItems.DriverId,
                            sal02vec02uin = saleItems.VechileId,
                            sal02destination = saleItems.Destination,
                            sal02ref_no = saleItems.Chalan_No,
                            sal02transportationfee = saleItems.Transportation_Fee,
                            sal02vatper = saleItems.IsVatApplied ? (double?)vatPercentage : null,

                            DateCreated = DateTime.Now,
                            CreatedName = _ActiveUserName,
                        };

                        salesEntity.sal02items.Add(saleItemsEntity);
                    }
                }

                if (!_ledgerService.UpdateLedgerOnSale(salesEntity).GetAwaiter().GetResult())
                {
                    throw new Exception("ledger not updated");
                }

                if (!_ledgerService.CreateLedgerOnSaleDisc(salesEntity).GetAwaiter().GetResult())
                {
                    throw new Exception("Discount ledger not Created");
                }

                return salesEntity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private decimal GetVatData()
        {
            var vatData = _configurationSettings.GetList().Where(x => x.Name == EnumConfigSettings.ApplySalesVat.ToString()).FirstOrDefault();
            if (vatData == null)
            {
                throw new Exception("Apply seed data !!!");
            }

            decimal vatPercentage = 0;
            if (vatData.Value == "true")
            {
                vatPercentage = _taxSettlementRepo.GetList().Where(x => x.Title == "VAT").Select(x => x.taxPercentage).FirstOrDefault();
            }

            return vatPercentage;
        }

        protected override async Task AfterUpdate(sal01sales data)
        {
            var response = await _ledgerService.UpdateLedgerOnSale(data);
            if (!response)
            {
                throw new Exception("ledger not updated");
            }
        }

        protected override void ReAssignValues(VMSale Data, sal01sales oldData)
        {
            if (Data.Disc_Percentage != 0)
            {
                Data.Disc_Amt = (Data.Sub_Total * Data.Disc_Percentage) / 100;
            }

            decimal vatPercentage = (decimal)oldData.sal01vat_per;

            oldData.sal01date_nep = "";
            oldData.sal01date_eng = Data.Date;
            oldData.sal01invoice_no = Data.Invoice_No;
            oldData.sal01remarks = Data.Remarks;
            oldData.sal01sub_total = Data.Sub_Total;
            oldData.sal01disc_amt = Data.Disc_Amt;
            oldData.sal01disc_percentage = Data.Disc_Percentage;
            oldData.sal01total = Data.Total;
            oldData.sal01vat_per = (double)vatPercentage;
            oldData.sal01vat_amt = (Data.Total * vatPercentage) / 100;
            oldData.sal01net_amt = Data.Total + ((Data.Total * vatPercentage) / 100);
            //oldData.sal01vat_per = oldData.VAT_Per;
            //oldData.sal01vat_amt = Data.VAT_Amt;
            //oldData.sal01net_amt = Data.Net_Amt;

            oldData.DateUpdated = DateTime.Now;
            oldData.UpdatedName = _ActiveUserName;

            _saleItemsRepo.LoadSaleItemDetails(oldData);
            if (Data.VMSaleItem != null)
            {
                SoftDeleteNonExisingChild(Data, oldData);
                AddUpdateNewChildRecords(Data, oldData);

            }
        }

        private void SoftDeleteNonExisingChild(VMSale Data, sal01sales oldData)
        {
            int[] ExistingIDs = Data
               .VMSaleItem.Where(x => x.ID > 0)
               .Select(x => x.ID)
               .ToArray();

            // Remove any details that are no longer present in the updated data
            var detailsToRemove = oldData.sal02items
                .Where(d => !ExistingIDs.Contains(d.sal02uin))
            .ToList();

            foreach (var detailToRemove in detailsToRemove)
            {
                detailToRemove.DateDeleted = DateTime.UtcNow;
                _saleItemsRepo.Save();
            }
        }

        private void AddUpdateNewChildRecords(VMSale Data, sal01sales oldData)
        {
            List<sal02items> oldDataItems = oldData.sal02items.ToList().GetRange(0, oldData.sal02items.Count);

            foreach (var updatedSaleItemDetail in Data.VMSaleItem)
            {
                var product = _productsRepo.GetDetail(updatedSaleItemDetail.ProductID);

                if (product == null)
                {
                    throw new Exception("product ID not found");
                }

                var unit = _unitsRepo.GetDetail(updatedSaleItemDetail.UnitID);

                if (unit == null)
                {
                    throw new Exception("Unit not found");
                }

                var existingDetail = oldDataItems
                    .FirstOrDefault(d => d.sal02uin == updatedSaleItemDetail.ID);

                bool CreateMode = false;

                if (existingDetail == null)
                {
                    CreateMode = true;
                    existingDetail = new sal02items()
                    {
                        sal02uin = 0,
                    };
                }

                existingDetail.sal02pro02uin = product.pro02uin;
                existingDetail.sal02un01uin = unit.un01uin;
                existingDetail.sal02qty = updatedSaleItemDetail.Quantity;
                existingDetail.sal02rate = updatedSaleItemDetail.Rate;
                existingDetail.sal02sub_total = updatedSaleItemDetail.Sub_Total;
                existingDetail.sal02disc_amt = updatedSaleItemDetail.Disc_Amt;
                existingDetail.sal02net_amt = updatedSaleItemDetail.Net_Amt;
                existingDetail.sal02emp01uin = updatedSaleItemDetail.DriverId;
                existingDetail.sal02vec02uin = updatedSaleItemDetail.VechileId;
                existingDetail.sal02destination = updatedSaleItemDetail.Destination;
                existingDetail.sal02ref_no = updatedSaleItemDetail.Chalan_No;
                existingDetail.sal02transportationfee = updatedSaleItemDetail.Transportation_Fee;

                existingDetail.DateUpdated = DateTime.Now;
                existingDetail.UpdatedName = _ActiveUserName;
                if (CreateMode) { oldData.sal02items.Add(existingDetail); }
            }
        }
    }
}
