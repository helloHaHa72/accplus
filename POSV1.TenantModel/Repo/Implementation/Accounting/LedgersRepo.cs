using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using POSV1.TenantModel.Models.EntityModels.Accounting;
using POSV1.TenantModel.Repo.Interface.Accounting;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using POSV1.TenantModel.Repo.Interface;
using POSV1.TenantModel.Models;

namespace POSV1.TenantModel.Repo.Implementation.Accounting
{
    public class LedgersRepo:
        RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, led01ledgers, int>,
         ILedgersRepo
    {
        private readonly MainDbContext _context;
        public LedgersRepo(    MainDbContext context) : base(context)
        {
            _context = context;
        }

        public override IQueryable<led01ledgers> FilterActive()
        {
            _Query = base.FilterActive().Where(x => x.led01status);
            return _Query;
        }
        public override IQueryable<led01ledgers> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }
        
        public async Task<led05ledger_types> GetLedgerTypeByTitleAsync(string title)
        {
            return await _context.Set<led05ledger_types>()
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.led05title == title);
        }

        public async Task<led03general_ledgers> GetGeneralLedgerByUinAsync(int uin)
        {
            return await _context.Set<led03general_ledgers>()
                 .AsNoTracking()
                 .FirstOrDefaultAsync(l => l.led03uin == uin);
        }
    }
}
