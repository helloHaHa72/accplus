using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using POSV1.TenantAPI.Models;
using POSV1.TenantModel.Repo.Implementation;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantAPI.Controllers.Inventory
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorStatementController : ControllerBase
    {
        private readonly IPurchaseRepo _purchaseRepo;
        private readonly IPurchaseItemsRepo _purchaseItemsRepo;
        private readonly IVendorRepo _vendorRepo;
        public VendorStatementController(
            IPurchaseRepo purchaseRepo, 
            IPurchaseItemsRepo purchaseItemsRepo,
            IVendorRepo vendorRepo)
        {
            _purchaseRepo = purchaseRepo;
            _purchaseItemsRepo = purchaseItemsRepo;
            _vendorRepo = vendorRepo;
        }

        [HttpGet("VendorStatementSummary")]
        public async Task<IActionResult> VendorStatementSummary(int VendorID)
        {
            var vendorDetail = _vendorRepo.GetDetail(VendorID);
            if (vendorDetail == null)
            {
                throw new Exception("Vendor not found");
            }

            var purchaseRecords = _purchaseRepo.GetList().
                Where(x => x.pur01ven01uin == VendorID).
                ToList();

            var result = purchaseRecords.Select(x => new VMVendorStatement
            {
                Id = x.pur01uin,
                _txnDate = x.pur01date,
                Bill_No = x.pur01invoice_no,
                Total_Amt = x.pur01net_amt,
                Remarks = x.pur01remarks
            }).ToList();

            return Ok(result);
        }

        [HttpGet("VendorStatementDetail")]
        public async Task<IActionResult> VendorStatementDetail(int id)
        {
            var purchaseRecord = _purchaseRepo.GetDetail(id);
            var vendor = _vendorRepo.GetList().Where(x => x.ven01uin == purchaseRecord.pur01ven01uin).FirstOrDefault();

            VMVendorStatementSummary result =  new VMVendorStatementSummary()
            {
                VendorName = vendor.ven01name_eng,
                _txnDate = purchaseRecord.pur01date,
                Bill_No = purchaseRecord.pur01invoice_no,
                Remarks = purchaseRecord.pur01remarks
            };
            var _Que = _purchaseItemsRepo.GetList().Where(x => x.pur02pur01uin == purchaseRecord.pur01uin);

            result.VMVendorStatementDetail = _Que.Select(x => new VMVendorStatementDetail()
            {
                ID = x.pur02uin,
                ProductID = x.pur02pro02uin,
                ProductName = x.pro02products.pro02name_eng,
                Quantity = x.pur02qty,
                UnitID = x.pur02un01uin,
                UnitName = x.un01units.un01name_eng,
                Rate = x.pur02rate,
                Sub_Total = x.pur02amount,
                Batch_No = x.pur02batch_no,
                Disc_Amt = x.pur02disc_amt,
                Net_Amt = x.pur02net_amt
            })
                .ToList();

            return Ok(result);
        }

    }
}
