using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using POSV1.TenantModel.Repo.Interface.Estimate;
using POSV1.TenantModel.Models.EntityModels.Estimate;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POSV1.TenantModel.Repo.Interface;
using POSV1.TenantModel;
using System.Security.Claims;
using POSV1.TenantAPI.Models.EntityModels;
using POSV1.TenantAPI.Utility;
using Microsoft.EntityFrameworkCore;

namespace POSV1.TenantAPI.Controllers.Estimate
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstimateController : ControllerBase
    {
        private readonly ILogger<EstimateController> _logger;
        private readonly IEstimateRepo _estimateRepo;
        private readonly ICustomersRepo _customersRepo;
        private readonly IProductsRepo _productsRepo;
        private readonly IUnitsRepo _unitsRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsPrincipal _ActiveUser => HttpContext.User;
        public string _ActiveUserName => _ActiveUser.Identity == null ? "Admin" : _ActiveUser.Identity.Name;

        public EstimateController(ILogger<EstimateController> logger,
            IEstimateRepo estimateRepo,
            ICustomersRepo customersRepo,
            IProductsRepo productsRepo,
            IUnitsRepo unitsRepo,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _estimateRepo = estimateRepo;
            _customersRepo = customersRepo;
            _productsRepo = productsRepo;
            _unitsRepo = unitsRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var data = await _estimateRepo.GetList()
                .Select(e => new EstimateList
                {
                    Id = e.est01uin,
                    CustomerName = e.est01customername, // Assuming 'Customer' is a navigation property
                    RefNumber = e.est01refnumber,
                    CreatedBy = e.CreatedName,
                    CreatedDate = e.DateCreated.Date
                })
                .ToListAsync();

            return Ok(data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            try
            {
                var estimate = await _estimateRepo.GetList()
                    .Include(x => x.est02estimatesales)
                        .ThenInclude(x => x.est02estimatesalesitems)
                    .Where(x => x.est01uin == id)
                    .FirstOrDefaultAsync();

                if (estimate == null)
                {
                    return NotFound("Estimate not found");
                }

                var estimateDetail = new EstimateDetailDto
                {
                    CustomerId = estimate.est02estimatesales.FirstOrDefault()?.est02cus01uin,
                    CustomerName = estimate.est01customername,
                    RefNumber = estimate.est01refnumber,
                    Date = estimate.est02estimatesales.FirstOrDefault()?.est02date_eng ?? DateTime.MinValue,
                    Remarks = estimate.est02estimatesales.FirstOrDefault()?.est02remarks,
                    SubTotal = estimate.est02estimatesales.FirstOrDefault()?.est02sub_total ?? 0,
                    DiscountAmount = estimate.est02estimatesales.FirstOrDefault()?.est02disc_amt ?? 0,
                    DiscountPercentage = estimate.est02estimatesales.FirstOrDefault()?.est02disc_percentage ?? 0,
                    Total = estimate.est02estimatesales.FirstOrDefault()?.est02total ?? 0,
                    EstimateItems = estimate.est02estimatesales
                        .SelectMany(es => es.est02estimatesalesitems)
                        .Select(item => new EstimateItemDto
                        {
                            ProductID = item.est02pro02uin,
                            UnitID = item.est02un01uin,
                            Quantity = item.est02qty,
                            Rate = item.est02rate,
                            SubTotal = (decimal)item.est02sub_total,
                            DiscountAmount = item.est02disc_amt,
                            NetAmount = (decimal)item.est02net_amt,
                            DriverId = item.est02emp01uin,
                            VehicleId = item.est02vec02uin,
                            Destination = item.est02destination,
                            ChalanNumber = item.est02ref_no,
                            TransportationFee = item.est02transportationfee ?? 0,
                            IsVatApplied = item.est02vatper.HasValue
                        })
                        .ToList()
                };

                return Ok(estimateDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving the estimate");
                return StatusCode(500, ex.Message);
            }
        }



        [HttpPost("create")]
        public async Task<IActionResult> CreateEstimate([FromBody] VMEestimate data)
        {
            var branchCode = _httpContextAccessor.HttpContext.User.FindFirst("BranchCode")?.Value;

            // Check if BranchCode exists
            if (string.IsNullOrEmpty(branchCode))
            {
                return BadRequest("BranchCode is missing or invalid in the token");
            }

            try
            {
                var customerDetail = new cus01customers();
                if (data.CustomerId.HasValue)
                {
                    customerDetail = await _customersRepo.GetDetailAsync(data.CustomerId.Value);
                }

                if (data.Disc_Percentage != 0)
                {
                    data.Disc_Amt = (data.Sub_Total * data.Disc_Percentage) / 100;
                }

                decimal vatPercentage = GetVatData();

                decimal vatApplicableTotal = data.VMEestimateItem
                    ?.Where(item => item.IsVatApplied)
                    .Sum(item => item.Net_Amt) ?? 0;

                vatApplicableTotal -= (decimal)data.Disc_Amt;

                decimal vatAmount = ((decimal)vatApplicableTotal * vatPercentage) / 100;

                var estimateEntity = new est01estimate
                {
                    est01customername = customerDetail?.cus01name_eng,
                    est01refnumber = GeneralUtility.GenerateReferenceNumber(),
                    est01status = EnumEstimateStatus.Created, // Or based on your business logic
                    DateCreated = DateTime.Now,
                    CreatedName = _ActiveUserName,
                    est02estimatesales = new List<est02estimatesales>()
                };

                var estimateSalesEntity = new est02estimatesales
                {
                    est02cus01uin = data.CustomerId,
                    est02date_nep = data.Date.ToString(),
                    est02date_eng = data.Date,
                    est02invoice_no = "00000",
                    est02remarks = data.Remarks,
                    est02sub_total = data.Sub_Total,
                    est02disc_amt = data.Disc_Amt,
                    est02disc_percentage = data.Disc_Percentage,
                    est02total = data.Total,
                    est02status = true,
                    DateCreated = DateTime.Now,
                    CreatedName = _ActiveUserName,
                    est02estimatesalesitems = new List<est02estimatesalesitems>()
                };

                if (data.VMEestimateItem != null)
                {
                    foreach (var saleItem in data.VMEestimateItem)
                    {
                        var product = await _productsRepo.GetDetailAsync(saleItem.ProductID);

                        if (product == null)
                        {
                            return NotFound("Product not found");
                        }

                        var unit = await _unitsRepo.GetDetailAsync(saleItem.UnitID);

                        if (unit == null)
                        {
                            return NotFound("Unit not found");
                        }

                        var salesItemEntity = new est02estimatesalesitems
                        {
                            est02pro02uin = product.pro02uin,
                            est02estimatesalesuin = estimateSalesEntity.est02uin,
                            est02un01uin = unit.un01uin,
                            est02qty = saleItem.Quantity,
                            est02rate = saleItem.Rate,
                            est02sub_total = (double)saleItem.Sub_Total,
                            est02disc_amt = saleItem.Disc_Amt,
                            est02net_amt = (double)saleItem.Net_Amt,
                            est02emp01uin = saleItem.DriverId,
                            est02vec02uin = saleItem.VechileId,
                            est02destination = saleItem.Destination,
                            est02ref_no = saleItem.Chalan_No,
                            est02transportationfee = saleItem.Transportation_Fee,
                            est02vatper = saleItem.IsVatApplied ? (double?)vatPercentage : null,
                            DateCreated = DateTime.Now,
                            CreatedName = _ActiveUserName,
                        };

                        estimateSalesEntity.est02estimatesalesitems.Add(salesItemEntity);
                    }
                }

                estimateEntity.est02estimatesales.Add(estimateSalesEntity);

                _estimateRepo.Insert(estimateEntity);
                await _estimateRepo.SaveAsync();

                return Ok("Data inserted !!!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating the estimate");
                return StatusCode(500, ex.Message);
            }
        }

        private decimal GetVatData()
        {
            // Fetch VAT percentage from settings or configuration
            return 13; // Placeholder for the actual logic
        }
    }
}
