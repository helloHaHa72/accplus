using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSV1.TenantAPI.Models;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantAPI.Controllers.Inventory
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerStatementController : ControllerBase
    {
        private readonly ISalesRepo _salesRepo;
        private readonly ISaleItemsRepo _saleItemsRepo;
        private readonly ICustomersRepo _customersRepo;
        public CustomerStatementController(
            ISalesRepo salesRepo,
            ISaleItemsRepo saleItemsRepo,
            ICustomersRepo customersRepo)
        {
            _salesRepo = salesRepo;
            _saleItemsRepo = saleItemsRepo;
            _customersRepo = customersRepo;
        }

        [HttpGet("CustomerStatementSummary")]
        public async Task<IActionResult> CustomerStatementSummary(int CustomerID)
        {
            var customerDetail = _customersRepo.GetDetail(CustomerID);
            if (customerDetail == null)
            {
                throw new Exception("Customer not found");
            }

            var saleRecords = _salesRepo
                .GetList()
                .Where(s => s.sal01cus01uin == CustomerID)
                .ToList();

            var result = saleRecords.Select(x => new VMCustomerStatement
            {
                ID = x.sal01uin,
                _txnDate = x.sal01date_eng,
                Bill_No = x.sal01invoice_no,
                Total_Amt = x.sal01net_amt,
                Remarks = x.sal01remarks
            }).ToList();

            return Ok(result);
        }

        [HttpGet("CustomerStatementDetail")]
        public async Task<IActionResult> CustomerStatementDetail(int id)
        {
            var saleRecord = _salesRepo.GetDetail(id);
            var customerEntity =await _customersRepo.GetDetailAsync(saleRecord.sal01cus01uin);

            VMCustomerStatementSummary result = new VMCustomerStatementSummary()
            {
                CustomerName = customerEntity.cus01name_eng,
                _txnDate = saleRecord.sal01date_eng,
                Bill_No = saleRecord.sal01invoice_no,
                Remarks = saleRecord.sal01remarks
            };
            var _Que = _saleItemsRepo.GetList().Where(x => x.sal02sal01uin == saleRecord.sal01uin);

            result.VMCustomerStatementDetail = _Que.Select(x => new VMCustomerStatementDetail()
            {
                ID = x.sal02uin,
                ProductID = x.sal02pro02uin,
                ProductName = x.pro02products.pro02name_eng,
                Quantity = x.sal02qty,
                UnitID = x.sal02un01uin,
                UnitName = x.un01units.un01name_eng,
                Rate = x.sal02rate,
                Sub_Total = x.sal02sub_total,
                Disc_Amt = x.sal02disc_amt,
                Net_Amt = x.sal02net_amt
            })
                .ToList();

            return Ok(result);
        }

    }
}
