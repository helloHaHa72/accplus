using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSV1.TenantAPI.Models;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemWiseTxnReport : ControllerBase
    {
        private readonly ISaleItemsRepo _saleItemsRepo;
        private readonly ISalesRepo _salesRepo;
        private readonly IPurchaseItemsRepo _purchaseItemsRepo;
        private readonly IPurchaseRepo _purchaseRepo;
        private readonly IProductsRepo _productsRepo;
        private readonly IUnitsRepo _unitsRepo;
        public ItemWiseTxnReport(
            ISaleItemsRepo saleItemsRepo,
            ISalesRepo salesRepo,
            IProductsRepo productsRepo,
            IUnitsRepo unitsRepo,
            IPurchaseRepo purchaseRepo,
            IPurchaseItemsRepo purchaseItemsRepo
            )
        {
            _saleItemsRepo = saleItemsRepo;
            _salesRepo = salesRepo;
            _productsRepo = productsRepo;
            _unitsRepo = unitsRepo;
            _purchaseRepo = purchaseRepo;
            _purchaseItemsRepo = purchaseItemsRepo;
        }

        [HttpGet("ItemTxnDetail")]
        public async Task<IActionResult> ItemTxnDetail(int Item_id)
        {
            var productDetail = _productsRepo.GetDetail(Item_id);
            if (productDetail == null)
            {
                throw new Exception("Item Id not found");
            }
            var unitName = _unitsRepo
                .GetList()
                .Where(x => x.un01uin == productDetail.pro02un01uin)
                .Select(x => x.un01name_eng)
                .FirstOrDefault();

            var salesData = _salesRepo.GetList();
            var purchaseData = _purchaseRepo.GetList();

            var saleItemRecords = _saleItemsRepo.GetList()
                .Where(detail => detail.sal02pro02uin == Item_id)
                .ToList();

            var purchaseItemRecords = _purchaseItemsRepo.GetList()
                .Where(detail => detail.pur02pro02uin == Item_id)
                .ToList();

            var saleResults = saleItemRecords
                .Join(salesData,
                    record => record.sal02sal01uin,
                    summary => summary.sal01uin,
                    (record, summary) => new VMItemWiseTxnReport
                    {
                        //ID = record.sal02uin,
                        ID = summary.sal01uin,
                        Type = "Sale", 
                        _Txn_Date = summary.sal01date_eng,
                        Ref_No = summary.sal01invoice_no,
                        Rate = record.sal02rate,
                        Quantity = record.sal02qty,
                        Unit = unitName,
                        Net_Amt = record.sal02net_amt,
                    });

            var purchaseResults = purchaseItemRecords
                .Join(purchaseData,
                    record => record.pur02pur01uin,
                    summary => summary.pur01uin,
                    (record, summary) => new VMItemWiseTxnReport
                    {
                        //ID = record.pur02uin,
                        ID = summary.pur01uin,
                        Type = "Purchase", 
                        _Txn_Date = summary.pur01date,
                        Ref_No = summary.pur01invoice_no,
                        Rate = record.pur02rate,
                        Quantity = record.pur02qty,
                        Unit = unitName,
                        Net_Amt = (double)record.pur02net_amt,
                    });

            var result = saleResults.Concat(purchaseResults).ToList();

            return Ok(result);
        }

        [HttpGet("TodaysItemTxnDetail")]
        public async Task<IActionResult> TodaysItemTxnDetail(int Item_id)
        {
            var productDetail = _productsRepo.GetDetail(Item_id);
            if (productDetail == null)
            {
                throw new Exception("Item Id not found");
            }
            var unitName = _unitsRepo
                .GetList()
                .Where(x => x.un01uin == productDetail.pro02un01uin)
                .Select(x => x.un01name_eng)
                .FirstOrDefault();

            var todayDate = DateTime.Now.Date;

            var salesData = _salesRepo.GetList().Where(s => s.sal01date_eng.Date == todayDate).ToList();
            var purchaseData = _purchaseRepo.GetList().Where(s => s.pur01date.Date == todayDate).ToList();

            var saleItemRecords = _saleItemsRepo.GetList()
                .Where(detail => detail.sal02pro02uin == Item_id)
                .ToList();

            var purchaseItemRecords = _purchaseItemsRepo.GetList()
                .Where(detail => detail.pur02pro02uin == Item_id)
                .ToList();

            var saleResults = saleItemRecords
                .Join(salesData,
                    record => record.sal02sal01uin,
                    summary => summary.sal01uin,
                    (record, summary) => new VMItemWiseTxnReport
                    {
                        ID = record.sal02uin,
                        Type = "Sale",
                        _Txn_Date = summary.sal01date_eng,
                        Ref_No = summary.sal01invoice_no,
                        Rate = record.sal02rate,
                        Quantity = record.sal02qty,
                        Unit = unitName,
                        Net_Amt = record.sal02net_amt,
                    });

            var purchaseResults = purchaseItemRecords
                .Join(purchaseData,
                    record => record.pur02pur01uin,
                    summary => summary.pur01uin,
                    (record, summary) => new VMItemWiseTxnReport
                    {
                        ID = record.pur02uin,
                        Type = "Purchase",
                        _Txn_Date = summary.pur01date,
                        Ref_No = summary.pur01invoice_no,
                        Rate = record.pur02rate,
                        Quantity = record.pur02qty,
                        Unit = unitName,
                        Net_Amt = (double)record.pur02net_amt,
                    });

            var result = saleResults.Concat(purchaseResults).ToList();

            return Ok(result);
        }

    }
}
