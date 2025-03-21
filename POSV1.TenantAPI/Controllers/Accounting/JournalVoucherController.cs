using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using POSV1.TenantAPI.Controllers.Accounting;
using POSV1.TenantAPI.Models;
using POSV1.TenantModel;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Repo.Interface.Accounting;
using POSV1.TenantModel.Repo.Interface;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantAPI.Services;
using Microsoft.AspNetCore.Authorization;
using POSV1.TenantModel.Models.EntityModels.Accounting;
using BaseAppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace POSV1.TenantAPI.Controllers
{
    //public JournalVoucherController(MainDbContext db) : base(db)
    //{
    //    VoucherApproved += OnJournalVoucherApproved;
    //}
    //// Then, define the event handler:
    //protected bool OnJournalVoucherApproved(vou02voucher_summary Data)
    //{
    //    // This method will be called when VoucherApproved event is raised.
    //    // It will execute the code inside this method.
    //    //my additional task code goes here
    //    return true;
    //}

    [Route("api/[controller]")]
    [ApiController]
    public class JournalVoucherController :
        _AbsVoucherCRUDController<
            IncomeVoucherController,
            IVoucherSummaryRepo,
            vou02voucher_summary,
            VMVoucher,
            VMVoucherList,
            VMVoucherListDetail,
            string>
    {
        private readonly IMapper mapper;
        private readonly MainDbContext _context;
        private readonly IVoucherDetailsRepo _voucherDetailsRepo;
        private readonly ILedgersRepo _ledgersRepo;
        private readonly IVoucherService _voucherService;
        public JournalVoucherController(
            ILogger<IncomeVoucherController> logger,
            IVoucherSummaryRepo voucherSummaryRepo,
            IVoucherDetailsRepo voucherDetailsRepo,
            ILedgersRepo ledgersRepo,
            IMapper mapper,
            MainDbContext context,
            IVoucherService voucherService,
            IConfigurationSettings configurationSettings)
            : base(logger, voucherSummaryRepo, mapper, context, voucherService, configurationSettings)
        {
            _context = context;
            _voucherDetailsRepo = voucherDetailsRepo;
            _ledgersRepo = ledgersRepo;
        }
        private static IQueryable<VMVoucherList> GetData(IQueryable<vou02voucher_summary> filteredData)
        {
            if (filteredData == null)
            {
                throw new Exception("Journal vouchers not found");
            }

            return filteredData.Select(voucher => new VMVoucherList
            {
                VoucherNo = voucher.vou02full_no,
                VoucherType = voucher.vou01voucher_types.vou01title,
                ValueDate = voucher.vou02value_date,
                ManualVno = voucher.vou02manual_vno,
                Remarks = voucher.vou02description,
                TotalCredit = voucher.vou03voucher_details.Sum(d => d.vou03cr),
                TotalDebit = voucher.vou03voucher_details.Sum(d => d.vou03dr),
                Status = voucher.vou02status.ToString(),

                CreatedName = voucher.CreatedName,
                UpdatedName = voucher.UpdatedName,
            });
        }
        protected override IQueryable<VMVoucherList> ProcessListData(IQueryable<vou02voucher_summary> data)
        {
            var filteredData = data
                   .Where(x => x.vou01voucher_types.vou01title == "Journal");
            return GetData(filteredData);
        }

        protected override IQueryable<VMVoucherList> ProcessApprovedListData(IQueryable<vou02voucher_summary> data)
        {
            var filteredData = data
                   .Where(x => x.vou01voucher_types.vou01title == "Journal" && x.vou02status == EnumVoucherStatus.Approved);
            return GetData(filteredData);
        }

        protected override IQueryable<VMVoucherList> ProcessUnApprovedListData(IQueryable<vou02voucher_summary> data)
        {
            var filteredData = data
                   .Where(x => x.vou01voucher_types.vou01title == "Journal" && x.vou02status == EnumVoucherStatus.Pending);
            return GetData(filteredData);
        }

        protected override VMVoucherListDetail ProcessDetailData(vou02voucher_summary data)
        {
            if (data == null)
            {
                throw new Exception("Journal vouchers not found");
            }

            var contraLedgerDetail = new led01ledgers();
            if (data.vou02contra_led05uin != null){
                contraLedgerDetail = _ledgersRepo.GetDetail((int)data.vou02contra_led05uin);
            }
            //if (contraLedgerDetail == null)
            //{
            //    throw new Exception("Contra ledger not found");
            //}

            VMVoucherListDetail Result = new VMVoucherListDetail()
            {
                ID = data.vou02full_no,
                VoucherNo = data.vou02full_no,
                VoucherType = "s",
                ValueDate = data.vou02value_date,
                ManualVno = data.vou02manual_vno,
                Remarks = data.vou02description,
                Status = data.vou02status.ToString(),
                ContraLedgerId = data.vou02contra_led05uin,
                ContraLedgerName = contraLedgerDetail.led01title,
                ChqNo = data.vou02chq,
                CreatedName = data.CreatedName,
                UpdatedName = data.UpdatedName,
            };

            var _Que = _voucherDetailsRepo.GetList()
                .Include(x => x.led01ledgers)
                  .Where(x => x.vou03vou02full_no == data.vou02full_no);

            Result.VMDetails = _Que.Select(x => new VMDetails()
            {
                ID = x.vou03uin,
                LedgerId = x.vou03led05uin,
                LedgerName = x.led01ledgers.led01title,
                LedgerNameCode = x.led01ledgers.led01title + "[" + x.led01ledgers.led01code + "]",
                Debit = x.vou03dr,
                Credit = x.vou03cr,
                Balance = x.vou03balance,
                Description = x.vou03description,
                ChqNo = x.vou03chq
            })
                 .ToList();

            return Result;
        }

        protected override vou02voucher_summary AssignValues(VMVoucher Data)
        {
            string prefix = "VOU";
            string branchPrefix = "JOU01";

            if (Data == null)
            {
                throw new Exception("Invalid Data");
            }

            var voucherEntity = new vou02voucher_summary
            {
                vou02full_no = GenerateVoucherNumber(prefix, branchPrefix, GetNextSerialNumber()),
                vou02vou01uin = (int)EnumVoucherTypes.Journal,
                vou02amount = Data.Amount,
                vou02description = Data.Remarks,
                vou02manual_vno = Data.ManualVno,
                vou02value_date = Data.ValueDate,
                vou02status = EnumVoucherStatus.Pending,
                vou02chq = Data.ChqNo,
                vou02contra_led05uin = Data.ContraLedgerId,
                vou03voucher_details = new List<vou03voucher_details>(),

                DateCreated = DateTime.Now,
                CreatedName = _ActiveUserName
            };

            if (Data.VMVoucherDetailCreate != null)
            {
                foreach (var voucher_Detail in Data.VMVoucherDetailCreate)
                {
                    var ledger = _ledgersRepo.GetDetail(voucher_Detail.LedgerId);

                    if (ledger == null)
                    {
                        throw new Exception("Ledger not found");
                    }

                    var voucherDetailEntity = new vou03voucher_details
                    {
                        vou03led05uin = ledger.led01uin,
                        vou03dr = voucher_Detail.Debit,
                        vou03cr = voucher_Detail.Credit,
                        vou03description = voucher_Detail.Description,
                        vou03chq = voucher_Detail.ChqNo,
                        vou03balance = ledger.led01balance,

                        CreatedName = _ActiveUserName,
                        DateCreated = DateTime.Now,
                    };

                    voucherEntity.vou03voucher_details.Add(voucherDetailEntity);
                }
            }

            return voucherEntity;
        }

        protected override void ReAssignValues(VMVoucher Data, vou02voucher_summary oldData)
        {
            try
            {
                if (oldData == null)
                {
                    throw new Exception("Voucher not found.");
                }

                oldData.vou02amount = Data.Amount;
                oldData.vou02description = Data.Remarks;
                oldData.vou02manual_vno = Data.ManualVno;
                oldData.vou02value_date = Data.ValueDate;
                oldData.vou02contra_led05uin = Data.ContraLedgerId;

                oldData.DateUpdated = DateTime.Now;
                oldData.UpdatedName = _ActiveUserName;

                _voucherDetailsRepo.LoadVoucherDetails(oldData);
                if (Data.VMVoucherDetailCreate != null)
                {
                    SoftDeleteNonExisingChild(Data, oldData);

                    AddUpdateNewChildRecords(Data, oldData);
                }


            }
            catch (Exception ex)
            {
                throw new Exception($"Internal Server Error: {ex.Message}");
            }
        }

        private void SoftDeleteNonExisingChild(VMVoucher Data, vou02voucher_summary oldData)
        {
            int[] ExistingIDs = Data
               .VMVoucherDetailCreate.Where(x => x.ID > 0)
               .Select(x => x.ID)
               .ToArray();

            // Remove any details that are no longer present in the updated data
            var detailsToRemove = oldData.vou03voucher_details
                .Where(d => !ExistingIDs.Contains(d.vou03uin))
                .ToList();

            foreach (var detailToRemove in detailsToRemove)
            {
                detailToRemove.DateDeleted = DateTime.UtcNow;
            }
        }

        private void AddUpdateNewChildRecords(VMVoucher Data, vou02voucher_summary oldData)
        {
            List<vou03voucher_details> oldDataItems = oldData.vou03voucher_details.ToList().GetRange(0, oldData.vou03voucher_details.Count);

            foreach (var updatedVoucherDetail in Data.VMVoucherDetailCreate)
            {
                var ledger = _context.led01ledgers.FirstOrDefault(l => l.led01uin == updatedVoucherDetail.LedgerId);

                if (ledger == null)
                {
                    throw new Exception("Ledger not found.");
                }

                var existingDetail = oldDataItems
                    .FirstOrDefault(d => d.vou03uin == updatedVoucherDetail.ID);

                bool CreateMode = false;

                if (existingDetail == null)
                {
                    CreateMode = true;
                    existingDetail = new vou03voucher_details()
                    {
                        vou03uin = 0,
                    };
                }

                existingDetail.vou03led05uin = ledger.led01uin;
                existingDetail.vou03dr = updatedVoucherDetail.Debit;
                existingDetail.vou03cr = updatedVoucherDetail.Credit;
                existingDetail.vou03description = updatedVoucherDetail.Description;
                existingDetail.vou03chq = updatedVoucherDetail.ChqNo;
                existingDetail.vou03balance = updatedVoucherDetail.Balance;

                existingDetail.DateUpdated = DateTime.Now;
                existingDetail.UpdatedName = _ActiveUserName;

                if (CreateMode) { oldData.vou03voucher_details.Add(existingDetail); }

            }
        }
    }
}
