using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Models.EntityModels.Inventory;
using POSV1.TenantAPI.Services;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Implementation;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseReturnController :
        _AbsCRUDWithDiffInputModelController<
            PurchaseReturnController,
            IPurchaseReturnRepo,
            pur01purchasereturns,
            VMPurchaseReturn,
            VMPurchaseReturnList,
            VMPurchaseReturnDetail,
            int>
    {
        private IVendorRepo _vendorRepo;
        private IProductsRepo _productsRepo;
        private IUnitsRepo _unitsRepo;
        private readonly IPurchaseReturnRepo _purchaseReturnRepo;
        private IPurchasereturnitemsRepo _purchaseReturnItemsRepo;
        private readonly IledgerService _ledgerService;
        public PurchaseReturnController(
            ILogger<PurchaseReturnController> logger,
            IPurchaseReturnRepo purchaseReturnRepo,
            IMapper mapper,
            IVendorRepo vendorRepo,
            IProductsRepo productsRepo,
            IUnitsRepo unitsRepo,
            IPurchasereturnitemsRepo purchaseReturnItemsRepo,
            IledgerService ledgerService)
            : base(logger, purchaseReturnRepo, mapper)
        {
            _vendorRepo = vendorRepo;
            _productsRepo = productsRepo;
            _unitsRepo = unitsRepo;
            _purchaseReturnItemsRepo = purchaseReturnItemsRepo;
            _ledgerService = ledgerService;
            _purchaseReturnRepo = purchaseReturnRepo;
        }

        //[HttpGet("GetDetail/{InvoiceNo}")]
        //public virtual async Task<VMPurchaseReturnDetail> GetDetail(string InvoiceNo)
        //{
        //    var _que = await _MainRepo.GetList().Where(x => x.pur01return_invoice_no == InvoiceNo).FirstOrDefaultAsync();  //get the data from db

        //    if (_que == null)
        //    {
        //        throw new Exception("Data not found !!!");
        //    }
        //    var result = ProcessDetailData(_que); // process the data as per the view model

        //    return result;
        //}

        [HttpGet("GetFilteredList")]
        public virtual async Task<ActionResult<PageResult<VMPurchaseReturnList>>> GetFilteredList(
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
                    query = query.Where(x => x.pur01return_invoice_no.Contains(invoiceNo));
                }

                if (startDate.HasValue && endDate.HasValue)
                {
                    query = query.Where(x => x.pur01return_date >= startDate.Value && x.pur01return_date <= endDate.Value);
                }

                var totalCount = await query.CountAsync();

                var processedQuery = ProcessListData(query);

                var resultList = await processedQuery
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToListAsync();

                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize.Value);

                var pageResult = new PageResult<VMPurchaseReturnList>
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

                var query = _purchaseReturnItemsRepo.GetList()
                    .Include(x => x.pro02products)
                    .Include(x => x.un01units)
                    .Include(x => x.pur01purchasereturns)
                        .ThenInclude(p => p.ven01vendors)
                    .AsNoTracking();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(vendorName))
                {
                    query = query.Where(x => x.pur01purchasereturns.ven01vendors.ven01name_eng.Contains(vendorName));
                }

                if (vendorId.HasValue)
                {
                    query = query.Where(x => x.pur01purchasereturns.pur01ven01uin == vendorId);
                }

                if (!string.IsNullOrWhiteSpace(invoiceNo))
                {
                    query = query.Where(x => x.pur01purchasereturns.pur01return_invoice_no.Contains(invoiceNo));
                }

                if (!string.IsNullOrWhiteSpace(itemName))
                {
                    query = query.Where(x => x.pro02products.pro02name_eng.Contains(itemName));
                }

                if (startDate.HasValue && endDate.HasValue)
                {
                    query = query.Where(x => x.pur01purchasereturns.pur01return_date >= startDate.Value && x.pur01purchasereturns.pur01return_date <= endDate.Value);
                }

                var totalCount = await query.CountAsync();
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

        private async Task<List<VMPurchaseDetailList>> MapToPurchaseDetailList(IQueryable<pur02returnitems> query)
        {
            return await query.Select(x => new VMPurchaseDetailList
            {
                Id = x.pur01purchasereturns.pur01uin,

                VendorId = x.pur01purchasereturns.pur01ven01uin,
                VendorName = x.pur01purchasereturns.ven01vendors.ven01name_eng,
                Date = x.pur01purchasereturns.pur01return_date,
                Invoice_No = x.pur01purchasereturns.pur01return_invoice_no,
                ItemName = x.pro02products.pro02name_eng,
                itemId = x.pro02products.pro02uin,
                UnitName = x.un01units.un01name_eng,
                Quantity = x.pur02returnqty,
                Rate = x.pur02returnrate,
                Sub_Total = x.pur02returnrate * x.pur02returnqty,
                Disc_Amt = x.pur02returndisc_amt,
                Total = (x.pur02returnrate * x.pur02returnqty) - x.pur02returndisc_amt,
                VoucherNo = x.pur01purchasereturns.pur01returnvoucher_no,
                IsVoucherLinked = !string.IsNullOrEmpty(x.pur01purchasereturns.pur01returnvoucher_no)
            }).ToListAsync();
        }
        protected override IQueryable<VMPurchaseReturnList> ProcessListData(IQueryable<pur01purchasereturns> data)
        {
            data.Include(x => x.ven01vendors);
            return data.Select(purchaseReturn => new VMPurchaseReturnList
            {
                Id = purchaseReturn.pur01returnuin,
                VendorId = purchaseReturn.pur01ven01uin,
                VendorName = purchaseReturn.ven01vendors.ven01name_eng,
                Date = purchaseReturn.pur01return_date,
                Invoice_No = purchaseReturn.pur01return_invoice_no,
                Remarks = purchaseReturn.pur01return_remarks,
                Sub_Total = purchaseReturn.pur01return_sub_total,
                Disc_Amt = purchaseReturn.pur01return_disc_amt,
                Additional_Disc_Amt = purchaseReturn.pur01return_additional_disc,
                Total = purchaseReturn.pur01return_total,
                VAT_Per = (decimal)purchaseReturn.pur01return_vat_per,
                VAT_Amt = purchaseReturn.pur01return_vat_amt,
                Net_Amt = purchaseReturn.pur01return_net_amt,
                VoucherNo = purchaseReturn.pur01returnvoucher_no,
                IsVoucherLinked = ! string.IsNullOrEmpty(purchaseReturn.pur01returnvoucher_no)
            });
        }

        protected override VMPurchaseReturnDetail ProcessDetailData(pur01purchasereturns data)
        {
            var purchaseReturnEntity = _MainRepo.GetDetail(data.pur01returnuin);

            if (purchaseReturnEntity == null)
            {
                throw new Exception("Purchase Return ID not found");
            }

            var vendor = _vendorRepo.GetList().FirstOrDefault(x => x.ven01uin == data.pur01ven01uin);

            VMPurchaseReturnDetail Result = new VMPurchaseReturnDetail()
            {
                Id = data.pur01returnuin,
                VendorId = data.pur01ven01uin,
                VendorName = vendor?.ven01name_eng, // Handle null safely
                Date = data.pur01return_date,
                Invoice_No = data.pur01return_invoice_no,
                Remarks = data.pur01return_remarks,
                Sub_Total = data.pur01return_sub_total,
                Disc_Amt = data.pur01return_disc_amt,
                Additional_Disc_Amt = data.pur01return_additional_disc,
                Total = data.pur01return_total,
                VAT_Per = (decimal)data.pur01return_vat_per,
                VAT_Amt = data.pur01return_vat_amt,
                Net_Amt = data.pur01return_net_amt,
                VoucherNo = data.pur01returnvoucher_no,
                IsVoucherLinked = !string.IsNullOrEmpty(data.pur01returnvoucher_no),
                Net_Amt_Words = EnglishNepaliNumberConverter.ConvertToWords(data.pur01return_net_amt)
            };

            var _Que = _purchaseReturnItemsRepo.GetList().Where(x => x.pur02returnpur01uin == data.pur01returnuin).Include(x => x.pro02products);
            var _data = _Que.Include(x => x.un01units).ToList();

            Result.VMPurchaseReturnItemDetails = _data.Select(x => new VMPurchaseReturnItemDetails()
            {
                ID = x.pur02returnuin,
                ProductID = x.pur02returnpro02uin,
                HsCode = x.pro02products.pro02hscode,
                Ratio = x.un01units.un01ratio,
                ProductName = x.pro02products.pro02name_eng,
                Quantity = x.pur02returnqty,
                UnitID = x.pur02returnun01uin,
                UnitName = x.un01units.un01name_eng,
                Rate = x.pur02returnrate,
                Amount = x.pur02returnamount ?? 0,
                Mgf_Date = x.pur02returnmfg_date ?? DateTime.MinValue,
                Exp_Date = x.pur02returnexp_date ?? DateTime.MinValue,
                Batch_No = x.pur02returnbatch_no,
                Disc_Amt = x.pur02returndisc_amt,
                Net_Amt = x.pur02net_amt
            })
            .ToList();

            return Result;
        }

        protected override pur01purchasereturns AssignValues(VMPurchaseReturn Data)
        {
            var vendorDetail = _vendorRepo.GetDetail(Data.VendorID);
            if (vendorDetail == null)
            {
                throw new Exception("Vendor ID not found");
            }

            decimal subTotal = Data.VMPurchaseReturnItems.Sum(item => (item.Rate * item.Quantity) - item.Disc_Amt);

            var purchaseReturnEntity = new pur01purchasereturns
            {
                pur01ven01uin = Data.VendorID,
                pur01uin = Data.PurchaseId,
                pur01return_date_nep = "2080-06-06 ",
                pur01return_date = Data.Return_Date,
                pur01return_invoice_no = Data.Invoice_No,
                pur01return_remarks = Data.Remarks,
                pur01return_sub_total = subTotal,
                pur01return_disc_amt = 0,
                pur01return_additional_disc = 0,
                pur01return_total = subTotal,
                pur01return_vat_per = 0,
                pur01return_vat_amt = 0,
                pur01return_net_amt = subTotal,
                DateCreated = DateTime.Now,
                CreatedName = _ActiveUserName,
                pur02returnitems = new List<pur02returnitems>(),
            };

            if (Data.VMPurchaseReturnItems != null)
            {
                foreach (var purchaseReturnItem_Detail in Data.VMPurchaseReturnItems)
                {
                    var product = _productsRepo.GetDetail(purchaseReturnItem_Detail.ProductID);

                    if (product == null)
                    {
                        throw new Exception("Product ID not found");
                    }

                    var unit = _unitsRepo.GetDetail(purchaseReturnItem_Detail.UnitID);

                    if (unit == null)
                    {
                        throw new Exception("Unit ID not found");
                    }

                    var pruchaseReturnItemDetailEntity = new pur02returnitems
                    {
                        pur02returnpro02uin = product.pro02uin,
                        pur02returnpur01uin = purchaseReturnEntity.pur01returnuin,
                        pur02returnun01uin = unit.un01uin,
                        pur02returnqty = (int)purchaseReturnItem_Detail.Quantity,
                        pur02returnrate = purchaseReturnItem_Detail.Rate,
                        //pur02returnamount = purchaseReturnItem_Detail.Amount,
                        pur02returnamount = purchaseReturnItem_Detail.Rate * (int)purchaseReturnItem_Detail.Quantity,
                        pur02returnmfg_date = purchaseReturnItem_Detail.Mgf_Date,
                        pur02returnexp_date = purchaseReturnItem_Detail.Exp_Date,
                        pur02returnbatch_no = purchaseReturnItem_Detail.Batch_No,
                        pur02returndisc_amt = purchaseReturnItem_Detail.Disc_Amt,
                        //pur02net_amt = purchaseReturnItem_Detail.Net_Amt,
                        pur02net_amt = (purchaseReturnItem_Detail.Rate * (int)purchaseReturnItem_Detail.Quantity) - purchaseReturnItem_Detail.Disc_Amt,
                        DateCreated = DateTime.Now,
                        CreatedName = _ActiveUserName,
                    };

                    purchaseReturnEntity.pur02returnitems.Add(pruchaseReturnItemDetailEntity);
                }
            }

            return purchaseReturnEntity;
        }

        protected override async Task AfterUpdate(pur01purchasereturns data)
        {
            var response = await _ledgerService.UpdateLedgerOnPurchaseReturn(data);
            if (!response)
            {
                throw new Exception("ledger not updated");
            }
        }

        protected override void ReAssignValues(VMPurchaseReturn Data, pur01purchasereturns oldData)
        {
            var vendorDetail = _vendorRepo.GetDetail(Data.VendorID);
            if (vendorDetail == null)
            {
                throw new Exception("Vendor ID not found");
            }

            decimal subTotal = Data.VMPurchaseReturnItems.Sum(item => (item.Rate * item.Quantity) - item.Disc_Amt);

            oldData.pur01ven01uin = Data.VendorID;
            oldData.pur01return_date = Data.Return_Date;
            oldData.pur01return_remarks = Data.Remarks;
            oldData.pur01return_invoice_no = Data.Invoice_No;
            oldData.pur01return_sub_total = subTotal;
            oldData.pur01return_disc_amt = 0;
            oldData.pur01return_additional_disc = 0;
            oldData.pur01return_total = subTotal;
            oldData.pur01return_vat_per = 0;
            oldData.pur01return_vat_amt = 0;
            oldData.pur01return_net_amt = subTotal;
            oldData.DateUpdated = DateTime.Now;
            oldData.UpdatedName = _ActiveUserName;

            _purchaseReturnItemsRepo.LoadPurchaseReturnItemDetails(oldData);

            if (Data.VMPurchaseReturnItems != null)
            {
                SoftDeleteNonExisingChild(Data, oldData);
                AddUpdateNewChildRecords(Data, oldData);
            }
        }

        private static void SoftDeleteNonExisingChild(VMPurchaseReturn Data, pur01purchasereturns oldData)
        {
            int[] ExistingIDs = Data
                .VMPurchaseReturnItems.Where(x => x.ID > 0)
                .Select(x => x.ID)
                .ToArray();

            // Remove any details that are no longer present in the updated data
            var detailsToRemove = oldData.pur02returnitems
                .Where(d => !ExistingIDs.Contains(d.pur02returnuin))
                .ToList();

            foreach (var detailToRemove in detailsToRemove)
            {
                detailToRemove.DateDeleted = DateTime.UtcNow;
            }
        }

        private void AddUpdateNewChildRecords(VMPurchaseReturn Data, pur01purchasereturns oldData)
        {
            List<pur02returnitems> oldDataItems = oldData.pur02returnitems.ToList().GetRange(0, oldData.pur02returnitems.Count);

            foreach (var updatedPurchaseReturnItemDetail in Data.VMPurchaseReturnItems)
            {
                var product = _productsRepo.GetDetail(updatedPurchaseReturnItemDetail.ProductID);

                if (product == null)
                {
                    throw new Exception("Product ID not found");
                }

                var unit = _unitsRepo.GetDetail(updatedPurchaseReturnItemDetail.UnitID);

                if (unit == null)
                {
                    throw new Exception("Unit ID not found");
                }

                var existingDetail = oldDataItems
                    .FirstOrDefault(d => d.pur02returnuin == updatedPurchaseReturnItemDetail.ID);

                bool CreateMode = false;

                if (existingDetail == null)
                {
                    CreateMode = true;
                    existingDetail = new pur02returnitems()
                    {
                        pur02returnuin = 0,
                    };
                }

                existingDetail.pur02returnpro02uin = product.pro02uin;
                existingDetail.pur02returnun01uin = unit.un01uin;
                existingDetail.pur02returnqty = (int)updatedPurchaseReturnItemDetail.Quantity;
                existingDetail.pur02returnrate = updatedPurchaseReturnItemDetail.Rate;
                existingDetail.pur02returnamount = updatedPurchaseReturnItemDetail.Rate * (int)updatedPurchaseReturnItemDetail.Quantity;
                existingDetail.pur02returnbatch_no = updatedPurchaseReturnItemDetail.Batch_No;
                existingDetail.pur02returndisc_amt = updatedPurchaseReturnItemDetail.Disc_Amt;
                existingDetail.pur02net_amt = (updatedPurchaseReturnItemDetail.Rate * (int)updatedPurchaseReturnItemDetail.Quantity) - updatedPurchaseReturnItemDetail.Disc_Amt;
                existingDetail.pur02returnexp_date = updatedPurchaseReturnItemDetail.Exp_Date;
                existingDetail.pur02returnmfg_date = updatedPurchaseReturnItemDetail.Mgf_Date;

                existingDetail.DateUpdated = DateTime.Now;
                existingDetail.UpdatedName = _ActiveUserName;
                if (CreateMode) { oldData.pur02returnitems.Add(existingDetail); }
            }
        }
    }
}
