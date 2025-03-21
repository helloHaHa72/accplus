using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Models.EntityModels.Inventory;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantAPI.Controllers.Inventory
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryDayBookController : ControllerBase
    {
        private readonly ISaleItemsRepo _saleItemsRepo;
        private readonly ISalesRepo _salesRepo;
        private readonly ISalesItemReturnRepo _saleItemReturnRepo;
        private readonly ISaleReturnRepo _saleReturnRepo;
        private readonly IPurchaseItemsRepo _purchaseItemsRepo;
        private readonly IPurchaseRepo _purchaseRepo;
        private readonly IPurchasereturnitemsRepo _purchaseReturnItemsRepo;
        private readonly IPurchaseReturnRepo _purchaseReturnRepo;
        private readonly ICashSettlementRepo _cashSettlementRepo;
        public InventoryDayBookController(
            ISaleItemsRepo saleItemsRepo,
            ISalesRepo salesRepo,
            IPurchaseRepo purchaseRepo,
            IPurchaseItemsRepo purchaseItemsRepo,
            ICashSettlementRepo cashSettlementRepo,
            ISaleReturnRepo saleReturnRepo,
            ISalesItemReturnRepo salesItemReturnRepo,
            IPurchaseReturnRepo purchaseReturnRepo,
            IPurchasereturnitemsRepo purchasereturnitemsRepo)
        {
            _saleItemsRepo = saleItemsRepo;
            _salesRepo = salesRepo;
            _purchaseItemsRepo = purchaseItemsRepo;
            _purchaseRepo = purchaseRepo;
            _cashSettlementRepo = cashSettlementRepo;
            _saleReturnRepo = saleReturnRepo;
            _saleItemReturnRepo = salesItemReturnRepo;
            _purchaseReturnRepo = purchaseReturnRepo;
            _purchaseReturnItemsRepo = purchasereturnitemsRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetDayBookList(DateTime date)
        {
            try
            {
                List<VMItemWiseSales> itemWiseSalesRecord =await GetItemWiseSalesList(date);

                List<VMItemWiseSales> itemWiseSalesReturnRecord =await GetItemWiseSalesReturnList(date);

                List<VMItemWisePurchase> itemWisePurhcaseRecord =await GetItemWisePurchaseList(date);

                List<VMItemWisePurchase> itemWisePurhcaseReturnRecord =await GetItemWisePurchaseReturnList(date);

                List<VMCustomerWiseSales> customerWiseSalesRecord =await GetCustomerWiseSalesList(date);

                List<VMVendorWisePurchase> vendorWisePurchaseRecord =await GetVendorWisePurchaseList(date);

                List<VMCashSettlementCustomerWise> userWiseCashSettlementRecord = await GetUserWiseCashSettlementList(date);

                var vmInventoryDayBookDetails = new VMInventoryDayBook
                {
                    ItemWiseSalesRecords = itemWiseSalesRecord,
                    ItemWiseSalesReturnRecord = itemWiseSalesReturnRecord,
                    ItemWisePurchaseRecords = itemWisePurhcaseRecord,
                    ItemWisePurhcaseReturnRecord = itemWisePurhcaseReturnRecord,
                    CustomerWiseSalesRecords = customerWiseSalesRecord,
                    VendorWisePurchaseRecords = vendorWisePurchaseRecord,
                    UserWiseCashSettlementRecord = userWiseCashSettlementRecord
                };

                return Ok(vmInventoryDayBookDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        private async Task<List<VMCashSettlementCustomerWise>> GetUserWiseCashSettlementList(DateTime date)
        {
            var VendorWisePurchaseData = _cashSettlementRepo
                .GetList()
                .Where(x => x.cas01transaction_date == date)
                .Include(x => x.cus01customers)
                .Include(x => x.ven01vendors)
                .AsQueryable();

            var vendorWisePurchaseRecord = await VendorWisePurchaseData.Select(x => new VMCashSettlementCustomerWise
            {
                Id = x.cas01uin,
                PaymentType = x.cas01payment_type.ToString(),
                CustomerId = x.cas01customeruin,
                CustomerName = x.cus01customers != null ? x.cus01customers.cus01name_eng : null,
                VendorName = x.ven01vendors != null ? x.ven01vendors.ven01name_eng : null,
                VendorId = x.cas01vendoruin,
                Remarks = x.cas01remarks,
                TransactionDate = x.cas01transaction_date,
                Amount = x.cas01amount,
                BankName = x.cas01bank_ledname,
                IsBank = x.cas01isbank,
                ChqNumber = x.cas01chqnumber
            })
               .ToListAsync();
            return vendorWisePurchaseRecord;
        }

        private async Task<List<VMVendorWisePurchase>> GetVendorWisePurchaseList(DateTime date)
        {
            var VendorWisePurchaseData = _purchaseRepo.GetList()
              .Where(x => x.pur01date.Date == date)
              .Include(x => x.ven01vendors)
              .AsQueryable();
           
            var vendorWisePurchaseRecord = await VendorWisePurchaseData.Select(x => new VMVendorWisePurchase
            {
                 Id = x.pur01uin,
                 VendorName = x.ven01vendors.ven01name_eng,
                 Total_Amt = x.pur01net_amt,
                 Bill_No = x.pur01invoice_no,
                 Remarks = x.pur01remarks,
            })
               .ToListAsync();
            return vendorWisePurchaseRecord;
        }

        private async Task<List<VMCustomerWiseSales>> GetCustomerWiseSalesList(DateTime date)
        {
            var customerWiseSalesData = _salesRepo.GetList()
               .Where(x => x.sal01date_eng.Date == date)
               .Include(x => x.cus01customers)
               .AsQueryable();

            var customerWiseSalesRecord = customerWiseSalesData.Select(x => new VMCustomerWiseSales
            {
                 Id = x.sal01uin,
                 CustomerName = x.cus01customers.cus01name_eng,
                 Total_Amt = x.sal01net_amt,
                 Bill_No = x.sal01invoice_no,
                 Remarks = x.sal01remarks,
            })
              .ToList();
            return customerWiseSalesRecord;
        }

        private async Task<List<VMItemWisePurchase>> GetItemWisePurchaseList(DateTime date)
        {
            var ItemWisePurchaseData = _purchaseRepo.GetList()
                .Where(x => x.pur01date.Date == date)
                .AsQueryable();
            var ItemWisePurchaseItemRecords = _purchaseItemsRepo.GetList()
                .Include(x => x.pro02products)
                .Include(x => x.un01units)
                .AsQueryable();
        
            var itemWisePurhcaseRecord = ItemWisePurchaseItemRecords
                  .Join(ItemWisePurchaseData,
                      record => record.pur02pur01uin,
                      summary => summary.pur01uin,
                      (record, summary) => new VMItemWisePurchase
                      {
                          ID = summary.pur01uin,
                          ProductName = record.pro02products.pro02name_eng,
                          Rate = record.pur02rate,
                          Quantity = record.pur02qty,
                          Unit = record.un01units.un01name_eng,
                          Net_Amt = summary.pur01net_amt,
                          Ref_No = summary.pur01invoice_no,
                      })
                  .ToList();
            return itemWisePurhcaseRecord;
        }


        private async Task<List<VMItemWisePurchase>> GetItemWisePurchaseReturnList(DateTime date)
        {
            var ItemWisePurchaseData = _purchaseReturnRepo.GetList()
                .Where(x => x.pur01return_date.Date == date)
                .AsQueryable();
            var ItemWisePurchaseItemRecords = _purchaseReturnItemsRepo.GetList()
                .Include(x => x.pro02products)
                .Include(x => x.un01units)
                .AsQueryable();

            var itemWisePurhcaseRecord = ItemWisePurchaseItemRecords
                  .Join(ItemWisePurchaseData,
                      record => record.pur02returnpur01uin,
                      summary => summary.pur01uin,
                      (record, summary) => new VMItemWisePurchase
                      {
                          ID = summary.pur01uin,
                          ProductName = record.pro02products.pro02name_eng,
                          Rate = record.pur02returnrate,
                          Quantity = record.pur02returnqty,
                          Unit = record.un01units.un01name_eng,
                          Net_Amt = summary.pur01return_net_amt,
                          Ref_No = summary.pur01return_invoice_no,
                      })
                  .ToList();
            return itemWisePurhcaseRecord;
        }

        private async Task<List<VMItemWiseSales>> GetItemWiseSalesList(DateTime date)
        {
            var ItemWiseSalesData = _salesRepo.GetList()
                .Where(x => x.sal01date_eng.Date == date)
                .AsQueryable();
            var ItemWiseSalesItemRecords = _saleItemsRepo.GetList()
                .Include(x => x.pro02products)
                .AsQueryable();

            var itemWiseSalesRecord = ItemWiseSalesItemRecords
               .Join(ItemWiseSalesData,
                   record => record.sal02sal01uin,
                   summary => summary.sal01uin,
                   (record, summary) => new VMItemWiseSales
                   {
                       ID = summary.sal01uin,
                       ProductName = record.pro02products.pro02name_eng,
                       Rate = record.sal02rate,
                       Quantity = record.sal02qty,
                       Unit = record.un01units.un01name_eng,
                       Net_Amt = summary.sal01net_amt,
                       Ref_No = summary.sal01invoice_no,
                   })
               .ToList();
            return itemWiseSalesRecord;
        }

        private async Task<List<VMItemWiseSales>> GetItemWiseSalesReturnList(DateTime date)
        {
            var ItemWiseSalesData = _saleReturnRepo.GetList()
                .Where(x => x.sal01date_eng.Date == date)
                .AsQueryable();

            var ItemWiseSalesItemRecords = _saleItemReturnRepo.GetList()
                .Include(x => x.pro02products)
                .AsQueryable();

            var itemWiseSalesRecord = ItemWiseSalesItemRecords
               .Join(ItemWiseSalesData,
                   record => record.sal02sal01uin,
                   summary => summary.sal01uin,
                   (record, summary) => new VMItemWiseSales
                   {
                       ID = summary.sal01uin,
                       ProductName = record.pro02products.pro02name_eng,
                       Rate = record.sal02rate,
                       Quantity = record.sal02qty,
                       Unit = record.un01units.un01name_eng,
                       Net_Amt = summary.sal01net_amt,
                       Ref_No = summary.sal01invoice_no,
                   })
               .ToList();
            return itemWiseSalesRecord;
        }
    }
}
