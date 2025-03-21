using Microsoft.Extensions.DependencyInjection;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Repo;
using POSV1.TenantModel.Repo.Implementation;
using POSV1.TenantModel.Repo.Implementation.Accounting;
using POSV1.TenantModel.Repo.Implementation.Estimate;
using POSV1.TenantModel.Repo.Implementation.Production;
using POSV1.TenantModel.Repo.Implementation.Settings;
using POSV1.TenantModel.Repo.Interface;
using POSV1.TenantModel.Repo.Interface.Accounting;
using POSV1.TenantModel.Repo.Interface.Estimate;
using POSV1.TenantModel.Repo.Interface.Production;
using POSV1.TenantModel.Repo.Interface.Settings;

namespace POSV1.TenantModel
{
    public static class StartupService
    {
        public static void ConfigureTenantModelServices(this IServiceCollection services)
        {
            //// Get all repository types from the assembly
            //var repositoryTypes = Assembly.GetExecutingAssembly()
            //                              .GetTypes()
            //                              .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGeneralRepositories<,>)));

            //// Register repositories
            //foreach (var repositoryType in repositoryTypes)
            //{
            //    var interfaceType = repositoryType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGeneralRepositories<,>));
            //    services.AddScoped(interfaceType, repositoryType);
            //}


            services.AddScoped<IProductCategoriesRepo, ProductCategoriesRepo>();
            services.AddScoped<IProductsRepo, ProductsRepo>();

            services.AddScoped<IProductionRepo, ProductionRepo>();
            services.AddScoped<IConsumeRawProductRepo, ConsumeRawProductRepo>();
            services.AddScoped<IProductionStatusLog, ProductionStatusLog>();
            services.AddScoped<IFinalProductRepo, FinalProductRepo>();
            services.AddScoped<IAdditionalChargesRepo, AdditionalChargesRepo>();
            services.AddScoped<IPurchaseAdditionalCharges, PurchaseAdditionalCharges>();
            services.AddScoped<IAdditionalChargesPurchaseRelation, AdditionalChargesPurchaseRelation>();

            services.AddScoped<IProductUnitsRepo, ProductUnitsRepo>();
            services.AddScoped<IUnitsRepo, UnitsRepo>();

            services.AddScoped<ICustomersRepo, CustomersRepo>();
            services.AddScoped<ICustomerTypeRepo, CustomerTypeRepo>();

            services.AddScoped<IVendorRepo, VendorRepo>();

            services.AddScoped<ISaleItemsRepo, SaleItemsRepo>();
            services.AddScoped<ISalesItemReturnRepo, SalesItemReturnRepo>();

            services.AddScoped<ISalesRepo, SalesRepo>();
            services.AddScoped<ISaleReturnRepo, SaleReturnRepo>();

            services.AddScoped<IPurchaseRepo, PurchaseRepo>();
            services.AddScoped<IPurchaseReturnRepo, PurchaseReturnRepo>();

            services.AddScoped<ICashSettlementRepo, CashSettlementRepo>();

            services.AddScoped<IPurchaseItemsRepo, PurchaseitemsRepo>();
            services.AddScoped<IPurchasereturnitemsRepo, PurchasereturnitemsRepo>();

            services.AddScoped<ISuppliersRepo, SuppliersRepo>();
            services.AddScoped<ILedgersRepo, LedgersRepo>();
            services.AddScoped<IGLedgersRepo, GLedgersRepo>();
            services.AddScoped<ILedgerTypesRepo, LedgerTypesRepo>();
            services.AddScoped<IVoucherSummaryRepo, VoucherSummaryRepo>();
            services.AddScoped<IVoucherDetailsRepo, VoucherDetailsRepo>();
            services.AddScoped<IVoucherLogRepo, VoucherLogRepo>();

            services.AddScoped<IConfigurationSettings, ConfigurationSettingsRepo>();
            services.AddScoped<IMainSetupRepo, MainSetupRepo>();

            services.AddScoped<IAccessListRepo, AccessListRepo>();
            services.AddScoped<IUserPermissionListRepo, UserPermissionListRepo>();

            services.AddScoped<IConfigRepo, ConfigRepo>();

            services.AddScoped<ISeriLogRepository, SeriLogRepository>();
            services.AddScoped<IOutGoingApiRequestRepository, OutGoingApiRequestRepository>();

            //AuthDbSetup
            //services.AddScoped<ITenantInfoRepo, TenantInfoRepo>();

            services.AddScoped<IEstimateRepo, EstimateRepo>();

            #region configuration
            services.AddScoped<IConfigValuesRepo, ConfigValuesRepo>();
            services.AddScoped<IConfigValuesByEnumRepo, ConfigValuesByEnumRepo>();
            #endregion
            // Add other services and configurations to the container
            // ...
        }

    }


}