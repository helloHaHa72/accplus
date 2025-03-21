using BaseAppSettings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo
{
    public interface ISeriLogRepository
    {
        IQueryable<SerilogRecords> GetList(bool trackEntity = false);
        Task<SerilogRecords> GetById(int id);
        Task Delete(long id);
        Task TruncateTable(string tablename);
    }
    public class SeriLogRepository : ISeriLogRepository
    {
        private readonly LoggingDbContext _context;
        public SeriLogRepository(LoggingDbContext context)
        {
            _context = context;
        }

        public IQueryable<SerilogRecords> GetList(bool trackEntity = false)
        {
            IQueryable<SerilogRecords> query = _context.Logs;

            if (!trackEntity)
            {
                query = query.AsNoTracking();
            }

            // Order by ID in descending order and take the last 100 records
            return query.OrderByDescending(apiLog => apiLog.Id)
                .Select(apiLog => new SerilogRecords
                {
                    Id = apiLog.Id,
                    Message = apiLog.Message,
                    MessageTemplate = apiLog.MessageTemplate,
                    Level = apiLog.Level,
                    TimeStamp = apiLog.TimeStamp,
                    Exception = apiLog.Exception ?? "",
                    Properties = apiLog.Properties
                });
        }

        public async Task<SerilogRecords> GetById(int id)
        {
            SerilogRecords record = await _context.Logs
                .Where(apiLog => apiLog.Id == id)
                .Select(apiLog => new SerilogRecords
                {
                    Id = apiLog.Id,
                    Message = apiLog.Message,
                    MessageTemplate = apiLog.MessageTemplate,
                    Level = apiLog.Level,
                    TimeStamp = apiLog.TimeStamp,
                    Exception = apiLog.Exception ?? "",
                    Properties = apiLog.Properties
                })
                .FirstOrDefaultAsync();

            return record;
        }

        public async Task Delete(long id)
        {
            var data = await _context.Logs.FirstAsync(x => x.Id == id);
            if (data != null)
            {
                _context.Logs.Remove(data);
                await _context.SaveChangesAsync();
            };
        }

        public async Task TruncateTable(string tableName)
        {
            await _context.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {tableName};");
        }

        /*public async Task<SerilogRecords> GetById(int id)
    {
        SerilogRecords record = await _context.Logs.FindAsync(id);

        if (record != null)
        {
            record.Exception = record.Exception ?? "";
        }

        return record;
    }*/
    }
}
