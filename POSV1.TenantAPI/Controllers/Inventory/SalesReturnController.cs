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
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;
using POSV1.TenantModel.Repo.Interface.Accounting;

namespace POSV1.TenantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesReturnController :
        _AbsCRUDWithDiffInputModelController<SalesReturnController, ISaleReturnRepo, sal01salesreturn, VMSalesReturn, VMSalesReturnList, VMSalesReturnDetail, int>
    {
        private readonly ICustomersRepo _customersRepo;
        private readonly IProductsRepo _productsRepo;
        private readonly IUnitsRepo _unitsRepo;
        private readonly ISalesItemReturnRepo _saleReturnItemsRepo;
        private readonly ILedgersRepo _ledgerRepo;
        private readonly IledgerService _ledgerService;

        public SalesReturnController(
            ILogger<SalesReturnController> logger,
            ISaleReturnRepo salesReturnRepo,
            IMapper mapper,
            ICustomersRepo customersRepo,
            IProductsRepo productsRepo,
            IUnitsRepo unitsRepo,
            ISalesItemReturnRepo saleReturnItemsRepo,
            IledgerService ledgerService)
            : base(logger, salesReturnRepo, mapper)
        {
            _customersRepo = customersRepo;
            _productsRepo = productsRepo;
            _unitsRepo = unitsRepo;
            _saleReturnItemsRepo = saleReturnItemsRepo;
            _ledgerService = ledgerService;
        }

        [HttpGet("GetFilteredList")]
        public virtual async Task<ActionResult<PageResult<VMSalesReturnList>>> GetFilteredList(
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

                var pageResult = new PageResult<VMSalesReturnList>
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

                var query = _saleReturnItemsRepo.GetList()
                    .Include(x => x.pro02products)
                    .Include(x => x.un01units)
                    .Include(x => x.sal01salesreturn)
                        .ThenInclude(x => x.cus01customers)
                    .AsNoTracking();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(customerName))
                {
                    query = query.Where(x => x.sal01salesreturn.cus01customers.cus01name_eng.Contains(customerName));
                }

                if (customerId.HasValue)
                {
                    query = query.Where(x => x.sal01salesreturn.sal01cus01uin == customerId);
                }

                if (!string.IsNullOrWhiteSpace(invoiceNo))
                {
                    query = query.Where(x => x.sal01salesreturn.sal01invoice_no.Contains(invoiceNo));
                }

                if (!string.IsNullOrWhiteSpace(itemName))
                {
                    query = query.Where(x => x.pro02products.pro02name_eng.Contains(itemName));
                }

                if (startDate.HasValue && endDate.HasValue)
                {
                    query = query.Where(x => x.sal01salesreturn.sal01date_eng >= startDate.Value && x.sal01salesreturn.sal01date_eng <= endDate.Value);
                }

                var totalCount = await query.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize.Value);

                var resultList = await MapToSaleDetailList(query
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value));

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

        private async Task<List<VMSaleDetailList>> MapToSaleDetailList(IQueryable<sal02itemsreturn> data)
        {
            return await data.Select(x => new VMSaleDetailList
            {
                Id = x.sal01salesreturn.sal01uin,
                CustomerId = x.sal01salesreturn.sal01cus01uin,
                CustomerName = x.sal01salesreturn.cus01customers.cus01name_eng,
                Date = x.sal01salesreturn.sal01date_eng,
                Invoice_No = x.sal01salesreturn.sal01invoice_no,
                ItemName = x.pro02products.pro02name_eng,
                itemId = x.pro02products.pro02uin,
                UnitName = x.un01units.un01name_eng,
                Quantity = x.sal02qty,
                Rate = x.sal02rate,
                Sub_Total = x.sal02rate * (decimal)x.sal02qty,
                Disc_Amt = x.sal02disc_amt,
                Total = (x.sal02rate * (decimal)x.sal02qty) - x.sal02disc_amt,
            }).ToListAsync();
        }

        protected override IQueryable<VMSalesReturnList> ProcessListData(IQueryable<sal01salesreturn> data)
        {
            data.Include(x => x.cus01customers);

            return data.Select(returnItem => new VMSalesReturnList
            {
                Id = returnItem.sal01uin,
                CustomerId = returnItem.sal01cus01uin,
                SaleId = returnItem.sal01_salId,
                CustomerName = returnItem.cus01customers.cus01name_eng,
                Date = returnItem.sal01date_eng,
                Invoice_No = returnItem.sal01invoice_no,
                Remarks = returnItem.sal01remarks,
                Sub_Total = returnItem.sal01sub_total,
                Disc_Amt = returnItem.sal01disc_amt,
                Total = returnItem.sal01total,
                VAT_Per = returnItem.sal01vat_per,
                VAT_Amt = returnItem.sal01vat_amt,
                Net_Amt = returnItem.sal01net_amt,
                VoucherNo = returnItem.sal01returnvoucher_no,
                IsVoucherLinked = !string.IsNullOrEmpty(returnItem.sal01returnvoucher_no)
            });
        }

        protected override VMSalesReturnDetail ProcessDetailData(sal01salesreturn data)
        {
            if (data == null)
            {
                throw new Exception("Sales return not found");
            }
            var customer = _customersRepo.GetDetail(data.sal01cus01uin);

            VMSalesReturnDetail result = new VMSalesReturnDetail()
            {
                Id = data.sal01uin,
                CustomerId = data.sal01cus01uin,
                SaleId = data.sal01_salId,
                CustomerName = customer.cus01name_eng,
                Date = data.sal01date_eng,
                Invoice_No = data.sal01invoice_no,
                Remarks = data.sal01remarks,
                Sub_Total = data.sal01sub_total,
                Disc_Amt = data.sal01disc_amt,
                Total = data.sal01total,
                VAT_Per = data.sal01vat_per,
                VAT_Amt = data.sal01vat_amt,
                Net_Amt = data.sal01net_amt,
                VoucherNo = data.sal01returnvoucher_no,
                IsVoucherLinked = !string.IsNullOrEmpty(data.sal01returnvoucher_no),
                Net_Amt_Words = EnglishNepaliNumberConverter.ConvertToWords(data.sal01net_amt)
            };

            IQueryable<sal02itemsreturn> query = _saleReturnItemsRepo.GetList().Where(x => x.sal02sal01uin == data.sal01uin);

            if (query == null)
            {
                throw new Exception("Sales return items not found");
            }

            var items = query.Include(x => x.pro02products)
                             .Include(x => x.un01units)
                             .ToList();

            result.VMSalesReturnItemDetails = items.Select(x => new VMSalesReturnItemDetails()
            {
                ID = x.sal02uin,
                ProductID = x.sal02pro02uin,
                HsCode = x.pro02products.pro02hscode,
                Ratio = x.un01units.un01ratio,
                ProductName = x.pro02products.pro02name_eng,
                Quantity = x.sal02qty,
                UnitID = x.sal02un01uin,
                UnitName = x.un01units.un01name_eng,
                Rate = x.sal02rate,
                Sub_Total = x.sal02sub_total,
                Disc_Amt = x.sal02disc_amt,
                Net_Amt = x.sal02net_amt
            }).ToList();

            return result;
        }

        //[HttpPost("CreateBulkReturn")]
        //public async Task<ActionResult> BulkCreateSalesReturns(VMBulkSalesReturnInformation data)
        //{
        //    try
        //    {
        //        List<sal01salesreturn> returnEntities = new List<sal01salesreturn>();

        //        IList<int> customerIds = data.Returns.Select(x => x.CustomerId).Distinct().ToList();

        //        foreach (var customerId in customerIds)
        //        {
        //            VMBulkSalesReturn returnItem = data.Returns.FirstOrDefault(x => x.CustomerId == customerId);

        //            var customerDetail = await _customersRepo.GetDetailAsync(returnItem.CustomerId);
        //            if (customerDetail == null)
        //            {
        //                return BadRequest($"Customer with ID {customerId} not found");
        //            }

        //            Dictionary<int, pro02products> productCache = new Dictionary<int, pro02products>();

        //            sal01salesReturn newReturn = new sal01salesReturn
        //            {
        //                sal01cus01uin = returnItem.CustomerId,
        //                sal01date_nep = "2080-06-06",
        //                sal01date_eng = data.TransactionDate,
        //                sal01invoice_no = returnItem.Invoice_No,
        //                sal01remarks = data.Remarks,
        //                sal01sub_total = returnItem.Sub_Total,
        //                sal01disc_amt = 0,
        //                sal01total = returnItem.Total,
        //                sal01vat_per = (double)returnItem.VatPercent,
        //                sal01vat_amt = returnItem.VAT_Amt,
        //                sal01net_amt = returnItem.Net_Amt,
        //                sal02returnItems = new List<sal02returnItems>()
        //            };

        //            foreach (VMBulkSalesReturn returnDetail in data.Returns.Where(x => x.CustomerId == customerId))
        //            {
        //                pro02products product;
        //                if (productCache.ContainsKey(returnDetail.ProductID))
        //                {
        //                    product = productCache[returnDetail.ProductID];
        //                }
        //                else
        //                {
        //                    product = await _productsRepo.GetDetailAsync(returnDetail.ProductID);
        //                    if (product == null)
        //                    {
        //                        return BadRequest($"Product with ID {returnDetail.ProductID} not found");
        //                    }
        //                    productCache.Add(returnDetail.ProductID, product);
        //                }

        //                var unit = await _unitsRepo.GetDetailAsync(returnDetail.UnitID);
        //                if (unit == null)
        //                {
        //                    return BadRequest($"Unit with ID {returnDetail.UnitID} not found");
        //                }

        //                sal02returnItems currentReturnItem = new sal02returnItems
        //                {
        //                    sal02pro02uin = product.pro02uin,
        //                    sal02un01uin = unit.un01uin,
        //                    sal02qty = returnDetail.Quantity,
        //                    sal02rate = returnDetail.Rate,
        //                    sal02sub_total = (double)returnDetail.subTotal,
        //                    sal02disc_amt = 0,
        //                    sal02net_amt = (double)returnDetail.netAmt,
        //                    DateCreated = DateTime.Now,
        //                    CreatedName = _ActiveUserName,
        //                };
        //                newReturn.sal02returnItems.Add(currentReturnItem);
        //            }

        //            returnEntities.Add(newReturn);
        //        }

        //        await _MainRepo.InsertBulkAsync(returnEntities);
        //        await _MainRepo.SaveAsync();

        //        return Ok("Sales returns created successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        protected override sal01salesreturn AssignValues(VMSalesReturn data)
        {
            try
            {
                var customerDetail = _customersRepo.GetDetail(data.CustomerId);
                if (customerDetail == null)
                {
                    throw new Exception("Customer not found");
                }

                decimal subTotal = 0;
                if (data.VMSalesReturnItem != null)
                {
                    subTotal = data.VMSalesReturnItem.Sum(item => (decimal)(item.Quantity * item.Rate));
                }

                var returnEntity = new sal01salesreturn
                {
                    sal01cus01uin = data.CustomerId,
                    sal01_salId = data.SaleId,
                    sal01date_nep = "2080-06-06",
                    sal01date_eng = data.Date,
                    sal01invoice_no = data.Invoice_No,
                    sal01remarks = data.Remarks,
                    sal01sub_total = subTotal,
                    sal01disc_amt = 0,
                    sal01total = subTotal,
                    sal01vat_per = 0,
                    sal01vat_amt = 0,
                    sal01net_amt = subTotal,

                    DateCreated = DateTime.Now,
                    CreatedName = _ActiveUserName,
                    sal02itemsreturn = new List<sal02itemsreturn>(),
                };

                if (data.VMSalesReturnItem != null)
                {
                    foreach (var returnItem in data.VMSalesReturnItem)
                    {
                        var product = _productsRepo.GetDetail(returnItem.ProductID);

                        if (product == null)
                        {
                            throw new Exception("Product not found");
                        }

                        var unit = _unitsRepo.GetDetail(returnItem.UnitID);

                        if (unit == null)
                        {
                            throw new Exception("Unit not found");
                        }

                        var returnItemEntity = new sal02itemsreturn
                        {
                            sal02pro02uin = product.pro02uin,
                            sal02un01uin = unit.un01uin,
                            sal02qty = returnItem.Quantity,
                            sal02rate = (decimal)returnItem.Rate,
                            sal02sub_total = (double)returnItem.Quantity * returnItem.Rate,
                            sal02disc_amt = 0,
                            sal02net_amt = (double)returnItem.Quantity * returnItem.Rate,
                            DateCreated = DateTime.Now,
                            CreatedName = _ActiveUserName,
                        };

                        returnEntity.sal02itemsreturn.Add(returnItemEntity);
                    }
                }

                return returnEntity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected override void ReAssignValues(VMSalesReturn data, sal01salesreturn oldData)
        {

            decimal subTotal = 0;
            if (data.VMSalesReturnItem != null)
            {
                subTotal = data.VMSalesReturnItem.Sum(item => (decimal)(item.Quantity * item.Rate));
            }

            try
            {
                // Update sales return header details
                oldData.sal01date_nep = "";
                oldData.sal01date_eng = data.Date;
                oldData.sal01invoice_no = data.Invoice_No;
                oldData.sal01remarks = data.Remarks;
                oldData.sal01sub_total = subTotal;
                oldData.sal01disc_amt = 0;
                oldData.sal01total = subTotal;
                oldData.sal01vat_per = 0;
                oldData.sal01vat_amt = 0;
                oldData.sal01net_amt = subTotal;

                oldData.DateUpdated = DateTime.Now;
                oldData.UpdatedName = _ActiveUserName;

                _saleReturnItemsRepo.LoadSaleReturnItemDetails(oldData);

                if (data.VMSalesReturnItem != null)
                {
                    SoftDeleteNonExisingReturnChild(data, oldData);
                    AddUpdateNewReturnChildRecords(data, oldData);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void AddUpdateNewReturnChildRecords(VMSalesReturn data, sal01salesreturn oldData)
        {
            // Load existing return items
            List<sal02itemsreturn> oldDataItems = oldData.sal02itemsreturn.ToList().GetRange(0, oldData.sal02itemsreturn.Count);

            foreach (var updatedReturnItem in data.VMSalesReturnItem)
            {
                var product = _productsRepo.GetDetail(updatedReturnItem.ProductID);

                if (product == null)
                {
                    throw new Exception("Product ID not found");
                }

                var unit = _unitsRepo.GetDetail(updatedReturnItem.UnitID);

                if (unit == null)
                {
                    throw new Exception("Unit not found");
                }

                // Check if the return item already exists
                var existingDetail = oldDataItems
                    .FirstOrDefault(d => d.sal02uin == updatedReturnItem.ID);

                bool createMode = false;

                if (existingDetail == null)
                {
                    createMode = true;
                    existingDetail = new sal02itemsreturn()
                    {
                        sal02uin = 0,
                    };
                }

                // Update item details
                existingDetail.sal02pro02uin = product.pro02uin;
                existingDetail.sal02un01uin = unit.un01uin;
                existingDetail.sal02qty = updatedReturnItem.Quantity;
                existingDetail.sal02rate = (decimal)updatedReturnItem.Rate;
                existingDetail.sal02sub_total = (double)updatedReturnItem.Quantity * updatedReturnItem.Rate;
                existingDetail.sal02disc_amt = 0;
                existingDetail.sal02net_amt = (double)updatedReturnItem.Quantity * updatedReturnItem.Rate;

                existingDetail.DateUpdated = DateTime.Now;
                existingDetail.UpdatedName = _ActiveUserName;

                if (createMode)
                {
                    oldData.sal02itemsreturn.Add(existingDetail);
                }
            }
        }

        private void SoftDeleteNonExisingReturnChild(VMSalesReturn data, sal01salesreturn oldData)
        {
            // Get existing IDs from the input data
            int[] existingIDs = data
                .VMSalesReturnItem.Where(x => x.ProductID > 0)
                .Select(x => x.ProductID)
                .ToArray();

            // Identify return items to be soft deleted
            var detailsToRemove = oldData.sal02itemsreturn
                .Where(d => !existingIDs.Contains(d.sal02pro02uin))
                .ToList();

            foreach (var detailToRemove in detailsToRemove)
            {
                detailToRemove.DateDeleted = DateTime.UtcNow;
                _saleReturnItemsRepo.Save();
            }
        }


    }
}
