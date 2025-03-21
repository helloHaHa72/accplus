using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Services;
using POSV1.TenantAPI.Utility;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController :
        _AbsCRUDWithDiffInputModelController<CustomersController, ICustomersRepo, cus01customers, VMCustomer, VMViewCustomer, VMViewCustomer, int>
    {
        private readonly IMapper _mapper;
        private readonly IledgerService _ledgerService;
        private readonly ICustomersRepo _customersRepo;
        private readonly IHttpContextAccessor _contextAccessor;
        public CustomersController(
            ILogger<CustomersController> logger,
            ICustomersRepo customersRepo,
            IMapper mapper,
            IledgerService ledgerService,
            IHttpContextAccessor contextAccessor)
           : base(logger, customersRepo, mapper)
        {
            _mapper = mapper;
            _ledgerService = ledgerService;
            _customersRepo = customersRepo;
            _contextAccessor = contextAccessor;
        }

        [HttpGet("GetFilteredList")]
        public virtual async Task<ActionResult<PageResult<VMViewCustomer>>> GetFilteredList(
            string name = null,
            string customerTypeName = null,
            bool? status = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string telephoneNo = null,
            string address = null,
            string ledgerCode = null,
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
                                     .Include(x => x.CustomerType)
                                     .OrderByDescending(x => x.DateCreated)
                                     .AsNoTracking();

                // Apply filters dynamically
                query = query.Where(x =>
                    (string.IsNullOrEmpty(name) || x.cus01name_eng.Contains(name)) &&
                    (string.IsNullOrEmpty(customerTypeName) || x.CustomerType.cus02Name.Contains(customerTypeName)) &&
                    (!status.HasValue || x.cus01status == status.Value) &&
                    (!fromDate.HasValue || x.cus01registered_date >= fromDate.Value) &&
                    (!toDate.HasValue || x.cus01registered_date <= toDate.Value) &&
                    (string.IsNullOrEmpty(telephoneNo) || x.cus01tel.Contains(telephoneNo)) &&
                    (string.IsNullOrEmpty(address) || x.cus01address.Contains(address)) &&
                    (string.IsNullOrEmpty(ledgerCode) || x.cus01led_code.Contains(ledgerCode))
                );

                var totalCount = await query.CountAsync();

                var processedQuery = ProcessListData(query);

                var resultList = await processedQuery
                    .OrderByDescending(x => x.Registered_Date)
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToListAsync();

                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize.Value);

                var pageResult = new PageResult<VMViewCustomer>
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
                _logger.LogError(ex, "Error occurred while fetching filtered customer data.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching filtered customer data.");
            }
        }



        protected override IQueryable<VMViewCustomer> ProcessListData(IQueryable<cus01customers> data)
        {
            return data.Select(customer => new VMViewCustomer
            {
                ID = customer.cus01uin,
                //Name = customer.cus01name_eng + (customer.cus01led_code),
                Name = $"{customer.cus01name_eng} ({customer.cus01led_code})",
                Address = customer.cus01address,
                TelePhone_No = customer.cus01tel,
                OpeningBalance = customer.cus01opening_bal,
                Registration_No = customer.cus01reg_no,
                Registered_Date = customer.cus01registered_date,
                Status = customer.cus01status,
                Ledger_code = customer.cus01led_code,
                CustomerType = FetchCustomerType(customer)
            });
        }

        private static VMCustomerTypeto FetchCustomerType(cus01customers customer)
        {
            return customer.CustomerType != null ? new VMCustomerTypeto
            {
                Id = (int)customer.cus01customerTypeId,
                Title = customer.CustomerType.cus02Name,
                DiscountPercentage = customer.CustomerType.cus02DiscountPercenatge
            } : new VMCustomerTypeto();
        }

        protected override VMViewCustomer ProcessDetailData(cus01customers data)
        {
            return new VMViewCustomer
            {
                ID = data.cus01uin,
                Name = data.cus01name_eng,
                Address = data.cus01address,
                TelePhone_No = data.cus01tel,
                Ledger_code = data.cus01led_code,
                OpeningBalance = data.cus01opening_bal,
                Registration_No = data.cus01reg_no,
                Registered_Date = data.cus01registered_date,
                CustomerType = FetchCustomerType(data),
                Status = data.cus01status
            };
        }

        protected override cus01customers AssignValues(VMCustomer Data)
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

                var preData = _customersRepo.GetList()
                    .Where(x => x.cus01led_code == ledgerCode)
                    .FirstOrDefault();

                if (preData == null)
                {
                    isUnique = true;
                }
            }

            var customerEntity = new cus01customers
            {
                cus01name_eng = Data.Name,
                cus01address = Data.Address,
                cus01tel = Data.TelePhone_No,
                cus01opening_bal = Data.OpeningBalance,
                cus01reg_no = Data.Registration_No,
                cus01registered_date = Data.Registered_Date,
                cus01status = Data.Status,
                cus01led_code = ledgerCode,
                cus01customerTypeId = Data.CustomerTypeId,
                cus01isvat = Data.IsVat,
                BranchCode = branchCode,
                //cus010number = Data.Number,

                cus01deleted = false,
                CreatedName = _ActiveUserName,
                DateCreated = DateTime.Now,
                UpdatedName = " ",
                DateUpdated = DateTime.Now,
                DeletedName = "",

                cus01name_nep = "रुश"
            };
            _ledgerService.OnCustomerCreated(customerEntity);
            return customerEntity;
        }

        protected override void ReAssignValues(VMCustomer Data, cus01customers oldData)
        {
            oldData.cus01name_eng = Data.Name;
            oldData.cus01address = Data.Address;
            oldData.cus01tel = Data.TelePhone_No;
            oldData.cus01opening_bal = Data.OpeningBalance;
            oldData.cus01reg_no = Data.Registration_No;
            oldData.cus01status = Data.Status;
            oldData.cus01registered_date = Data.Registered_Date;
            oldData.cus01customerTypeId = Data.CustomerTypeId;

            oldData.cus01deleted = false;
            oldData.cus01name_nep = "रुश";

            oldData.DateUpdated = DateTime.Now;
            oldData.UpdatedName = _ActiveUserName;
        }

    }
}
