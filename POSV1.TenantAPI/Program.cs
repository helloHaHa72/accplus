using POSV1.TenantAPI.Startup;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();

//configure appsettings 
await builder.ConfigureAppSettings();
ConfigurationManager configuration = builder.Configuration;
builder.Services.AddHttpContextAccessor();

await builder.Startup(configuration);

var app = builder.Build();

await app.AppStart(configuration, builder);

app.Run();
