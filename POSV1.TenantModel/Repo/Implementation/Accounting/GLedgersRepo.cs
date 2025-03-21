using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using POSV1.TenantModel.Models.EntityModels.Accounting;
using POSV1.TenantModel.Repo.Interface.Accounting;

namespace POSV1.TenantModel.Repo.Implementation.Accounting
{
    public class GLedgersRepo :
        RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, led03general_ledgers, int>,
         IGLedgersRepo
    {
        private readonly MainDbContext _context;
        public GLedgersRepo(MainDbContext context) : base(context)
        {
            _context = context;
        }
        public override IQueryable<led03general_ledgers> FilterActive()
        {
            _Query = base.FilterActive().Where(x => x.led03status);
            return _Query;
        }
        public override IQueryable<led03general_ledgers> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }

        public Task<led03general_ledgers> GetGeneralLedgerByUinAsync(int uin)
        {
            throw new NotImplementedException();
        }
    }
}
