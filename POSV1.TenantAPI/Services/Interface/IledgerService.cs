using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Models.EntityModels.Production;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Models.EntityModels.Accounting;
using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantAPI.Services
{
    public interface IledgerService
    {
        void OnProductCreated(pro02products createdProduct);
        void OnCustomerCreated(cus01customers createdCustomer);
        void OnVendorCreated(ven01vendors createdVendor);
        //void OnEmployeeCreated(emp01employees createdEmployee);
        Task<bool> UpdateLedgerOnSale(sal01sales salesData);
        Task<bool> UpdateLedgerOnPurhcase(pur01purchases purchaseData);
        //Task<bool> UpdateLedgerOnEmpTxn(tran02transaction_details txnData,decimal BalanceCalculator);
        Task<bool> CreateLedgerOnSaleDisc(sal01sales data);
        Task<bool> CreateLedgerOnPurchaseDisc(pur01purchases data);
        Task<bool> UpdateLedgerOnPurchaseReturn(pur01purchasereturns returnData);
        Task<List<LedgerDto>> FetchDefaultLedgerDate(string data);
        Task<bool> UpdateLedgerOnAdditionalCharge(List<ChargeData> data);
        Task<IEnumerable<VMUserStatement>> GetStatement(string ledgerCode);
    }
}
