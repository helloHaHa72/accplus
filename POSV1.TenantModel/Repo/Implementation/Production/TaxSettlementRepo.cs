using POSV1.TenantModel.Models;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface.ERP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Implementation.ERP
{
    public class TaxSettlementRepo : RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, ta01taxsettlement, int>, ITaxSettlementRepo
    {
        private readonly MainDbContext _context;
        public TaxSettlementRepo(MainDbContext context) : base(context)
        {
            _context = context;
        }

        public override IQueryable<ta01taxsettlement> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }

        public override IQueryable<ta01taxsettlement> GetList(bool IncludeDeleted = false)
        {
            var query = base.GetList(IncludeDeleted);

            if (!IncludeDeleted)
            {
                query = query.Where(c => !c.DateDeleted.HasValue);
            }

            // Example filter for specific conditions if needed
            // query = query.Where(c => c.Name != "SomeCountryName");

            return query;
        }
    }
}
