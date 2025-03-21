using AutoMapper;
using BaseAppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Services;
using POSV1.TenantModel;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Repo.Interface;
using RepoBaseModelCore;
using System.Security.Claims;

namespace POSV1.TenantAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public abstract class _AbsVoucherCRUDController<ChildController, MainRepo, MainEntity, ReqModel, ListVMModel, DetailVMModel, PKType>
          : ControllerBase
          where ChildController : ControllerBase
          where MainRepo : IGeneralRepositories<MainEntity, PKType>
          where MainEntity : Auditable
          where ReqModel : class
          where ListVMModel : class
          where DetailVMModel : class
    {
        protected readonly ILogger<ChildController> _logger;
        protected readonly MainRepo _MainRepo;
        private readonly IMapper _mapper;
        private readonly IVoucherService _voucherService;
        private readonly IConfigurationSettings _configurationSettings;
        private readonly MainDbContext _context;

        #region authUser
        public ClaimsPrincipal _ActiveUser => HttpContext.User;
        public string _ActiveUserName => _ActiveUser.Identity == null ? "Admin" : _ActiveUser.Identity.Name;
        #endregion

        public delegate bool VoucherStatusChangedEventHandler(vou02voucher_summary Data);

        public event VoucherStatusChangedEventHandler VoucherApproved;
        public event VoucherStatusChangedEventHandler VoucherRejected;
        public event VoucherStatusChangedEventHandler VoucherUnApproved;
        public _AbsVoucherCRUDController(
            ILogger<ChildController> log,
            MainRepo repo,
            IMapper mapper,
            MainDbContext context,
            IVoucherService voucherService,
            IConfigurationSettings configurationSettings)
        {
            _logger = log;
            _MainRepo = repo;
            _mapper = mapper;
            _context = context;
            VoucherApproved = OnVoucherApproved;
            VoucherRejected = OnVoucherRejected;
            VoucherUnApproved = OnVoucherUnApproved;
            _voucherService = voucherService;
            _configurationSettings = configurationSettings;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public string GenerateVoucherNumber(string prefix, string branchPrefix, int sn)
        {
            string paddedSn = sn.ToString().PadLeft(4, '0');

            return $"{prefix}-{branchPrefix}-{paddedSn}";
        }

        [HttpGet]
        public int GetNextSerialNumber()
        {
            var lastVoucher = _context.vou02voucher_summary.OrderByDescending(v => v.DateCreated).FirstOrDefault();
            if (lastVoucher == null)
            {
                return 1;
            }

            string[] parts = lastVoucher.vou02full_no.Split('-');
            if (parts.Length >= 3 && int.TryParse(parts[2], out int serialNumber))
            {
                return serialNumber + 1;
            }
            else
            {
                return 1;
            }
        }


        protected abstract IQueryable<ListVMModel> ProcessListData(IQueryable<MainEntity> data);

        [HttpGet("GetList")]
        public virtual async Task<IEnumerable<ListVMModel>> GetList()
        {
            var _que = _MainRepo.GetList()
                .OrderByDescending(v => v.DateCreated)
                .AsNoTracking();

            var processQuery = ProcessListData(_que); // proecss the data as per the view model

            var resultList = await processQuery.ToListAsync();
            return resultList;
        }

        protected abstract IQueryable<ListVMModel> ProcessApprovedListData(IQueryable<MainEntity> data);
        [HttpGet("GetApprovedList")]
        public virtual async Task<IEnumerable<ListVMModel>> GetApprovedList()
        {
            var _que = _MainRepo.GetList()
                .OrderByDescending(v => v.DateCreated)
                .AsNoTracking();

            var processQuery = ProcessApprovedListData(_que);

            var resultList = await processQuery.ToListAsync();
            return resultList;
        }

        protected abstract IQueryable<ListVMModel> ProcessUnApprovedListData(IQueryable<MainEntity> data);
        [HttpGet("GetUnApprovedList")]
        public virtual async Task<IEnumerable<ListVMModel>> GetUnApprovedList()
        {
            var _que = _MainRepo.GetList()
                .OrderByDescending(v => v.DateCreated)
                .AsNoTracking();

            var processQuery = ProcessUnApprovedListData(_que);

            var resultList = await processQuery.ToListAsync();
            return resultList;
        }

        [HttpPost("Create")]
        public virtual async Task<IActionResult> Create(ReqModel Data)
        {
            try
            {
                MainEntity newData = AssignValues(Data);
                //var eventsdata = _mapper.Map<MainEntity>(newData);
                _MainRepo.Insert(newData);
                await _MainRepo.SaveAsync();

                var configData = _configurationSettings.GetList()
                    .FirstOrDefault(x => x.Name == EnumConfigSettings.AutoApproveVoucher.ToString());

                if (configData != null && configData.Value == "true")
                {
                    if (newData is vou02voucher_summary voucherData)
                    {
                        var remark = new ChangeStatusRequest()
                        {
                            Remarks = "Auto Approved By SYSTEM"
                        };

                        ChangeVoucherStatus(EnumVoucherStatus.Approved, voucherData.vou02full_no, remark);
                    }
                    else
                    {
                        return BadRequest("Invalid entity type for auto-approval.");
                    }
                }


                return Ok("Created Succesfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to add data. {ex.Message}");
            }
        }

        protected abstract MainEntity AssignValues(ReqModel Data);


        [HttpPatch("Update")]
        public async Task<IActionResult> Update(PKType id, ReqModel Data)
        {
            try
            {

                var oldData = await _MainRepo.GetDetailAsync(id);
                if (oldData == null) { throw new Exception("Invalid  ID"); }

                ReAssignValues(Data, oldData);

                _MainRepo.Update(oldData);
                await _MainRepo.SaveAsync();

                return Ok("Data Updated Successfully.");

            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message);
            }
        }
        protected virtual void ReAssignValues(ReqModel Data, MainEntity oldData)
        {
            return;
        }

        protected abstract DetailVMModel ProcessDetailData(MainEntity data);

        [HttpGet("GetDetail")]
        public virtual async Task<DetailVMModel> GetDetail(PKType id)
        {
            var _que = await _MainRepo.GetDetailAsync(id);  //get the data from db

            var result = ProcessDetailData(_que); // proecss the data as per the view model

            return result;

        }

        [HttpDelete("Delete")]
        public virtual async Task<IActionResult> Delete(PKType id)
        {
            var oldData = await _MainRepo.GetDetailAsync(id);
            if (oldData == null) { throw new Exception("Invalid  ID"); }
            oldData.DateDeleted = DateTime.UtcNow;

            _MainRepo.Update(oldData);
            await _MainRepo.SaveAsync();
            return Ok("Data Deleted Sucessfully");

        }

        [HttpPatch]
        [Route("[action]/{id}")]
        public IActionResult Approve(string id, [FromBody] ChangeStatusRequest request)
        {
            return ChangeVoucherStatus(EnumVoucherStatus.Approved, id, request);
        }
        [HttpPatch]
        [Route("[action]/{id}")]
        public IActionResult Reject(string id, [FromBody] ChangeStatusRequest request)
        {
            return ChangeVoucherStatus(EnumVoucherStatus.Rejected, id, request);
        }
        [HttpPatch]
        [Route("[action]/{id}")]
        public IActionResult UnApprove(string id, [FromBody] ChangeStatusRequest request)
        {
            return ChangeVoucherStatus(EnumVoucherStatus.Pending, id, request);
        }

        private IActionResult ChangeVoucherStatus(EnumVoucherStatus status, string id, [FromBody] ChangeStatusRequest request)
        {
            try
            {
                var voucherEntity = _context.vou02voucher_summary.Include(c => c.vou03voucher_details).ThenInclude(x => x.led01ledgers).ThenInclude(x => x.led05ledger_types).FirstOrDefault(v => v.vou02full_no == id);

                if (voucherEntity == null)
                {
                    throw new Exception("Voucher ID not found.");
                }
                EnumVoucherStatus oldStatus = voucherEntity.vou02status;
                //validation 
                //already approved cha ki nai/

                voucherEntity.vou02status = status;
                voucherEntity.vou02description = request.Remarks;
                _context.SaveChanges();

                if (!_voucherService.CreateVoucherLogOnStatusUpdate(status, id).GetAwaiter().GetResult())
                {
                    throw new Exception("Voucher log not Created");
                }

                //lets update ledger balance
                // Raise events based on status change
                if (oldStatus == EnumVoucherStatus.Pending && status == EnumVoucherStatus.Approved)
                {
                    VoucherApproved?.Invoke(voucherEntity);
                }
                if (status == EnumVoucherStatus.Pending && oldStatus == EnumVoucherStatus.Approved)
                {
                    VoucherUnApproved?.Invoke(voucherEntity);
                }

                return Ok("Done");
            }
            catch (Exception ex)
            {

                return Problem(statusCode: 500, detail: ex.Message);
            }
        }

        protected bool UpdateLedgerBalance(vou02voucher_summary Data, bool IsReverse = false)
        {
            foreach (var item in Data.vou03voucher_details)
            {
                if (item.led01ledgers.led05ledger_types.led05add_dr)
                {
                    item.led01ledgers.led01balance += (item.vou03dr - item.vou03cr) * (IsReverse ? -1 : 1);
                    item.led01ledgers.DateUpdated = DateTime.Now;
                }
                else
                {
                    item.led01ledgers.led01balance += (item.vou03cr - item.vou03dr) * (IsReverse ? -1 : 1);
                    item.led01ledgers.DateUpdated = DateTime.Now;
                }
                _context.Entry(item).State = EntityState.Modified;
            }
            _context.SaveChanges();
            return true;
        }

        protected bool OnVoucherApproved(vou02voucher_summary Data)
        {
            ///update related sub ledgers ko data to their balance
            return UpdateLedgerBalance(Data, false);
        }
        protected bool OnVoucherRejected(vou02voucher_summary Data)
        {
            return true;//
        }
        protected bool OnVoucherUnApproved(vou02voucher_summary Data)
        {
            ///update related sub ledgers ko data to their balance
            return UpdateLedgerBalance(Data, true);
        }
    }
}