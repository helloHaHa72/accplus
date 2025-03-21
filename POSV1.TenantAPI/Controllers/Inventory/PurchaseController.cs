using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Models.EntityModels.Inventory;
using POSV1.TenantAPI.Models.EntityModels.Production;
using POSV1.TenantAPI.Services;
using POSV1.TenantModel;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Models.EntityModels.Production;
using POSV1.TenantModel.Repo.Implementation.Production;
using POSV1.TenantModel.Repo.Interface;
using POSV1.TenantModel.Repo.Interface.ERP;
using POSV1.TenantModel.Repo.Interface.Production;
using System.Linq;

namespace POSV1.TenantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController :
        _AbsCRUDWithDiffInputModelController<
            PurchaseController,
            IPurchaseRepo,
            pur01purchases,
            VMPurchase,
            VMPurchaseList,
            VMPurchaseDetail,
            int>
    {
        private IVendorRepo _vendorRepo;
        private IProductsRepo _productsRepo;
        private IUnitsRepo _unitsRepo;
        private IPurchaseItemsRepo _purchaseItemsRepo;
        private readonly IledgerService _ledgerService;
        private readonly IGlobalService _globalService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPurchaseAdditionalCharges _purchaseAdditionalCharges;
        private readonly IAdditionalChargesPurchaseRelation _additionalChargesPurchaseRelation;
        private readonly IConfigurationSettings _configurationSettings;
        private readonly ITaxSettlementRepo _taxSettlementRepo;

        private readonly IPurchaseRepo _purchaseRepo;
        public PurchaseController(
            ILogger<PurchaseController> logger,
            IPurchaseRepo PurchaseRepo,
            IMapper mapper,
            IVendorRepo vendorRepo,
            IProductsRepo productsRepo,
            IUnitsRepo unitsRepo,
            IPurchaseItemsRepo purchaseItemsRepo,
            IledgerService ledgerService,
            IGlobalService globalService,
            IHttpContextAccessor httpContextAccessor,
            IPurchaseAdditionalCharges purchaseAdditionalCharges,
            IAdditionalChargesPurchaseRelation additionalChargesPurchaseRelation,
            IPurchaseRepo purchaseRepo,
            IConfigurationSettings configurationSettings,
            ITaxSettlementRepo taxSettlementRepo)
            : base(logger, PurchaseRepo, mapper)
        {
            _vendorRepo = vendorRepo;
            _productsRepo = productsRepo;
            _unitsRepo = unitsRepo;
            _purchaseItemsRepo = purchaseItemsRepo;
            _ledgerService = ledgerService;
            _globalService = globalService;
            _httpContextAccessor = httpContextAccessor;
            _purchaseAdditionalCharges = purchaseAdditionalCharges;
            _additionalChargesPurchaseRelation = additionalChargesPurchaseRelation;
            _purchaseRepo = purchaseRepo;
            _configurationSettings = configurationSettings;
            _taxSettlementRepo = taxSettlementRepo;
        }

        [HttpGet("GetDetail/{InvoiceNo}")]
        public virtual async Task<VMPurchaseDetailWithReturnDetails> GetDetail(string InvoiceNo)
        {
            var _que = await _MainRepo.GetList()
                .Include(x => x.pur01purchasereturns)
                    .ThenInclude(x => x.pur02returnitems)
                .Where(x => x.pur01invoice_no == InvoiceNo).FirstOrDefaultAsync();  //get the data from db

            if(_que == null)
            {
                throw new Exception("Data not found !!!");
            }
            var result = ProcessDetailWithReturnData(_que); // proecss the data as per the view model

            return result;
        }

        [HttpGet("GetPayablesData")]
        public virtual async Task<IActionResult> GetPayablesData()
        {
            try
            {
                var purchaseData = await _MainRepo
                    .GetList()
                    .Include(x => x.pur01purchasereturns)
                    .Include(x => x.ven01vendors)
                    .Where(x => x.pur01payment_status == TenantModel.EnumPaymentStatus.FullCredit)
                    .ToListAsync();

                var payableReport = purchaseData
                    .GroupBy(x => x.ven01vendors)
                    .Select(group => new
                    {
                        VendorId = group.Key.ven01uin,
                        VendorName = group.Key.ven01name_eng,
                        TotalPurchaseAmount = group.Sum(x => x.pur01net_amt),
                        TotalReturnAmount = group.Sum(x => x.pur01purchasereturns.Sum(r => r.pur01return_net_amt)),
                        TotalPurchaseTransactions = group.Count(),
                        TotalReturnTransactions = group.Sum(x => x.pur01purchasereturns.Count),
                        NetPayable = group.Sum(x => x.pur01net_amt) - group.Sum(x => x.pur01purchasereturns.Sum(r => r.pur01return_net_amt))
                    })
                    .OrderBy(x => x.VendorName)
                    .ToList();

                var overallSummary = new
                {
                    TotalPurchaseAmount = payableReport.Sum(x => x.TotalPurchaseAmount),
                    TotalReturnAmount = payableReport.Sum(x => x.TotalReturnAmount),
                    TotalPurchaseTransactions = payableReport.Sum(x => x.TotalPurchaseTransactions),
                    TotalReturnTransactions = payableReport.Sum(x => x.TotalReturnTransactions),
                    NetPayable = payableReport.Sum(x => x.NetPayable)
                };

                return Ok(new
                {
                    VendorWiseReport = payableReport,
                    OverallSummary = overallSummary
                });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, new
                {
                    Message = "An error occurred while fetching the payable data.",
                    Details = ex.Message
                });
            }
        }



        [HttpGet("GetFilteredList")]
        public virtual async Task<ActionResult<PageResult<VMPurchaseList>>> GetFilteredList(
            int? vendorId,
            string vendorName,
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
                    .Include(x => x.ven01vendors)
                    .AsNoTracking();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(vendorName))
                {
                    query = query.Where(x => x.ven01vendors.ven01name_eng.Contains(vendorName));
                }

                if (vendorId.HasValue)
                {
                    query = query.Where(x => x.pur01ven01uin == vendorId);
                }

                if (!string.IsNullOrWhiteSpace(invoiceNo))
                {
                    query = query.Where(x => x.pur01invoice_no.Contains(invoiceNo));
                }

                if (startDate.HasValue && endDate.HasValue)
                {
                    query = query.Where(x => x.pur01date >= startDate.Value && x.pur01date <= endDate.Value);
                }

                var totalCount = await query.CountAsync();

                var processedQuery = ProcessListData(query);

                var resultList = await processedQuery
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToListAsync();

                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize.Value);

                var pageResult = new PageResult<VMPurchaseList>
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
        public virtual async Task<ActionResult<PageResult<VMPurchaseDetailList>>> GetDetailedFilteredList(
            int? vendorId,
            string vendorName,
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

                var query = _purchaseItemsRepo.GetList()
                    .OrderByDescending(x => x.DateCreated)
                    .Include(x => x.pro02products)
                    .Include(x => x.un01units)
                    .Include(x => x.pur01purchases)
                        .ThenInclude(p => p.ven01vendors)
                    .AsNoTracking();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(vendorName))
                {
                    query = query.Where(x => x.pur01purchases.ven01vendors.ven01name_eng.Contains(vendorName));
                }

                if (vendorId.HasValue)
                {
                    query = query.Where(x => x.pur01purchases.pur01ven01uin == vendorId);
                }

                if (!string.IsNullOrWhiteSpace(invoiceNo))
                {
                    query = query.Where(x => x.pur01purchases.pur01invoice_no.Contains(invoiceNo));
                }

                if (!string.IsNullOrWhiteSpace(itemName))
                {
                    query = query.Where(x => x.pro02products.pro02name_eng.Contains(itemName));
                }

                if (startDate.HasValue && endDate.HasValue)
                {
                    query = query.Where(x => x.pur01purchases.pur01date >= startDate.Value && x.pur01purchases.pur01date <= endDate.Value);
                }

                var totalCount = await query.CountAsync();

                query = query.Skip((pageNumber.Value - 1) * pageSize.Value)
                     .Take(pageSize.Value);

                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize.Value);

                var resultList = await MapToPurchaseDetailList(query);

                var pageResult = new PageResult<VMPurchaseDetailList>
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

        private async Task<List<VMPurchaseDetailList>> MapToPurchaseDetailList(IQueryable<pur02items> query)
        {
            return await query.Select(x => new VMPurchaseDetailList
            {
                Id = x.pur01purchases.pur01uin,
                VendorId = x.pur01purchases.pur01ven01uin,
                VendorName = x.pur01purchases.ven01vendors.ven01name_eng,
                Date = x.pur01purchases.pur01date,
                Invoice_No = x.pur01purchases.pur01invoice_no,
                ItemName = x.pro02products.pro02name_eng,
                itemId = x.pro02products.pro02uin,
                UnitName = x.un01units.un01name_eng,
                Quantity = x.pur02qty,
                Rate = x.pur02rate,
                Sub_Total = x.pur02rate * x.pur02qty,
                Disc_Amt = x.pur02disc_amt,
                Total = (x.pur02rate * x.pur02qty) - x.pur02disc_amt,
                VoucherNo = x.pur01purchases.pur01voucher_no,
                IsVoucherLinked = !string.IsNullOrEmpty(x.pur01purchases.pur01voucher_no)
            }).ToListAsync();
        }


        protected override IQueryable<VMPurchaseList> ProcessListData(IQueryable<pur01purchases> data)
        {
            data.Include(x => x.ven01vendors);
            return data.Select(purchase => new VMPurchaseList
            {
                Id = purchase.pur01uin,
                VendorId = purchase.pur01ven01uin,
                VendorName = purchase.ven01vendors.ven01name_eng,
                Date = purchase.pur01date,
                Invoice_No = purchase.pur01invoice_no,
                Remarks = purchase.pur01remarks,
                Sub_Total = purchase.pur01sub_total,
                AdditionalCharge = purchase.pur01additionalcharge,
                Disc_Amt = purchase.pur01disc_amt,
                Disc_Percentage = purchase.pur01disc_percentage,
                Additional_Disc_Amt = purchase.pur01additional_disc,
                VoucherNo = purchase.pur01voucher_no,
                IsVoucherLinked = !string.IsNullOrEmpty(purchase.pur01voucher_no),
                Total = purchase.pur01sub_total,
                VAT_Per = purchase.pur01vat_per,
                VAT_Amt = purchase.pur01vat_amt,
                Net_Amt = purchase.pur01net_amt
            });
        }

        protected override VMPurchaseDetail ProcessDetailData(pur01purchases data)
        {

            try
            {
                var purchaseEntity = _MainRepo.GetDetail(data.pur01uin);

                if (purchaseEntity == null)
                {
                    throw new Exception("Purchase ID not found");
                }
                var vendor = _vendorRepo.GetList().FirstOrDefault(x => x.ven01uin == data.pur01ven01uin);

                var VendorData = new VMViewVendorData()
                {
                    ID = vendor.ven01uin,
                    Name = vendor.ven01name_eng,
                    Address = vendor.ven01address,
                    Registration_No = vendor.ven01reg_no,
                    Tel_No = vendor.ven01tel
                };

                VMPurchaseDetail Result = new VMPurchaseDetail()
                {
                    Id = data.pur01uin,
                    VendorData = VendorData,
                    //VendorId = data.pur01ven01uin,
                    //VendorName = vendor.ven01name_eng,
                    Date = data.pur01date,
                    Invoice_No = data.pur01invoice_no,
                    
                    Remarks = data.pur01remarks,
                    Sub_Total = data.pur01sub_total,
                    Disc_Amt = data.pur01disc_amt,
                    Disc_Percentage = data.pur01disc_percentage,
                    Additional_Disc_Amt = data.pur01additional_disc,
                    VatClaimable = data.pur01vatclamable,
                    VatApplicable = data.pur01vatapplicable,
                    Total = data.pur01sub_total,
                    VAT_Per = data.pur01vat_per,
                    VAT_Amt = data.pur01vat_amt,
                    Net_Amt = data.pur01net_amt,
                    AdditionalCharges = data.pur01additionalcharge,
                    VoucherNo = data.pur01voucher_no,
                    IsVoucherLinked = !string.IsNullOrEmpty(data.pur01voucher_no),
                    //Net_Amt_Words = ((long)data.pur01net_amt).ToWords() + " only"
                    Net_Amt_Words = EnglishNepaliNumberConverter.ConvertToWords(data.pur01net_amt)
                };

                var _Que = _purchaseItemsRepo.GetList().Where(x => x.pur02pur01uin == data.pur01uin).Include(x => x.pro02products);
                var _data = _Que.Include(x => x.un01units).ToList();

                Result.VMPurchaseItemDetails = _data.Select(x => new VMPurchaseItemDetails()
                {
                    ID = x.pur02uin,
                    ProductID = x.pur02pro02uin,
                    HsCode = x.pro02products.pro02hscode,
                    Ratio = x.un01units.un01ratio,
                    ProductName = x.pro02products.pro02name_eng,
                    Quantity = x.pur02qty,
                    UnitID = x.pur02un01uin,
                    UnitName = x.un01units.un01name_eng,
                    Rate = x.pur02rate,
                    Amount = x.pur02amount,
                    Mgf_Date = x.pur02mfg_date,
                    Exp_Date = x.pur02exp_date,
                    Batch_No = x.pur02batch_no,
                    Disc_Amt = x.pur02disc_amt,
                    Net_Amt = x.pur02net_amt
                })
                    .ToList();

                var addChargeData = _additionalChargesPurchaseRelation
                    .GetList()
                    .Where(x => x.add04purchaseuin == data.pur01uin)
                    .Include(x => x.PurchaseAdditionalCharges)
                    .ThenInclude(x => x.AdditionalChargesDetails)
                    .ToList();

                List<ViewChargeData> additionalChargeList = new List<ViewChargeData>();

                if(addChargeData != null)
                {
                    foreach (var x in addChargeData)
                    {
                        var viewChargeData = new ViewChargeData()
                        {
                            Id = x.add04uin,
                            Title = x.PurchaseAdditionalCharges.AdditionalChargesDetails
                                .FirstOrDefault()?.add03title,
                            Amount = x.PurchaseAdditionalCharges.AdditionalChargesDetails
                                .FirstOrDefault()?.add03amount ?? 0,
                            Remarks = x.PurchaseAdditionalCharges.AdditionalChargesDetails
                                .FirstOrDefault()?.add03remarks
                        };

                        additionalChargeList.Add(viewChargeData);
                    }

                    Result.AdditionalChargeList = additionalChargeList;
                }

                return Result;
            }
            catch(Exception ex)
            {
                throw new Exception($"Error processing purchase details: {ex.Message}", ex);
            }
        }

        protected VMPurchaseDetailWithReturnDetails ProcessDetailWithReturnData(pur01purchases data)
        {
            var purchaseDetail = ProcessDetailData(data);

            var purchaseReturnDetails = data.pur01purchasereturns.Select(pr => new VMPurchaseReturnDetail
            {
                Id = pr.pur01returnuin,
                VendorId = pr.pur01ven01uin,
                VendorName = _vendorRepo.GetList().FirstOrDefault(v => v.ven01uin == pr.pur01ven01uin)?.ven01name_eng,
                Date = pr.pur01return_date,
                Invoice_No = pr.pur01return_invoice_no,
                Remarks = pr.pur01return_remarks,
                Sub_Total = pr.pur01return_sub_total,
                Disc_Amt = pr.pur01return_disc_amt,
                Additional_Disc_Amt = pr.pur01return_additional_disc,
                Total = pr.pur01return_total,
                VAT_Per = (decimal)pr.pur01return_vat_per,
                VAT_Amt = pr.pur01return_vat_amt,
                Net_Amt = pr.pur01return_net_amt,
                VMPurchaseReturnItemDetails = pr.pur02returnitems.Select(ri => new VMPurchaseReturnItemDetails
                {
                    ID = ri.pur02returnuin,
                    ProductID = ri.pur02returnpro02uin,
                    ProductName = ri.pro02products?.pro02name_eng,
                    Quantity = ri.pur02returnqty,
                    UnitID = ri.pur02returnun01uin,
                    UnitName = ri.un01units?.un01name_eng,
                    Rate = ri.pur02returnrate,
                    Amount = ri.pur02returnamount,
                    Mgf_Date = ri.pur02returnmfg_date,
                    Exp_Date = ri.pur02returnexp_date,
                    Batch_No = ri.pur02returnbatch_no,
                    Disc_Amt = ri.pur02returndisc_amt,
                    Net_Amt = ri.pur02net_amt
                }).ToList()
            }).ToList();

            var groupedReturnItems = purchaseReturnDetails
                .SelectMany(pr => pr.VMPurchaseReturnItemDetails)
                .GroupBy(ri => ri.ProductID)
                .Select(group => new GroupedReturnItem
                {
                    ProductID = group.Key,
                    ProductName = group.FirstOrDefault()?.ProductName,
                    TotalQuantity = group.Sum(ri => ri.Quantity)
                }).ToList();

            return new VMPurchaseDetailWithReturnDetails
            {
                VMPurchaseDetail = purchaseDetail,
                VMPurchaseReturnDetail = purchaseReturnDetails,
                GroupedReturnItems = groupedReturnItems
            };
        }

        protected override pur01purchases AssignValues(VMPurchase Data)
        {
            return null;
        }


        [HttpPost("Create/V1")]
        public virtual async Task<IActionResult> Create(VMPurchase Data)
        {
            try
            {
                pur01purchases newData = await AssignValuesV1(Data);
                _purchaseRepo.Insert(newData);
                await _purchaseRepo.SaveAsync();

                await AddAditionalCharges(Data, newData);

                return Ok("Created Succesfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch("Update/V1")]
        public async Task<IActionResult> Update(int id, VMPurchase Data)
        {
            try
            {

                var oldData = await _MainRepo.GetDetailAsync(id);
                if (oldData == null) { throw new Exception("Invalid  ID"); }

                await ReAssignValuesV1(Data, oldData);

                _MainRepo.Update(oldData);
                await _MainRepo.SaveAsync();
                await AfterUpdate(oldData);

                await UpdateCharges(Data.AdditionalCharge, oldData);

                return Ok("Data Updated Successfully.");

            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message);
            }
        }

        protected async Task<pur01purchases> ReAssignValuesV1(VMPurchase Data, pur01purchases oldData)
        {
            {
                var vendorDetail = _vendorRepo.GetDetail(Data.VendorID);
                if (vendorDetail == null)
                {
                    throw new Exception("Vendor ID not found");
                }

                if (Data.Disc_Percentage != 0)
                {
                    Data.Disc_Amt = (Data.Sub_Total * Data.Disc_Percentage) / 100;
                }

                decimal vatPercentage = GetVatData();

                decimal vatApplicableTotal = Data.VMPurchaseItems
                       .Sum(item => item.Net_Amt);

                if (vatApplicableTotal != 0)
                {
                    vatApplicableTotal -= Data.Disc_Amt;
                }

                decimal vatAmount = ((decimal)vatApplicableTotal * vatPercentage) / 100;

                oldData.pur01ven01uin = Data.VendorID;
                oldData.pur01date = Data.Purchase_Date;
                oldData.pur01remarks = Data.Remarks;
                oldData.pur01invoice_no = Data.Invoice_No;
                oldData.pur01remarks = Data.Remarks;
                oldData.pur01sub_total = Data.Sub_Total;
                oldData.pur01disc_amt = Data.Disc_Amt;
                oldData.pur01vatapplicable = Data.VatApplicable;
                oldData.pur01vatclamable = Data.VatClaimable;
                oldData.pur01disc_percentage = Data.Disc_Percentage;
                oldData.pur01additional_disc = Data.Additional_Disc_Amt;
                oldData.pur01total = Data.Total;
                oldData.pur01vat_per = (double)vatPercentage;
                oldData.pur01vat_amt = vatAmount;
                oldData.pur01net_amt = Data.Net_Amt + vatAmount;

                oldData.DateUpdated = DateTime.Now;
                oldData.UpdatedName = _ActiveUserName;

                _purchaseItemsRepo.LoadPurchaseItemDetails(oldData);

                if (Data.VMPurchaseItems != null)
                {
                    SoftDeleteNonExisingChild(Data, oldData);

                    AddUpdateNewChildRecords(Data, oldData);
                }

                return oldData;
            }
        }

        protected async Task<pur01purchases> AssignValuesV1(VMPurchase Data)
        {
            var branchCode = _httpContextAccessor.HttpContext.User.FindFirst("BranchCode")?.Value;

            // Check if BranchCode exists
            if (string.IsNullOrEmpty(branchCode))
            {
                throw new Exception("BranchCode is missing or invalid in the token");
            }

            var vendorDetail = _vendorRepo.GetDetail(Data.VendorID);
            if (vendorDetail == null)
            {
                throw new Exception("Vendor ID not found");
            }

            if (Data.Disc_Percentage != 0)
            {
                Data.Disc_Amt = (Data.Sub_Total * Data.Disc_Percentage) / 100;
            }

            if (Data.Invoice_No == null)
            {
                Data.Invoice_No = _globalService.GetInvoiceNumber();
            }

            decimal vatPercentage = GetVatData();

            decimal vatApplicableTotal = Data.VMPurchaseItems
                   .Sum(item => item.Net_Amt);

            if (vatApplicableTotal != 0)
            {
                vatApplicableTotal -= Data.Disc_Amt;
            }

            decimal vatAmount = ((decimal)vatApplicableTotal * vatPercentage) / 100;

            var purchaseEntity = new pur01purchases
            {
                pur01ven01uin = Data.VendorID,
                pur01date_nep = "2080-06-06 ",
                pur01date = Data.Purchase_Date,
                pur01invoice_no = Data.Invoice_No,
                pur01remarks = Data.Remarks,
                pur01sub_total = Data.Sub_Total,
                pur01disc_amt = Data.Disc_Amt,
                pur01disc_percentage = Data.Disc_Percentage,
                pur01additional_disc = Data.Additional_Disc_Amt,
                pur01total = Data.Total,
                pur01vatapplicable = Data.VatApplicable,
                pur01vatclamable = Data.VatClaimable,
                pur01vat_per = (double)vatPercentage,
                pur01vat_amt = vatAmount,
                pur01net_amt = Data.Net_Amt + vatAmount,
                BranchCode = branchCode,

                DateCreated = DateTime.Now,
                CreatedName = _ActiveUserName,
                pur02items = new List<pur02items>(),
            };

            if (Data.VMPurchaseItems != null)
            {
                foreach (var purchaseItem_Detail in Data.VMPurchaseItems)
                {
                    var product = _productsRepo.GetDetail(purchaseItem_Detail.ProductID);

                    if (product == null)
                    {
                        throw new Exception("Product ID not found");
                    }

                    var unit = _unitsRepo.GetDetail(purchaseItem_Detail.UnitID);

                    if (unit == null)
                    {
                        throw new Exception("Unit ID not found");
                    }

                    var pruchaseItemDetailEntity = new pur02items
                    {
                        pur02pro02uin = product.pro02uin,
                        pur02pur01uin = purchaseEntity.pur01uin,
                        pur02un01uin = unit.un01uin,
                        pur02qty = purchaseItem_Detail.Quantity,
                        pur02rate = purchaseItem_Detail.Rate,
                        pur02amount = purchaseItem_Detail.Amount,
                        pur02mfg_date = purchaseItem_Detail.Mgf_Date,
                        pur02exp_date = purchaseItem_Detail.Exp_Date,
                        pur02batch_no = purchaseItem_Detail.Batch_No,
                        pur02disc_amt = purchaseItem_Detail.Disc_Amt,
                        pur02net_amt = purchaseItem_Detail.Net_Amt,

                        DateCreated = DateTime.Now,
                        CreatedName = _ActiveUserName,
                    };

                    purchaseEntity.pur02items.Add(pruchaseItemDetailEntity);
                }
            }

            if (!_ledgerService.UpdateLedgerOnPurhcase(purchaseEntity).GetAwaiter().GetResult())
            {
                throw new Exception("ledger not updated");
            }
            if (!_ledgerService.CreateLedgerOnPurchaseDisc(purchaseEntity).GetAwaiter().GetResult())
            {
                throw new Exception("Discout ledger not Created");
            }

            return purchaseEntity;
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

        private async Task AddAditionalCharges(VMPurchase Data, pur01purchases purchaseEntity)
        {
            if (Data.AdditionalCharge.Any())
            {
                add02purchaseadditionalcharges insertData = await CreateCharges(Data.AdditionalCharge);

                var insertRelData = new add04chargepurchaserel()
                {
                    add04puraddchargeuin = insertData.add02uin,
                    add04purchaseuin = purchaseEntity.pur01uin,
                };

                _additionalChargesPurchaseRelation.Insert(insertRelData);

                purchaseEntity.pur01additionalcharge = Data.AdditionalCharge.Sum(charge => charge.Amount);

                _purchaseRepo.Update(purchaseEntity);
                await _purchaseRepo.SaveAsync();

                if (!_ledgerService.UpdateLedgerOnAdditionalCharge(Data.AdditionalCharge.ToList()).GetAwaiter().GetResult())
                {
                    throw new Exception("Ledger not updated");
                }
            }
        }

        private async Task UpdateCharges(IList<ChargeData> updatedCharges, pur01purchases purchaseEntity)
        {
            var existingRelations = _additionalChargesPurchaseRelation
                .GetList()
                .Where(x => x.add04purchaseuin == purchaseEntity.pur01uin)
                .ToList();

            var existingChargeIds = existingRelations.Select(x => x.add04puraddchargeuin).ToList();
            var updatedChargeIds = updatedCharges.Select(c => c.Id).ToList();

            DeleteCharges(existingRelations, updatedChargeIds);

            foreach (var charge in updatedCharges)
            {
                var existingRelation = existingRelations.FirstOrDefault(x => x.add04puraddchargeuin == charge.Id);

                if (existingRelation != null)
                {
                    var existingCharge = _purchaseAdditionalCharges
                        .GetList()
                        .FirstOrDefault(x => x.add02uin == existingRelation.add04puraddchargeuin);

                    if (existingCharge != null)
                    {
                        var chargeDetail = existingCharge.AdditionalChargesDetails.FirstOrDefault();
                        if (chargeDetail != null)
                        {
                            chargeDetail.add03title = charge.Title;
                            chargeDetail.add03amount = charge.Amount;
                            chargeDetail.add03remarks = charge.Remarks;
                        }

                        existingCharge.DateUpdated = DateTime.UtcNow;
                        existingCharge.UpdatedName = _ActiveUserName;

                        _purchaseAdditionalCharges.Update(existingCharge);
                    }
                }
                else
                {
                    add02purchaseadditionalcharges newCharge = await CreateCharges(new List<ChargeData> { charge });

                    var newRelation = new add04chargepurchaserel()
                    {
                        add04puraddchargeuin = newCharge.add02uin,
                        add04purchaseuin = purchaseEntity.pur01uin,
                    };

                    _additionalChargesPurchaseRelation.Insert(newRelation);
                }
            }

            await _purchaseAdditionalCharges.SaveAsync();
            await _additionalChargesPurchaseRelation.SaveAsync();

            purchaseEntity.pur01additionalcharge = updatedCharges.Sum(c => c.Amount);
            _purchaseRepo.Update(purchaseEntity);
            await _purchaseRepo.SaveAsync();

            if (!_ledgerService.UpdateLedgerOnAdditionalCharge(updatedCharges.ToList()).GetAwaiter().GetResult())
            {
                throw new Exception("Ledger not updated");
            }
        }

        private void DeleteCharges(List<add04chargepurchaserel> existingRelations, List<int> updatedChargeIds)
        {
            var chargesToDelete = existingRelations.Where(rel => !updatedChargeIds.Contains(rel.add04puraddchargeuin)).ToList();

            foreach (var relation in chargesToDelete)
            {
                _additionalChargesPurchaseRelation.Delete(relation);

                var chargeToDelete = _purchaseAdditionalCharges
                    .GetList()
                    .FirstOrDefault(x => x.add02uin == relation.add04puraddchargeuin);

                if (chargeToDelete != null)
                {
                    _purchaseAdditionalCharges.Delete(chargeToDelete);
                }
            }
        }

        private async Task<add02purchaseadditionalcharges> CreateCharges(IList<ChargeData> dtos)
        {
            var entitiesToInsert = new List<add03purchaseadditionalchargesdetail>();

            foreach (var d in dtos)
            {
                var data = new add03purchaseadditionalchargesdetail()
                {
                    add03title = d.Title,
                    add03amount = d.Amount,
                    add03remarks = d.Remarks,
                };

                entitiesToInsert.Add(data);
            }

            var insertData = new add02purchaseadditionalcharges
            {
                DateCreated = DateTime.UtcNow,
                CreatedName = _ActiveUserName,
                // Map other properties from dto as needed
                AdditionalChargesDetails = entitiesToInsert,
            };

            _purchaseAdditionalCharges.Insert(insertData);
            await _purchaseAdditionalCharges.SaveAsync();
            return insertData;
        }

        protected override async Task AfterUpdate(pur01purchases data)
        {
            var response = await _ledgerService.UpdateLedgerOnPurhcase(data);
            if (!response)
            {
                throw new Exception("ledger not updated");
            }
        }
        protected override void ReAssignValues(VMPurchase Data, pur01purchases oldData)
        {
            var vendorDetail = _vendorRepo.GetDetail(Data.VendorID);
            if (vendorDetail == null)
            {
                throw new Exception("Vendor ID not found");
            }

            if (Data.Disc_Percentage != 0)
            {
                Data.Disc_Amt = (Data.Sub_Total * Data.Disc_Percentage) / 100;
            }

            oldData.pur01ven01uin = Data.VendorID;
            oldData.pur01date = Data.Purchase_Date;
            oldData.pur01remarks = Data.Remarks;
            oldData.pur01invoice_no = Data.Invoice_No;
            oldData.pur01remarks = Data.Remarks;
            oldData.pur01sub_total = Data.Sub_Total;
            oldData.pur01disc_amt = Data.Disc_Amt;
            oldData.pur01vatapplicable = Data.VatApplicable;
            oldData.pur01vatclamable = Data.VatClaimable;
            oldData.pur01disc_percentage = Data.Disc_Percentage;
            oldData.pur01additional_disc = Data.Additional_Disc_Amt;
            oldData.pur01total = Data.Total;
            oldData.pur01vat_per = Data.VAT_Per;
            oldData.pur01vat_amt = Data.VAT_Amt;
            oldData.pur01net_amt = Data.Net_Amt;

            oldData.DateUpdated = DateTime.Now;
            oldData.UpdatedName = _ActiveUserName;

            _purchaseItemsRepo.LoadPurchaseItemDetails(oldData);

            if (Data.VMPurchaseItems != null)
            {
                SoftDeleteNonExisingChild(Data, oldData);

                AddUpdateNewChildRecords(Data, oldData);
            }
        }

        private static void SoftDeleteNonExisingChild(VMPurchase Data, pur01purchases oldData)
        {
            int[] ExistingIDs = Data
                .VMPurchaseItems.Where(x => x.ID > 0)
                .Select(x => x.ID)
                .ToArray();

            // Remove any details that are no longer present in the updated data
            var detailsToRemove = oldData.pur02items
                .Where(d => !ExistingIDs.Contains(d.pur02uin))
                .ToList();

            foreach (var detailToRemove in detailsToRemove)
            {
                detailToRemove.DateDeleted = DateTime.UtcNow;
            }
        }

        private void AddUpdateNewChildRecords(VMPurchase Data, pur01purchases oldData)
        {
            List<pur02items> oldDataItems = oldData.pur02items.ToList().GetRange(0, oldData.pur02items.Count);

            foreach (var updatedPurchaseItemDetail in Data.VMPurchaseItems)
            {
                var product = _productsRepo.GetDetail(updatedPurchaseItemDetail.ProductID);

                if (product == null)
                {
                    throw new Exception("Product ID not found");
                }

                var unit = _unitsRepo.GetDetail(updatedPurchaseItemDetail.UnitID);

                if (unit == null)
                {
                    throw new Exception("Unit ID not found");
                }

                var existingDetail = oldDataItems
                    .FirstOrDefault(d => d.pur02uin == updatedPurchaseItemDetail.ID);

                bool CreateMode = false;

                if (existingDetail == null)
                {
                    CreateMode = true;
                    existingDetail = new pur02items()
                    {
                        pur02uin = 0,
                    };
                }

                existingDetail.pur02pro02uin = product.pro02uin;
                existingDetail.pur02un01uin = unit.un01uin;
                existingDetail.pur02qty = updatedPurchaseItemDetail.Quantity;
                existingDetail.pur02rate = updatedPurchaseItemDetail.Rate;
                existingDetail.pur02amount = updatedPurchaseItemDetail.Amount;
                existingDetail.pur02batch_no = updatedPurchaseItemDetail.Batch_No;
                existingDetail.pur02disc_amt = updatedPurchaseItemDetail.Disc_Amt;
                existingDetail.pur02net_amt = updatedPurchaseItemDetail.Net_Amt;
                existingDetail.pur02exp_date = updatedPurchaseItemDetail.Exp_Date;
                existingDetail.pur02mfg_date = updatedPurchaseItemDetail.Mgf_Date;

                existingDetail.DateUpdated = DateTime.Now;
                existingDetail.UpdatedName = _ActiveUserName;
                if (CreateMode) { oldData.pur02items.Add(existingDetail); }

            }
        }
    }
}
