using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Services;
using POSV1.TenantModel.Repo.Implementation;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantAPI.Controllers.Inventory
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatementController : ControllerBase
    {
        private readonly ISalesRepo _salesRepo;
        private readonly ISaleReturnRepo _saleReturnRepo;
        private readonly IledgerService _ledgerService;
        private readonly ICashSettlementRepo _cashSettlementRepo;
        private readonly ICustomersRepo _customersRepo;

        private readonly IPurchaseRepo _purchaseRepo;
        private readonly IPurchaseReturnRepo _purchaseReturnRepo;
        private readonly IVendorRepo _vendorRepo;

        public StatementController(ISalesRepo salesRepo, ISaleReturnRepo saleReturnRepo, IledgerService ledgerService, ICashSettlementRepo cashSettlementRepo
            , IPurchaseRepo purchaseRepo, IPurchaseReturnRepo purchaseReturnRepo, IVendorRepo vendorRepo, ICustomersRepo customersRepo)
        {
            _salesRepo = salesRepo;
            _saleReturnRepo = saleReturnRepo;
            _ledgerService = ledgerService;
            _cashSettlementRepo = cashSettlementRepo;
            _purchaseRepo = purchaseRepo;
            _purchaseReturnRepo = purchaseReturnRepo;
            _vendorRepo = vendorRepo;
            _customersRepo = customersRepo;
        }

        [HttpGet("CustomerTxnSummary")]
        public async Task<IActionResult> EmployeeTransactionSummary(int cus_id)
        {
            var customer = await _customersRepo.GetDetailAsync(cus_id);

            if (customer == null)
            {
                throw new Exception("Invalid user id !!!");
            }
            try
            {
                var saleData = await _salesRepo.GetList().Where(x => x.sal01cus01uin == cus_id).ToListAsync();

                var saleReturnData = await _saleReturnRepo.GetList().Where(x => x.sal01cus01uin == cus_id).ToListAsync();

                var cashSettlementData = await _cashSettlementRepo.GetList().Where(x => x.cas01customeruin == cus_id).ToListAsync();

                var txnDetailRecords = await _ledgerService.GetStatement(customer.cus01led_code);

                var transactions = new List<VMEmpTransactionSummaryDTO>();

                transactions.AddRange(saleData.Select(sale => new VMEmpTransactionSummaryDTO
                {
                    Date = DateTime.Parse(sale.sal01date_nep),
                    Debit = sale.sal01net_amt,
                    Credit = 0,
                    Discount = sale.sal01disc_amt,
                    VAT = sale.sal01vat_amt
                }));

                transactions.AddRange(saleReturnData.Select(saleReturn => new VMEmpTransactionSummaryDTO
                {
                    Date = DateTime.Parse(saleReturn.sal01date_nep),
                    Debit = 0,
                    Credit = saleReturn.sal01net_amt,
                    Discount = saleReturn.sal01disc_amt,
                    VAT = saleReturn.sal01vat_amt
                }));

                transactions.AddRange(cashSettlementData.Select(cashSettlement => new VMEmpTransactionSummaryDTO
                {
                    Date = cashSettlement.cas01transaction_date,
                    Debit = cashSettlement.cas01amount,
                    Credit = 0,
                    Discount = 0,
                    VAT = 0
                }));

                transactions.AddRange(txnDetailRecords.Select(txn => new VMEmpTransactionSummaryDTO
                {
                    Date = txn.Date.Date,
                    Debit = txn.Debit,
                    Credit = txn.Credit,
                    Discount = 0,
                    VAT = 0
                }));

                // Order the transactions by date
                var orderedTransactions = transactions.OrderBy(t => t.Date).ToList();

                return Ok(orderedTransactions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("VendorTxnSummary")]
        public async Task<IActionResult> VendorTransactionSummary(int ven_id)
        {
            var vendor = await _vendorRepo.GetDetailAsync(ven_id);
            try
            {
                var purchaseData = await _purchaseRepo.GetList().Where(x => x.pur01ven01uin == ven_id).ToListAsync();

                var purchaseReturnData = await _purchaseReturnRepo.GetList().Where(x => x.pur01ven01uin == ven_id).ToListAsync();

                var cashSettlementData = await _cashSettlementRepo.GetList().Where(x => x.cas01vendoruin == ven_id).ToListAsync();

                var txnDetailRecords = await _ledgerService.GetStatement(vendor.ven01led_code);

                var transactions = new List<VMEmpTransactionSummaryDTO>();

                transactions.AddRange(purchaseData.Select(sale => new VMEmpTransactionSummaryDTO
                {
                    Date = DateTime.Parse(sale.pur01date_nep),
                    Debit = sale.pur01net_amt,
                    Credit = 0,
                    Discount = sale.pur01disc_amt,
                    VAT = sale.pur01vat_amt
                }));

                transactions.AddRange(purchaseReturnData.Select(saleReturn => new VMEmpTransactionSummaryDTO
                {
                    Date = DateTime.Parse(saleReturn.pur01return_date_nep),
                    Debit = 0,
                    Credit = saleReturn.pur01return_net_amt,
                    Discount = saleReturn.pur01return_disc_amt,
                    VAT = saleReturn.pur01return_vat_amt
                }));

                transactions.AddRange(cashSettlementData.Select(cashSettlement => new VMEmpTransactionSummaryDTO
                {
                    Date = cashSettlement.cas01transaction_date,
                    Debit = cashSettlement.cas01amount,
                    Credit = 0,
                    Discount = 0,
                    VAT = 0
                }));

                transactions.AddRange(txnDetailRecords.Select(txn => new VMEmpTransactionSummaryDTO
                {
                    Date = txn.Date.Date,
                    Debit = txn.Debit,
                    Credit = txn.Credit,
                    Discount = 0,
                    VAT = 0
                }));

                // Order the transactions by date
                var orderedTransactions = transactions.OrderBy(t => t.Date).ToList();

                return Ok(orderedTransactions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

public class VMEmpTransactionSummaryDTO
{
    public DateTime Date { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public decimal Discount { get; set; }
    public decimal VAT { get; set; }
}
