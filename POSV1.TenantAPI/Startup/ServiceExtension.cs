using POSV1.TenantAPI.EventArg;
using POSV1.TenantAPI.Services.Implementation;
using POSV1.TenantAPI.Services.Interface;
using POSV1.TenantAPI.Services;
using BaseAppSettings.Middlewares;
using POSV1.TenantModel;

namespace POSV1.TenantAPI.Startup;

public static class ServiceExtension
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var service = builder.Services;

        service.AddScoped<IAuthService, AuthService>();
        service.AddScoped<ProductEventHandler>();
        service.AddScoped<IledgerService, LedgerService>();
        service.AddScoped<IVoucherService, VoucherService>();
        service.AddScoped<IGlobalService, GlobalService>();
        service.AddScoped<IItemTransactionService, ItemTransactionService>();
        service.AddScoped<MigrationStatusService>();
        service.AddTransient<OutgoingRequestLoggingHandler, OutgoingRequestLoggingHandler>();

    }

}
