using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Services;
using POSV1.TenantAPI.Utility;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Implementation;
using POSV1.TenantModel.Repo.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace POSV1.TenantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController :
        _AbsCRUDWithDiffInputModelController<
            VendorsController,
            IVendorRepo,
            ven01vendors,
            CreateVMVendor,
            VMVendor,
            VMVendor,
            int>
    {
        private readonly IMapper _mapper;
        private readonly IledgerService _ledgerService;
        private readonly IPurchaseRepo _purchaseRepo;
        private readonly IVendorRepo _vendorRepo;
        private readonly IHttpContextAccessor _contextAccessor;
        public VendorsController(
            ILogger<VendorsController> logger,
            IVendorRepo vendorRepo,
            IMapper mapper,
            IledgerService ledgerService,
            IPurchaseRepo purchaseRepo,
            IHttpContextAccessor contextAccessor)
           : base(logger, vendorRepo, mapper)
        {
            _ledgerService = ledgerService;
            _purchaseRepo = purchaseRepo;
            _vendorRepo = vendorRepo;
            _contextAccessor = contextAccessor;
        }

        [HttpGet("GetFilteredList")]
        public virtual async Task<ActionResult<PageResult<VMVendor>>> GetFilteredList(
            string name = null,
            string address = null,
            string ledgerCode = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            int? pageNumber = 1,
            int? pageSize = 10)
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
                    .OrderByDescending(x => x.DateCreated)
                    .AsNoTracking();

                // Apply filters dynamically
                query = query.Where(x =>
                    (string.IsNullOrEmpty(name) || x.ven01name_eng.Contains(name)) &&
                    (string.IsNullOrEmpty(address) || x.ven01address.Contains(address)) &&
                    (string.IsNullOrEmpty(ledgerCode) || x.ven01led_code.Contains(ledgerCode)) &&
                    (!fromDate.HasValue || x.ven01registered_date >= fromDate.Value) &&
                    (!toDate.HasValue || x.ven01registered_date <= toDate.Value)
                );

                var totalCount = await query.CountAsync();

                var processedQuery = ProcessListData(query);

                var resultList = await processedQuery // Order before pagination
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToListAsync();

                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize.Value);

                var pageResult = new PageResult<VMVendor>
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
                _logger.LogError(ex, "Error occurred while fetching filtered vendor data.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching filtered vendor data.");
            }
        }



        protected override VMVendor ProcessDetailData(ven01vendors data)
        {
            return new VMVendor
            {
                ID = data.ven01uin,
                Name = data.ven01name_eng,
                Address = data.ven01address,
                Tel_No = data.ven01tel,
                Ledger_code = data.ven01led_code,
                Opening_Balance = data.ven01opening_bal,
                Registration_No = data.ven01reg_no,
                Registered_Date = data.ven01registered_date
            };
        }

        [HttpGet("VendorWithTxnDetail")]
        public async Task<IActionResult> VendorWithTxnDetail(int ven_id)
        {
            try
            {
                var vendorDetail = await _MainRepo.GetDetailAsync(ven_id);

                if (vendorDetail == null)
                {
                    return NotFound("Vendor not found");
                }

                List<VMVenPurchase> purchaseRecord = GetPurchaseList(ven_id);

                var vmVendorWithTxnDetails = new VMVendorWithTxnDetails
                {
                    ID = vendorDetail.ven01uin,
                    Name = vendorDetail.ven01name_eng,
                    Address = vendorDetail.ven01address,
                    Tel_No = vendorDetail.ven01tel,
                    Ledger_code = vendorDetail.ven01led_code,
                    Opening_Balance = vendorDetail.ven01opening_bal,
                    Registration_No = vendorDetail.ven01reg_no,
                    Registered_Date = vendorDetail.ven01registered_date,
                    Purchases = purchaseRecord,
                };

                return Ok(vmVendorWithTxnDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private List<VMVenPurchase> GetPurchaseList(int vendorId)
        {
            var logEntity = _purchaseRepo.GetList()
              .Where(x => x.pur01ven01uin == vendorId)
              .Include(x => x.ven01vendors)
                .ThenInclude(x => x.pur01purchases)
                    .ThenInclude(x => x.pur02items)
                        .ThenInclude(x => x.pro02products)
              .Select(x => new VMVenPurchase()
              {
                  Id = x.pur01uin,
                  _date = x.pur01date,
                  VendorId = x.ven01vendors.ven01uin,
                  VendorName = x.ven01vendors.ven01name_eng,
                  Invoice_No = x.pur01invoice_no,
                  Net_Amt = x.pur01net_amt,
                  Remarks = x.pur01remarks,
                  Products = string.Join(", ", x.pur02items
                                .Select(i => i.pro02products.pro02name_eng)
                                .Distinct())
              }).ToList();

            return logEntity;
        }

        protected override IQueryable<VMVendor> ProcessListData(IQueryable<ven01vendors> data)
        {
            return data.Select(vendor => new VMVendor
            {
                ID = vendor.ven01uin,
                //Name = vendor.ven01name_eng,
                Name = $"{vendor.ven01name_eng} ({vendor.ven01led_code})",
                Address = vendor.ven01address,
                Tel_No = vendor.ven01tel,
                Opening_Balance = vendor.ven01opening_bal,
                Registration_No = vendor.ven01reg_no,
                Registered_Date = vendor.ven01registered_date,
                Ledger_code = vendor.ven01led_code,
            });
        }

        protected override ven01vendors AssignValues(CreateVMVendor Data)
        {
            var branchCode = _contextAccessor.HttpContext.User.FindFirst("BranchCode")?.Value;

            // Check if BranchCode exists
            if (string.IsNullOrEmpty(branchCode))
            {
                throw new Exception("BranchCode is missing or invalid in the token");
            }

            string ledgerCode = "";
            bool isUnique = false;

            while (!isUnique)
            {
                ledgerCode = GeneralUtility.GenerateRandomSixCharacterCode();

                var preData = _vendorRepo.GetList()
                    .Where(x => x.ven01led_code == ledgerCode)
                    .FirstOrDefault();

                if (preData == null)
                {
                    isUnique = true;
                }
            }

            var vendorEntity = new ven01vendors
            {
                ven01name_eng = Data.Name,
                ven01address = Data.Address,
                ven01tel = Data.Tel_No,
                ven01opening_bal = Data.Opening_Balance,
                ven01reg_no = Data.Registration_No,
                ven01registered_date = Data.Registered_Date,
                ven01led_code = ledgerCode,
                ven01isvat = Data.IsVat,
                BranchCode = branchCode,
                //ven010number = Data.Number,

                ven01is_deleted = false,
                CreatedName = _ActiveUserName,
                DateCreated = DateTime.Now,
                UpdatedName = " ",
                DateUpdated = DateTime.Now,
                DeletedName = "",
                ven01name_nep = "रुश"
            };
            _ledgerService.OnVendorCreated(vendorEntity);
            return vendorEntity;
        }

        protected override void ReAssignValues(CreateVMVendor Data, ven01vendors oldData)
        {
            oldData.ven01name_eng = Data.Name;
            oldData.ven01address = Data.Address;
            oldData.ven01tel = Data.Tel_No;
            oldData.ven01opening_bal = Data.Opening_Balance;
            oldData.ven01reg_no = Data.Registration_No;
            oldData.ven01registered_date = Data.Registered_Date;

            oldData.DateUpdated = DateTime.Now;
            oldData.UpdatedName = _ActiveUserName;
            oldData.ven01is_deleted = false;
            oldData.ven01name_nep = "रुश";
        }
    }
}
