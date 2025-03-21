using Serilog.Sinks.MSSqlServer;
using Serilog;

namespace POSV1.TenantAPI.Startup;
public static class SerilogExtension
{
    public static void ConfigureSeriLog(
        this WebApplicationBuilder builder,
        IConfiguration configuration)
    {
        // Read connection string from appsettings.json
        var connectionString = configuration.GetConnectionString("LoggingDbContext");

        // Configure MSSqlServerSink options
        var sinkOptions = new MSSqlServerSinkOptions
        {
            TableName = "Logs", // Table name
            AutoCreateSqlTable = true, // Auto create table if it does not exist
            SchemaName = "SeriLog", // Database schema name
            BatchPostingLimit = 50, // Number of log events to include in each batch
            BatchPeriod = TimeSpan.FromSeconds(5), // Time to wait between checking for event batches
            EagerlyEmitFirstEvent = true // Emit the first event immediately rather than waiting for batchPostingLimit
        };
        Log.Logger = new LoggerConfiguration()
            //.Enrich.WithEnvironmentUserName()
            //.Enrich.WithMachineName()
            //.Enrich.WithProperty("IPAddress", GetIPAddress()) // Custom property for IP address
            .WriteTo.MSSqlServer(connectionString, sinkOptions: sinkOptions) // "Logs" is the name of the table to which logs will be written
            .CreateLogger();

        builder.Services.AddLogging(loggingBuilder =>
                  loggingBuilder.AddSerilog(dispose: true));
    }
}

