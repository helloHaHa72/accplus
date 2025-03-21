using System.ComponentModel.DataAnnotations.Schema;
using Azure.Core;
using BaseAppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Services;
using POSV1.TenantModel;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Repo.Implementation;
using POSV1.TenantModel.Repo.Interface;
using POSV1.TenantModel.Repo.Interface.Accounting;

namespace POSV1.TenantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoVoucherController : ControllerBase
    {
        private readonly ISaleItemsRepo _saleItemsRepo;
        private readonly ISalesRepo _salesRepo;
        private readonly IPurchaseItemsRepo _purchaseItemsRepo;
        private readonly IPurchaseRepo _purchaseRepo;
        private readonly ILedgersRepo _ledgerRepo;
        private readonly IVoucherSummaryRepo _voucherSummaryRepo;
        private readonly IVoucherDetailsRepo _voucherDetailsRepo;
        private readonly ICashSettlementRepo _cashSettlementRepo;
        private readonly IPurchaseReturnRepo _purchaseReturnRepo;
        private readonly IPurchasereturnitemsRepo _purchasereturnitemsRepo;
        private readonly ISaleReturnRepo _saleReturnRepo;
        private readonly ISalesItemReturnRepo _salesItemReturnRepo;
        private readonly IVoucherService _voucherService;
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSettings _configurationSettings;
        public AutoVoucherController(
            ISaleItemsRepo saleItemsRepo,
            ISalesRepo salesRepo,
            IPurchaseRepo purchaseRepo,
            IPurchaseItemsRepo purchaseItemsRepo,
            ILedgersRepo ledgersRepo,
            IVoucherSummaryRepo voucherSummaryRepo,
            IVoucherDetailsRepo voucherDetailsRepo,
            ICashSettlementRepo cashSettlementRepo,
            ISaleReturnRepo saleReturnRepo,
            ISalesItemReturnRepo salesItemReturnRepo,
            IPurchaseReturnRepo purchaseReturnRepo,
            IPurchasereturnitemsRepo purchasereturnitemsRepo,
            IConfiguration configuration,
            IVoucherService voucherService,
            IConfigurationSettings configurationSettings)
        {
            _saleItemsRepo = saleItemsRepo;
            _salesRepo = salesRepo;
            _purchaseItemsRepo = purchaseItemsRepo;
            _purchaseRepo = purchaseRepo;
            _ledgerRepo = ledgersRepo;
            _voucherSummaryRepo = voucherSummaryRepo;
            _voucherDetailsRepo = voucherDetailsRepo;
            _cashSettlementRepo = cashSettlementRepo;
            _saleReturnRepo = saleReturnRepo;
            _salesItemReturnRepo = salesItemReturnRepo;
            _purchaseReturnRepo = purchaseReturnRepo;
            _purchasereturnitemsRepo = purchasereturnitemsRepo;
            _configuration = configuration;
            _voucherService = voucherService;
            _configurationSettings = configurationSettings;
        }

        private string GenerateVoucherNumber(string prefix, string branchPrefix, int sn)
        {
            string paddedSn = sn.ToString().PadLeft(4, '0');

            return $"{prefix}-{branchPrefix}-{paddedSn}";
        }

        private int GetNextSerialNumber()
        {
            var lastVoucher = _voucherSummaryRepo.GetList().OrderByDescending(v => v.vou02full_no).FirstOrDefault();

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

        [HttpPost]
        public async Task<IActionResult> Create(DateTime date)
        {
            try
            {
                var voucherDetail = await _voucherSummaryRepo.GetList()
                    .Include(x => x.vou03voucher_details)
               .Where(x => x.vou02is_sys_generated && x.vou02value_date.Date == date)
               .AsNoTracking()
               .FirstOrDefaultAsync();
                if (voucherDetail != null)
                {
                    return Ok(await GetVoucherEntity(voucherDetail));
                }
                else
                {
                    var salesData = _salesRepo.GetList().Where(x => x.sal01date_eng.Date == date).Include(x => x.cus01customers).ToList();
                    var saleItemRecords = _saleItemsRepo.GetList().Where(x => x.DateCreated.Date == date).Include(x => x.pro02products).ToList();

                    var salesReturnData = _saleReturnRepo.GetList().Where(x => x.sal01date_eng.Date == date).Include(x => x.cus01customers).ToList();
                    var saleReturnItemRecords = _salesItemReturnRepo.GetList().Where(x => x.DateCreated.Date == date).Include(x => x.pro02products).ToList();

                    var purchaseData = _purchaseRepo.GetList().Where(x => x.pur01date.Date == date).Include(x => x.ven01vendors).ToList();
                    var purchaseItemRecords = _purchaseItemsRepo.GetList().Where(x => x.DateCreated.Date == date).Include(x => x.pro02products).ToList();

                    var purchaseReturnData = _purchaseReturnRepo.GetList().Where(x => x.pur01return_date == date).Include(x => x.ven01vendors).ToList();
                    var purchaseReturnItemsRecords = _purchasereturnitemsRepo.GetList().Where(x => x.DateCreated.Date == date).Include(x => x.pro02products).ToList();

                    var saleDiscLedger = _ledgerRepo.GetList().Where(x => x.led01title == "Discount_Given" && x.led01date.HasValue && x.led01date.Value.Date == date.Date).ToList();
                    var purchaseDiscledger = _ledgerRepo.GetList().Where(x => x.led01title == "Discount_Taken" && x.led01date.HasValue && x.led01date.Value.Date == date.Date).ToList();

                    var vendorCashSettlement = _cashSettlementRepo.GetList().Where(x => x.cas01vendoruin != null && x.cas01vendoruin != 0 && x.cas01transaction_date == date).Include(x => x.ven01vendors).ToList();
                    var customerCashSettlement = _cashSettlementRepo.GetList().Where(x => x.cas01customeruin != null && x.cas01customeruin != 0 && x.cas01transaction_date == date).Include(x => x.cus01customers).ToList();

                    var totalSalesDiscount = saleDiscLedger.Sum(item => item.led01balance);
                    var totalPurchaseDiscount = purchaseDiscledger.Sum(item => item.led01balance);

                    var result = saleItemRecords.Select(item => new VMAutoVoucherDetail()
                    {
                        Led_Code = "p_" + item.pro02products.pro02code,
                        Dr = 0,
                        Cr = (decimal)item.sal02net_amt
                    })
                        .Concat(salesReturnData.Select(x => new VMAutoVoucherDetail()
                        {
                            Led_Code = "c_" + x.cus01customers.cus01led_code,
                            Dr = 0,
                            Cr = x.sal01net_amt,
                        }))
                        .Concat(salesData.Select(x => new VMAutoVoucherDetail()
                        {
                            Led_Code = "c_" + x.cus01customers.cus01led_code,
                            Dr = x.sal01net_amt,
                            Cr = 0,
                        }))
                        .Concat(saleReturnItemRecords.Select(x => new VMAutoVoucherDetail()
                        {
                            Led_Code = "p_" + x.pro02products.pro02code,
                            Dr = (decimal)x.sal02net_amt,
                            Cr = 0
                        }))
                        .Concat(purchaseItemRecords.Select(item => new VMAutoVoucherDetail()
                        {
                            Led_Code = "p_" + item.pro02products.pro02code,
                            Dr = item.pur02net_amt,
                            Cr = 0
                        }))
                        .Concat(purchaseReturnItemsRecords.Select(item => new VMAutoVoucherDetail()
                        {
                            Led_Code = "p_" + item.pro02products.pro02code,
                            Dr = 0,
                            Cr = item.pur02net_amt
                        }))
                        .Concat(purchaseData.Select(x => new VMAutoVoucherDetail()
                        {
                            Led_Code = "v_" + x.ven01vendors.ven01led_code,
                            Dr = 0,
                            Cr = x.pur01net_amt,
                        }))
                        .Concat(purchaseReturnData.Select(x => new VMAutoVoucherDetail()
                        {
                            Led_Code = "v_" + x.ven01vendors.ven01led_code,
                            Dr = x.pur01return_net_amt,
                            Cr = 0,
                        }))
                        //.Concat(saleDiscLedger.Select(item => new VMAutoVoucherDetail()
                        //{
                        //    //Led_Code = _configuration["GeneralLedgerConfigurations:DefaultLedgerForSalesDiscount"],
                        //    Led_Code = "SalesDiscount",
                        //    //Led_Code = item.led01code,
                        //    //Dr = item.led01balance,
                        //    Dr = saleDiscLedger.Sum(item => item.led01balance),
                        //    Cr = 0
                        //}))
                        //.Concat(purchaseDiscledger.Select(x => new VMAutoVoucherDetail()
                        //{
                        //    Led_Code = "PurchaseDiscount",
                        //    //Led_Code = _configuration["GeneralLedgerConfigurations:DefaultLedgerForPurchaseDiscount"],
                        //    //Led_Code = x.led01code,
                        //    Dr = 0,
                        //    Cr = purchaseDiscledger.Sum(item => item.led01balance),
                        //    //Cr = x.led01balance,
                        //}))
                        .Concat(vendorCashSettlement.Select(x => new VMAutoVoucherDetail() //vendor cash settlement
                        {
                            Led_Code = "v_" + x.ven01vendors.ven01led_code,
                            Dr = x.cas01amount,
                            Cr = 0,
                        }))
                        .Concat(customerCashSettlement.Select(x => new VMAutoVoucherDetail() //vendor cash settlement
                        {
                            Led_Code = "c_" + x.cus01customers.cus01led_code,
                            Dr = 0,
                            Cr = x.cas01amount,
                        }))
                        .ToList();

                    if (totalSalesDiscount != 0)
                    {
                        result.Add(new VMAutoVoucherDetail()
                        {
                            Led_Code = "SalesDiscount",
                            Dr = totalSalesDiscount,
                            Cr = 0
                        });
                    }

                    // Add purchase discount only once
                    if (totalPurchaseDiscount != 0)
                    {
                        result.Add(new VMAutoVoucherDetail()
                        {
                            Led_Code = "PurchaseDiscount",
                            Dr = 0,
                            Cr = totalPurchaseDiscount
                        });
                    }

                    if (result.Count == 0)
                    {
                        return Ok("No sales and items and purchase records");
                    }

                    var total_Cr = result.Sum(x => x.Cr);
                    var total_Dr = result.Sum(x => x.Dr);

                    return Ok(await CreateVoucher(result, date));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<VMAutoVoucher> CreateVoucher(List<VMAutoVoucherDetail> result, DateTime dateTime)
        {
            try
            {
                string prefix = "VOU";
                string branchPrefix = "JOU01";

                var voucherEntity = new vou02voucher_summary
                {
                    vou02full_no = GenerateVoucherNumber(prefix, branchPrefix, GetNextSerialNumber()),
                    vou02vou01uin = (int)EnumVoucherTypes.Journal,
                    vou02amount = 0,
                    vou02description = "Voucher Summary of " + DateTime.Today,
                    vou02manual_vno = dateTime.ToString("yyyymmdd") + "_V",
                    vou02is_sys_generated = true,
                    vou02value_date = dateTime,
                    vou02status = EnumVoucherStatus.Pending,
                    vou02chq = "123",
                    vou03voucher_details = new List<vou03voucher_details>(),
                };

                var chq_no = await _voucherDetailsRepo.GetList().Select(x => x.vou03uin).OrderByDescending(x => x).LastOrDefaultAsync() + 1;

                foreach (var detail in result)
                {
                    var ledger = _ledgerRepo.GetList().Where(x => x.led01code == detail.Led_Code).FirstOrDefault();

                    if (ledger == null)
                    {
                        throw new Exception("Ledger not found.");

                    }
                    var voucherDetailEntity = new vou03voucher_details
                    {
                        vou03led05uin = ledger.led01uin,
                        vou03dr = detail.Dr,
                        vou03cr = detail.Cr,
                        vou03description = "voucher",
                        vou03chq = $"{chq_no}",
                        vou03balance = ledger.led01balance,
                    };
                    chq_no += 1;
                    voucherEntity.vou03voucher_details.Add(voucherDetailEntity);
                }
                //voucherEntity.vou02status = 
                _voucherSummaryRepo.Insert(voucherEntity);
                await _voucherSummaryRepo.SaveAsync();

                ApproveVoucher(voucherEntity);

                await UpdatePreviousRecordsWithVoucherNo(dateTime, voucherEntity.vou02full_no);

                return await GetVoucherEntity(voucherEntity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ApproveVoucher(vou02voucher_summary voucherEntity)
        {
            try
            {
                var configData = _configurationSettings.GetList()
                    .FirstOrDefault(x => x.Name == EnumConfigSettings.AutoApproveVoucher.ToString());

                if (configData != null && configData.Value == "true")
                {
                    EnumVoucherStatus oldStatus = voucherEntity.vou02status;
                    voucherEntity.vou02status = EnumVoucherStatus.Approved;
                    voucherEntity.vou02description = "Approved by SYSTEM";
                    //_context.SaveChanges();

                    if (!_voucherService.CreateVoucherLogOnStatusUpdate(EnumVoucherStatus.Approved, voucherEntity.vou02full_no).GetAwaiter().GetResult())
                    {
                        throw new Exception("Voucher log not Created");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task UpdatePreviousRecordsWithVoucherNo(DateTime date, string voucherNo)
        {
            // **Update Sales Table**
            var salesRecords = _salesRepo.GetList().Where(x => x.sal01date_eng.Date == date).ToList();
            foreach (var sale in salesRecords)
            {
                sale.sal01voucher_no = voucherNo;
            }
            if (salesRecords.Any())
            {
                await _salesRepo.UpdateRangeAsync(salesRecords);
                //await _salesRepo.SaveAsync();
            }

            var salesReturnRecords = _saleReturnRepo.GetList().Where(x => x.sal01date_eng.Date == date).ToList();
            foreach (var sale in salesReturnRecords)
            {
                sale.sal01returnvoucher_no = voucherNo;
            }
            if (salesReturnRecords.Any())
            {
                await _saleReturnRepo.UpdateRangeAsync(salesReturnRecords);
                //await _salesRepo.SaveAsync();
            }

            // **Update Purchase Table**
            var purchaseRecords = _purchaseRepo.GetList().Where(x => x.pur01date.Date == date).ToList();
            foreach (var purchase in purchaseRecords)
            {
                purchase.pur01voucher_no = voucherNo;
            }
            if (purchaseRecords.Any())
            {
                await _purchaseRepo.UpdateRangeAsync(purchaseRecords);
                //await _purchaseRepo.SaveAsync();
            }

            var purchaseReturnRecords = _purchaseReturnRepo.GetList().Where(x => x.pur01return_date.Date == date).ToList();
            foreach (var purchase in purchaseReturnRecords)
            {
                purchase.pur01returnvoucher_no = voucherNo;
            }
            if (purchaseReturnRecords.Any())
            {
                await _purchaseReturnRepo.UpdateRangeAsync(purchaseReturnRecords);
                //await _purchaseRepo.SaveAsync();
            }

            // **Update Cash Settlement Table**
            var cashSettlements = _cashSettlementRepo.GetList()
                .Where(x => x.cas01transaction_date == date)
                .ToList();
            foreach (var settlement in cashSettlements)
            {
                settlement.cas0101voucher_no = voucherNo;
            }
            if (cashSettlements.Any())
            {
                await _cashSettlementRepo.UpdateRangeAsync(cashSettlements);
                //await _cashSettlementRepo.SaveAsync();
            }

            var transactionTypes = new List<int> { (int)EnumTransactionTypes.MonthlyPayroll, (int)EnumTransactionTypes.Advance };

        }

        private async Task<VMAutoVoucher> GetVoucherEntity(vou02voucher_summary voucherEntity)
        {
            VMAutoVoucher rlt = new VMAutoVoucher()
            {
                VoucherNo = voucherEntity.vou02full_no,
                ValueDate = voucherEntity.vou02value_date,
                ManualVno = voucherEntity.vou02manual_vno,
                TotalCredit = voucherEntity.vou03voucher_details.Sum(d => d.vou03cr),
                TotalDebit = voucherEntity.vou03voucher_details.Sum(d => d.vou03dr),
                UpdatedName = "Admin"
            };
            IQueryable<vou03voucher_details> _que = _voucherDetailsRepo.GetList()
                 .Where(vd =>
                     vd.vou03vou02full_no == voucherEntity.vou02full_no);

            rlt.VMAutoVoucherDetail = await _que.Select(x => new VMAutoVoucherDetail()
            {
                Led_Code = x.led01ledgers.led01code,
                Dr = x.vou03dr,
                Cr = x.vou03cr,


            }).ToListAsync();

            return rlt;
        }
    }
}
