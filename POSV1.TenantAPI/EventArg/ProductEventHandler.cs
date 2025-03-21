using Microsoft.Extensions.Logging;
using POSV1.TenantAPI.Models;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Models.EntityModels.Accounting;
using POSV1.TenantModel.Repo.Interface.Accounting;

namespace POSV1.TenantAPI.EventArg
{
    public class ProductEventHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IGLedgersRepo _gledgersRepo;
        private readonly ILedgersRepo _LedgersRepo;

        public ProductEventHandler(
            IConfiguration configuration,
            IGLedgersRepo gLedgersRepo,
            ILedgersRepo ledgersRepo)
        {
            _configuration = configuration;
            _gledgersRepo = gLedgersRepo;
            EventManager.ProductCreated += OnProductCreated;
            _LedgersRepo = ledgersRepo;
        }

        private void OnProductCreated(object sender, ProductCreatedEventArgs e)
        {
            var createdProduct = e.CreatedProduct;

            var gLedger = _configuration["GeneralLedgerConfigurations:DefaultLedgerForNewProducts"];
            var GLDetail = _gledgersRepo.GetList().Where(x => x.led03title == gLedger).FirstOrDefault();

            var ledger_code = "p_" + createdProduct.pro02uin;

            //create ledger 
            var ledgerEntity = new led01ledgers
            {
                led01code = ledger_code,
                led01led03uin = GLDetail.led03uin,
                led01related_id = createdProduct.pro02uin,
                led01led05uin = (int)EnumLedgerTypes.Income,
                led01title = "Cash",
                led01desc = "IS Income",


                led01status = true,
                led01deleted = false,
                CreatedName = "Admin",
                DateCreated = DateTime.Now,
                UpdatedName = "Admin",
                DateUpdated = DateTime.Now,
                DeletedName = "",
                led01balance = 0

            };
            _LedgersRepo.Insert(ledgerEntity);
            _LedgersRepo.Save();
        }
    }
}
