using BaseAppSettings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo
{
    public interface IOutGoingApiRequestRepository
    {
        IQueryable<ApiLog> GetList(bool trackEntity);
        Task<ApiLog> GetById(long id);
        Task Delete(long id);
        Task TruncateTable(string tableName);
    }

    public class OutGoingApiRequestRepository : IOutGoingApiRequestRepository
    {
        private readonly LoggingDbContext _context;
        public OutGoingApiRequestRepository(LoggingDbContext context)
        {
            _context = context;
        }

        public IQueryable<ApiLog> GetList(bool trackEntity)
        {
            IQueryable<ApiLog> query = _context.ApiLogs;

            if (!trackEntity)
            {
                query = query.AsNoTracking();
            }

            // Order by ID in descending order and take the last 100 records
            query = query
                .OrderByDescending(apiLog => apiLog.Id)
                .Take(100);

            return query;
        }

        public async Task<ApiLog> GetById(long id)
        {
            return await _context.ApiLogs.FindAsync(id);
        }

        public async Task Delete(long id)
        {
            var data = await _context.ApiLogs.FirstAsync(x => x.Id == id);
            _context.ApiLogs.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task TruncateTable(string tableName)
        {
            await _context.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {tableName};");
        }
    }
}
