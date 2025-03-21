using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Models.EntityModels.Production;
using POSV1.TenantAPI.Utility;
using POSV1.TenantModel;
using POSV1.TenantModel.Models.EntityModels.Production;
using POSV1.TenantModel.Repo.Interface;
using POSV1.TenantModel.Repo.Interface.Production;
using System.Security.Claims;

namespace POSV1.TenantAPI.Controllers.Production
{
    [Route("api/ProductionLine")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductionListController : ControllerBase
    {
        private readonly ILogger<ProductionListController> _logger;
        private readonly IProductionRepo _productionRepo;
        private readonly IConsumeRawProductRepo _consumeRawProductRepo;
        private readonly IProductsRepo _productsRepo;
        private readonly IUnitsRepo _unitsRepo;

        public ClaimsPrincipal _ActiveUser => HttpContext.User;
        public string _ActiveUserName => _ActiveUser.Identity == null ? "Admin" : _ActiveUser.Identity.Name;
        public ProductionListController(ILogger<ProductionListController> logger,
            IProductionRepo productionRepo,
            IConsumeRawProductRepo consumeRawProductRepo,
            IProductsRepo productsRepo,
            IUnitsRepo unitsRepo)
        {
            _logger = logger;
            _productionRepo = productionRepo;
            _consumeRawProductRepo = consumeRawProductRepo;
            _productsRepo = productsRepo;
            _unitsRepo = unitsRepo;
        }

        [HttpPost]
        public async Task<IActionResult> Create(VMProductionDto data)
        {
            try
            {
                var validProductIds = _productsRepo.GetList().Select(p => p.pro02uin).ToHashSet();

                var insertData = new prod01production()
                {
                    prod01title = data.Title,
                    prod01code = GeneralUtility.GenerateProductionCode(data.Title),
                    prod01description = data.Description,
                    prod01startdate = data.StartDate,
                    prod01enddate = data.EndDate,
                    prod01status = TenantModel.EnumProductionStatus.NotStarted,
                    RawProducts = new List<prod02consumerawproduct>(),
                    CreatedName = _ActiveUserName,
                    DateCreated = DateTime.UtcNow,
                };

                foreach (var rawProduct in data.RawProducts)
                {
                    if (!validProductIds.Contains(rawProduct.ProductId))
                    {
                        return BadRequest($"Invalid ProductId: {rawProduct.ProductId}");
                    }

                    insertData.RawProducts.Add(new prod02consumerawproduct
                    {
                        prod02productuin = rawProduct.ProductId,
                        prod2productname = rawProduct.ProductName,
                        prod02unituin = rawProduct.UnitId,
                        prod02unitname = rawProduct.UnitName,
                        prod02rate = rawProduct.Rate,
                        prod02qty = rawProduct.Qty,
                        prod02isallused = false,
                        prod02remainingqty = 0
                    });
                }

                _productionRepo.Insert(insertData);
                await _productionRepo.SaveAsync();

                return Ok("Data inserted !!!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating production");

                var errorMessage = $"Internal Server Error: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $" | Inner Exception: {ex.InnerException.Message}";
                    if (ex.InnerException.InnerException != null)
                    {
                        errorMessage += $" | Inner-Inner Exception: {ex.InnerException.InnerException.Message}";
                    }
                }
                errorMessage += $" | StackTrace: {ex.StackTrace}";

                return StatusCode(500, errorMessage);
            }
        }

        [HttpPut, Route("AddFinalProductDetail")]
        public async Task<IActionResult> AddFinalProductDetail(int ProductionId, VMFinalProduct data)
        {
            try
            {
                var production = await _productionRepo.GetList()
                    .Include(x => x.RawProducts)
                    .Include(x => x.FinalProducts)
                    .Where(x => x.prod01uin == ProductionId)
                    .FirstOrDefaultAsync();

                if (production.prod01status == EnumProductionStatus.Completed)
                {
                    throw new Exception("The ststus is completed. Data cannot be update now !!!");
                }

                if (production == null)
                    return NotFound("Production not found");

                var productData = await _productsRepo.GetDetailAsync(data.ProductId);

                var unitData = await _unitsRepo.GetDetailAsync(data.UnitId);

                // Update raw products
                //await _consumeRawProductRepo.RemoveRangeAsync(production.RawProducts);

                //foreach (var rawProduct in data.RawProducts)
                //{
                production.FinalProducts.Add(new prod04finalproducts
                {
                    prod4productionuin = production.prod01uin,
                    prod04productuin = data.ProductId,
                    prod04productname = productData.pro02name_eng,
                    prod04unituin = data.UnitId,
                    prod04unitname = unitData.un01name_eng,
                    prod04unitratio = (decimal)unitData.un01ratio,
                    prod04desc = data.Description,
                    prod04date = data.Date,
                    prod04qty = data.Qty,
                    prod04remarks = data.Remarks
                });
                //}

                _productionRepo.Update(production);
                await _productionRepo.SaveAsync();

                return Ok("Production updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating production");
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var productions = await _productionRepo
                    .GetList()
                    .Include(x => x.RawProducts)
                    .Include(x => x.FinalProducts)
                    .OrderByDescending(x => x.DateCreated)
                    .ToListAsync();

                var productionList = productions.Select(p => new ViewProductionDto
                {
                    Id = p.prod01uin,
                    Title = p.prod01title,
                    Code = p.prod01code,
                    Status = p.prod01status.ToString(),
                    Description = p.prod01description,
                    StartDate = p.prod01startdate.Date,
                    EndDate = p.prod01enddate?.Date,
                    RawProducts = p.RawProducts.Select(rp => new ViewConsumerRawProduct
                    {
                        Id = rp.prod02uin,
                        ProductId = rp.prod02productuin,
                        ProductName = rp.prod2productname,
                        UnitId = rp.prod02unituin,
                        UnitName = rp.prod02unitname,
                        Rate = rp.prod02rate,
                        Qty = rp.prod02qty,
                        RemainingQty = rp.prod02remainingqty
                    }).ToList(),
                    FinalProducts = p.FinalProducts.Select(fp => new ViewFinalProduct
                    {
                        Id = fp.prod04uin,
                        ProductId = fp.prod04productuin,
                        ProductName = fp.prod04productname,
                        UnitId = fp.prod04unituin,
                        UnitName = fp.prod04unitname,
                        Ratio = fp.prod04unitratio,
                        Description = fp.prod04desc,
                        Qty = fp.prod04qty,
                        Date = fp.prod04date.Date,
                        Remarks = fp.prod04remarks
                    }).ToList()
                }).ToList();

                return Ok(productionList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching production list");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var production = await _productionRepo.GetList()
                    .Where(x => x.prod01uin == id)
                    .Include(x => x.RawProducts)
                    .Include(x => x.FinalProducts)
                    .FirstOrDefaultAsync();

                if (production == null)
                    return NotFound("Production not found");

                var detailDto = new ViewProductionDto
                {
                    Id = production.prod01uin,
                    Title = production.prod01title,
                    Code = production.prod01code,
                    Status= production.prod01status.ToString(),
                    Description = production.prod01description,
                    StartDate = production.prod01startdate.Date,
                    EndDate = production.prod01enddate?.Date,
                    RawProducts = production.RawProducts.Select(rp => new ViewConsumerRawProduct
                    {
                        Id = rp.prod02uin,
                        ProductId = rp.prod02productuin,
                        ProductName = rp.prod2productname,
                        UnitId = rp.prod02unituin,
                        UnitName = rp.prod02unitname,
                        Rate = rp.prod02rate,
                        Qty = rp.prod02qty,
                        RemainingQty = rp.prod02remainingqty
                    }).ToList(),
                    FinalProducts = production.FinalProducts.Select(fp => new ViewFinalProduct
                    {
                        Id = fp.prod04uin,
                        ProductId = fp.prod04productuin,
                        ProductName = fp.prod04productname,
                        UnitId = fp.prod04unituin,
                        UnitName = fp.prod04unitname,
                        Ratio = fp.prod04unitratio,
                        Description = fp.prod04desc,
                        Qty = fp.prod04qty,
                        Date = fp.prod04date.Date,
                        Remarks = fp.prod04remarks
                    }).ToList()
                };

                return Ok(detailDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching production details");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VMProductionDto data)
        {
            try
            {
                var production = await _productionRepo.GetList()
                    .Include(x => x.RawProducts)
                    .Where(x => x.prod01uin == id)
                    .FirstOrDefaultAsync();

                if(production.prod01status == EnumProductionStatus.Completed)
                {
                    throw new Exception("The ststus is completed. Data cannot be update now !!!");
                }

                if (production == null)
                    return NotFound("Production not found");

                production.prod01title = data.Title;
                production.prod01description = data.Description;
                production.prod01startdate = data.StartDate;
                production.prod01enddate = data.EndDate;
                production.UpdatedName = _ActiveUserName;
                production.DateUpdated = DateTime.UtcNow;

                // Update raw products
                await _consumeRawProductRepo.RemoveRangeAsync(production.RawProducts);

                foreach (var rawProduct in data.RawProducts)
                {
                    production.RawProducts.Add(new prod02consumerawproduct
                    {
                        prod02productuin = rawProduct.ProductId,
                        prod2productname = rawProduct.ProductName,
                        prod02unituin = rawProduct.UnitId,
                        prod02unitname = rawProduct.UnitName,
                        prod02rate = rawProduct.Rate,
                        prod02qty = rawProduct.Qty,
                        prod02remainingqty = 0,
                        prod02isallused = true
                    });
                }

                _productionRepo.Update(production);
                await _productionRepo.SaveAsync();

                return Ok("Production updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating production");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, EnumProductionStatus status)
        {
            try
            {
                var production = await _productionRepo.GetList()
                    .Include(x => x.RawProducts)
                    .Where(x => x.prod01uin == id)
                    .FirstOrDefaultAsync();

                if (production == null)
                    return NotFound("Production not found");

                var statusLog = new prod03statuslog
                {
                    prod03previousstatus = production.prod01status,
                    prod03newstatus = status,
                    prod03remarks = $"Status changed from {production.prod01status} to {status} by {_ActiveUserName}",
                    CreatedName = _ActiveUserName,
                    DateCreated = DateTime.UtcNow,
                    prod03productionuin = production.prod01uin
                };

                production.prod01status = status;
                production.UpdatedName = _ActiveUserName;
                production.DateUpdated = DateTime.UtcNow;

                production.StatusLogs.Add(statusLog);

                _productionRepo.Update(production);
                await _productionRepo.SaveAsync();

                return Ok("Production updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating production");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var production = await _productionRepo.GetDetailAsync(id);
                if (production == null)
                    return NotFound("Production not found");

                production.DeletedName = _ActiveUserName;
                production.DateDeleted = DateTime.UtcNow;

                _productionRepo.Update(production);
                await _productionRepo.SaveAsync();

                return Ok("Production deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting production");
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
