using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantAPI.EventArg;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Models.EntityModels.Inventory;
using POSV1.TenantAPI.Services;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Implementation;
using POSV1.TenantModel.Repo.Interface;
using System.Numerics;

namespace POSV1.TenantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController :
        _AbsCRUDWithDiffInputModelController<ProductsController, IProductsRepo, pro02products, CreateVMProduct, VMProduct, VMProduct, int>
    {
        private ProductEventHandler _productEventHandler { get; set; }
        private readonly IledgerService _ledgerService;
        private readonly IProductCategoriesRepo _productCategoriesRepo;
        private readonly IUnitsRepo _unitsRepo;
        private readonly IProductsRepo _productsRepo;
        private readonly IPurchaseItemsRepo _purchaseItemsRepo;
        private readonly IPurchasereturnitemsRepo _purchaseReturnitemsRepo;
        private readonly ISaleItemsRepo _saleItemsRepo;
        private readonly ISalesItemReturnRepo _salesItemReturnRepo;
        private readonly IProductUnitsRepo _productUnitRepo;
        public ProductsController(
            ILogger<ProductsController> logger,
            IProductsRepo ProductRepo,
            IMapper mapper, 
            ProductEventHandler productEventHandler,
            IledgerService ledgerService,
            IUnitsRepo unitsRepo,
            IProductCategoriesRepo productCategoriesRepo,
            IPurchaseItemsRepo purchaseItemsRepo,
            IPurchasereturnitemsRepo purchasereturnitemsRepo,
            ISaleItemsRepo saleItemsRepo,
            ISalesItemReturnRepo salesItemReturnRepo,
            IProductUnitsRepo productUnitsRepo)
            : base(logger, ProductRepo, mapper)
        {
            _productEventHandler = productEventHandler;
            _ledgerService = ledgerService;
            _productCategoriesRepo = productCategoriesRepo;
            _unitsRepo = unitsRepo;
            _productsRepo = ProductRepo;
            _purchaseItemsRepo = purchaseItemsRepo;
            _purchaseReturnitemsRepo = purchasereturnitemsRepo;
            _saleItemsRepo = saleItemsRepo;
            _salesItemReturnRepo = salesItemReturnRepo;
            _productUnitRepo = productUnitsRepo;
        }

        [HttpGet("GetFilteredList")]
        public virtual async Task<ActionResult<PageResult<VMProduct>>> GetFilteredList(
            string name,
            string categoryName,
            bool? isTaxable,
            bool? enableStock,
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
                                     .Include(x => x.pro01categories)
                                     .Include(x => x.un01units)
                                     .OrderByDescending(x => x.DateCreated)
                                     .AsNoTracking();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(name))
                {
                    query = query.Where(x => x.pro02name_eng.Contains(name));
                }

                if (!string.IsNullOrWhiteSpace(categoryName))
                {
                    query = query.Where(x => x.pro01categories.pro01name_eng.Contains(categoryName));
                }

                if (isTaxable.HasValue)
                {
                    query = query.Where(x => x.pro02is_taxable == isTaxable.Value);
                }

                if (enableStock.HasValue)
                {
                    query = query.Where(x => x.pro02enable_stock == enableStock.Value);
                }

                var totalCount = await query.CountAsync();

                var processedQuery = ProcessListData(query);

                var resultList = await processedQuery
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToListAsync();

                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize.Value);

                var pageResult = new PageResult<VMProduct>
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
                _logger.LogError(ex, "Error occurred while fetching filtered product data.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching filtered product data.");
            }
        }


        [HttpGet("GetDetailView")]
        public virtual async Task<VMProduct> GetDetail(int id)
        {
            var _que = await _MainRepo.GetList()
                .Where(x => x.pro02uin == id)
                .Include(x => x.pro03units)
                    .ThenInclude(x => x.un01units)
                .FirstOrDefaultAsync();  //get the data from db

            var result = ProcessDetailData(_que); // proecss the data as per the view model

            return result;

        }


        protected override IQueryable<VMProduct> ProcessListData(IQueryable<pro02products> data)
        {
            data.Include(x => x.pro01categories)
                .Include(x => x.un01units)
                .Include(x => x.pro03units)
                    .ThenInclude(x => x.un01units);
            return data.Select(product => new VMProduct
            {
                ID = product.pro02uin,
                Name = $"{product.pro02name_eng} ({product.pro02hscode})",
                Code = product.pro02code,
                HsCode = product.pro02hscode,
                Description = product.pro02description,
                CategoryID = product.pro02pro01uin,
                CategoryName = product.pro01categories.pro01name_eng,
                UnitID = product.pro02un01uin ?? product.pro03units
                               .Where(x => x.IsDefault)
                               .Select(x => x.un01units.un01uin)
                               .FirstOrDefault(),

                UnitName = product.pro02un01uin != null
                               ? product.un01units.un01name_eng
                               : product.pro03units
                                   .Where(x => x.IsDefault)
                                   .Select(x => x.un01units.un01name_eng)
                                   .FirstOrDefault(),
                //UnitID = product.pro02un01uin,
                //UnitName = product.un01units.un01name_eng,
                Last_CP = product.pro02last_cp,
                Last_SP = product.pro02last_sp,
                Opening_Qty = product.pro02opening_qty,
                Ledger_Code = product.pro02code,
                Enable_Stock = product.pro02enable_stock,
                Is_Taxable = product.pro02is_taxable,
                Status = product.pro02status,
                HasMultipleUnits = product.pro02hasmultipleunits,
                Units = product.pro02hasmultipleunits == true
                       ? product.pro03units.Select(u => new VMViewUnit
                       {
                           UnitId = u.un01units.un01uin,
                           UnitName = u.un01units.un01name_eng,
                           Ratio = u.pro03ratio,
                           CostPrice = u.pro03last_cp,
                           SalePrice = u.pro03last_sp,
                           IsDefault = u.IsDefault
                       }).ToList()
                       : new List<VMViewUnit>()
            });
        }

        protected override VMProduct ProcessDetailData(pro02products data)
        {
            var categoryName = _productCategoriesRepo.GetDetail(data.pro02pro01uin).pro01name_eng;
            int unitId = data.pro02un01uin ?? 0;
            string unitName = "";
            if (unitId != 0)
            {
                unitName = _unitsRepo.GetDetail(unitId).un01name_eng;
            }
            else
            {
                unitName = data.pro03units
                    .Where(x => x.IsDefault)
                    .Select(x => x.un01units.un01name_eng)
                    .FirstOrDefault();

                unitId = data.pro03units
                    .Where(x => x.IsDefault)
                    .Select(x => x.un01units.un01uin)
                    .FirstOrDefault();
            }

            return new VMProduct
            {
                ID = data.pro02uin,
                Name = data.pro02name_eng,
                Code = data.pro02code,
                HsCode = data.pro02hscode,
                CategoryID = data.pro02pro01uin,
                CategoryName = categoryName,
                Description = data.pro02description,
                UnitID = unitId,
                UnitName = unitName,
                Last_CP = data.pro02last_cp,
                Last_SP = data.pro02last_sp,
                Opening_Qty = data.pro02opening_qty,
                Enable_Stock = data.pro02enable_stock,
                Is_Taxable = data.pro02is_taxable,
                Ledger_Code = data.pro02code,
                Status = data.pro02status,
                HasMultipleUnits = data.pro02hasmultipleunits,
                Units = data.pro02hasmultipleunits == true
                       ? data.pro03units.Select(u => new VMViewUnit
                       {
                           UnitId = u.un01units.un01uin,
                           UnitName = u.un01units.un01name_eng,
                           Ratio = u.pro03ratio,
                           CostPrice = u.pro03last_cp,
                           SalePrice = u.pro03last_sp,
                           IsDefault = u.IsDefault
                       }).ToList()
                       : new List<VMViewUnit>()
            };
        }

        protected override pro02products AssignValues(CreateVMProduct Data)
        {
            var categoryEntity = _productCategoriesRepo.GetDetail(Data.CategoryID);
            if (categoryEntity == null)
            {
                throw new Exception("category not found");
            }

            //var unitEntity = _unitsRepo.GetDetail(Data.UnitID);
            //if (unitEntity == null)
            //{
            //    throw new Exception("Unit not found");
            //}
            var hsCode = _productsRepo.GetList().Where(x => x.pro02hscode == Data.HsCode).Count();
            if (hsCode > 0)
            {
                throw new Exception("Product with similar HS Code already exists !!!");
            }

            if (Data.hasMultipleUnit && (Data.Units == null || !Data.Units.Any()))
            {
                throw new Exception("Units cannot be null !!!");
            }

            var productEntity = new pro02products
            {
                pro02name_eng = Data.Name,
                pro02hscode = Data.HsCode,
                pro02pro01uin = Data.CategoryID,
                pro02status = Data.Status,
                pro02description = Data.Description,
                pro02code = Data.Code,
                pro02un01uin = Data.UnitID,
                pro02is_taxable = Data.Is_Taxable,
                pro02enable_stock = Data.Enable_Stock,
                pro02last_cp = Data.Last_CP,
                pro02last_sp = Data.Last_SP,
                pro02hasmultipleunits = Data.hasMultipleUnit,
                pro02opening_qty = Data.Opening_Qty,
                pro02image_url = "upload/image",

                CreatedName = _ActiveUserName,
                DateCreated = DateTime.Now,
                UpdatedName = " ",
                DateUpdated = DateTime.Now,
                DeletedName = "",
                pro02name_nep = "रुश"
            };

            if (Data.hasMultipleUnit)
            {
                productEntity.pro03units = Data.Units.Select(unit => new pro03units
                {
                    pro03pro02uin = productEntity.pro02uin,
                    pro03un01uin = unit.UnitId,
                    pro03ratio = unit.Ratio,
                    pro03last_cp = unit.CostPrice,
                    pro03last_sp = unit.SalePrice,
                    pro03status = true,
                    IsDefault = unit.IsDefault
                }).ToList();
            }

            //EventManager.OnProductCreated(productEntity);
            _ledgerService.OnProductCreated(productEntity);
            return productEntity;
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] CreateVMProduct data)
        {
            if (data == null)
            {
                return BadRequest("Invalid data.");
            }

            var oldData = await _productsRepo.GetDetailAsync(id);
            if (oldData == null)
            {
                return NotFound("Product not found.");
            }

            await ReAssignValues(data, oldData);

            _productsRepo.Update(oldData);
            await _productsRepo.SaveAsync();
            return Ok(new { message = "Product updated successfully." });
        }

        private async Task ReAssignValues(CreateVMProduct data, pro02products oldData)
        {
            oldData.pro02name_eng = data.Name;
            oldData.pro02hscode = data.HsCode;
            oldData.pro02pro01uin = data.CategoryID;
            oldData.pro02status = data.Status;
            oldData.pro02description = data.Description;
            oldData.pro02code = data.Code;
            oldData.pro02un01uin = data.UnitID;
            oldData.pro02is_taxable = data.Is_Taxable;
            oldData.pro02enable_stock = data.Enable_Stock;
            oldData.pro02last_cp = data.Last_CP;
            oldData.pro02last_sp = data.Last_SP;
            oldData.pro02name_nep = "रुश";

            oldData.DateUpdated = DateTime.Now;
            oldData.UpdatedName = _ActiveUserName;

            if (data.hasMultipleUnit)
            {
                var existingUnits = await _productUnitRepo.GetList()
                    .Where(x => x.pro03pro02uin == oldData.pro02uin)
                    .ToListAsync();

                if (existingUnits.Any())
                {
                    await _productUnitRepo.RemoveRangeAsync(existingUnits);
                }

                oldData.pro03units = data.Units.Select(unit => new pro03units
                {
                    pro03pro02uin = oldData.pro02uin,
                    pro03un01uin = unit.UnitId,
                    pro03ratio = 1,
                    pro03last_cp = unit.CostPrice,
                    pro03last_sp = unit.SalePrice,
                    pro03status = true,
                    IsDefault = unit.IsDefault
                }).ToList();
            }
        }

        //protected override void ReAssignValues(CreateVMProduct Data, pro02products oldData)
        //{
        //    oldData.pro02name_eng = Data.Name;
        //    oldData.pro02hscode = Data.HsCode;
        //    oldData.pro02pro01uin = Data.CategoryID;
        //    oldData.pro02status = Data.Status;
        //    oldData.pro02description = Data.Description;
        //    oldData.pro02code = Data.Code;
        //    oldData.pro02un01uin = Data.UnitID;
        //    oldData.pro02is_taxable = Data.Is_Taxable;
        //    oldData.pro02enable_stock = Data.Enable_Stock;
        //    oldData.pro02last_cp = Data.Last_CP;
        //    oldData.pro02last_sp = Data.Last_SP;
        //    oldData.pro02name_nep = "रुश";

        //    oldData.DateUpdated = DateTime.Now;
        //    oldData.UpdatedName = _ActiveUserName;

        //    if (Data.hasMultipleUnit)
        //    {
        //        var existingUnits = _productUnitRepo.GetList().Where(x => x.pro03pro02uin == oldData.pro02uin).ToList();
        //        foreach (var unit in existingUnits)
        //        {
        //            _productUnitRepo.RemoveRangeAsync(existingUnits);
        //        }

        //        oldData.pro03units = Data.Units.Select(unit => new pro03units
        //        {
        //            pro03pro02uin = oldData.pro02uin,
        //            pro03un01uin = unit.UnitId,
        //            pro03ratio = 1,
        //            pro03last_cp = unit.CostPrice,
        //            pro03last_sp = unit.SalePrice,
        //            pro03status = true,
        //            IsDefault = unit.IsDefault
        //        }).ToList();
        //    }
        //}

        [HttpGet("GetItemTransactions/{itemId}")]
        public async Task<IActionResult> GetItemTransactions(int itemId)
        {
            // Fetch the product details for the given itemId
            var productDetail = await _productsRepo.GetList()
                .FirstOrDefaultAsync(p => p.pro02uin == itemId);

            if (productDetail == null)
            {
                return NotFound($"Product with ID {itemId} not found.");
            }

            // Initialize the summary object
            var summary = new ItemTransactionSummaryViewModel
            {
                ItemId = itemId,
                ItemName = productDetail.pro02name_eng
            };

            // Fetch purchase transactions
            var purchaseItems = await _purchaseItemsRepo
                .GetList()
                .Include(x => x.pur01purchases)
                    .ThenInclude(x => x.ven01vendors)
                .Where(p => p.pur02pro02uin == itemId)
                .ToListAsync();

            summary.TotalPurchaseQuantity = purchaseItems.Sum(p => p.pur02qty);

            summary.Transactions.AddRange(purchaseItems.Select(p => new TransactionDetailViewModel
            {
                ItemId = itemId,
                ItemName = productDetail.pro02name_eng,
                Quantity = p.pur02qty,
                TransactionType = TransactionType.Purchase.ToString(),
                Date = p.DateCreated,
                VendorName = FormatVendorName(p.pur01purchases.ven01vendors)
            }));

            // Fetch purchase return transactions
            var purchaseReturnItems = await _purchaseReturnitemsRepo
                .GetList()
                .Include(x => x.pur01purchasereturns)
                    .ThenInclude(x => x.ven01vendors)
                .Where(pr => pr.pur02returnpro02uin == itemId)
                .ToListAsync();

            summary.TotalPurchaseReturnQuantity = purchaseReturnItems.Sum(pr => pr.pur02returnqty);

            summary.Transactions.AddRange(purchaseReturnItems.Select(pr => new TransactionDetailViewModel
            {
                ItemId = itemId,
                ItemName = productDetail.pro02name_eng,
                Quantity = pr.pur02returnqty,
                TransactionType = TransactionType.PurchaseReturn.ToString(),
                Date = pr.DateCreated,
                VendorName = FormatVendorName(pr.pur01purchasereturns.ven01vendors)
            }));

            // Fetch sale transactions
            var saleItems = await _saleItemsRepo
                .GetList()
                .Include(x => x.sal01sales)
                    .ThenInclude(x => x.cus01customers)
                .Where(s => s.sal02pro02uin == itemId)
                .ToListAsync();

            summary.TotalSalesQuantity = (decimal)saleItems.Sum(s => s.sal02qty);

            summary.Transactions.AddRange(saleItems.Select(s => new TransactionDetailViewModel
            {
                ItemId = itemId,
                ItemName = productDetail.pro02name_eng,
                Quantity = (decimal)s.sal02qty,
                TransactionType = TransactionType.Sale.ToString(),
                Date = s.DateCreated,
                CustomerName = FormatCustomerName(s.sal01sales.cus01customers)
            }));

            // Fetch sale return transactions
            var saleReturnItems = await _salesItemReturnRepo
                .GetList()
                .Include(x => x.sal01salesreturn)
                    .ThenInclude(x => x.cus01customers)
                .Where(sr => sr.sal02pro02uin == itemId)
                .ToListAsync();

            summary.TotalSalesReturnQuantity = (decimal)saleReturnItems.Sum(sr => sr.sal02qty);

            summary.Transactions.AddRange(saleReturnItems.Select(sr => new TransactionDetailViewModel
            {
                ItemId = itemId,
                ItemName = productDetail.pro02name_eng,
                Quantity = (decimal)sr.sal02qty,
                TransactionType = TransactionType.SaleReturn.ToString(),
                Date = sr.DateCreated,
                CustomerName = FormatCustomerName(sr.sal01salesreturn.cus01customers)
            }));

            // Return the summary as the response
            return Ok(summary);
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
    }
}