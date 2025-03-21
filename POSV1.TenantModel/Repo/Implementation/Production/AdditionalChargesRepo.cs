using POSV1.TenantModel.Models.EntityModels.Production;
using POSV1.TenantModel.Repo.Interface.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Implementation.Production
{
    public class AdditionalChargesRepo : RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, add01additionalcharges, int>, IAdditionalChargesRepo
    {
        private readonly MainDbContext _context;
        public AdditionalChargesRepo(MainDbContext context) : base(context)
        {
            _context = context;
        }

        public override IQueryable<add01additionalcharges> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }
    }
}
