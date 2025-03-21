using BaseAppSettings.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection.Emit;

namespace BaseAppSettings
{
    public partial class LoggingDbContext : DbContext
    {
        public DbSet<ApiLog> ApiLogs { get; set; }
        //public DbSet<Logs> Logs { get; set; }
        public DbSet<SerilogRecords> Logs { get; set; }

        public LoggingDbContext(DbContextOptions<LoggingDbContext> options) : base(options)
        {
        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=DefaultConnection");*/

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("LoggingDbContext");
            }

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Customize the entity mapping if needed
            modelBuilder.Entity<SerilogRecords>(entity =>
            {
                entity.ToTable("Logs", "SeriLog");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Message).HasColumnName("Message");
                entity.Property(e => e.MessageTemplate).HasColumnName("MessageTemplate");
                entity.Property(e => e.Level).HasColumnName("Level");
                entity.Property(e => e.TimeStamp).HasColumnName("TimeStamp");
                entity.Property(e => e.Exception).HasColumnName("Exception");
                entity.Property(e => e.Properties).HasColumnName("Properties");
            });
        }

        public async Task LogApiCall(string applicationName,
            string path,
            string requestBody, string requestHeaders,
            string response, string responseHeaders,
            int responseStatusCode,
            string httpMethod)
        {
            await LogApiCall(new ApiLog()
            {
                ApplicationName = applicationName,
                Path = path,
                HttpMethod = httpMethod,
                RequestBody = requestBody,
                RequestHeaders = requestHeaders,
                ResponseBody = response,
                ResponseHeaders = responseHeaders,
                ResponseStatusCode = responseStatusCode,
            });
        }
        public async Task LogApiCall(ApiLog Data)
        {
            var saveLogs = BaseAppSettings.GeneralAppConfig.SaveLogs;

            if (saveLogs == true)
            {
                this.ApiLogs.Add(Data);
                await this.SaveChangesAsync();
            }
        }
    }
}