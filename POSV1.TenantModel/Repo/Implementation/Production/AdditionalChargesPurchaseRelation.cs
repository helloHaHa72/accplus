using POSV1.TenantModel.Models.EntityModels.Production;
using POSV1.TenantModel.Repo.Interface.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Implementation.Production
{
    public class AdditionalChargesPurchaseRelation : RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, add04chargepurchaserel, int>, IAdditionalChargesPurchaseRelation
    {
        private readonly MainDbContext _context;
        public AdditionalChargesPurchaseRelation(MainDbContext context) : base(context)
        {
            _context = context;
        }

        public override IQueryable<add04chargepurchaserel> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.add04isdeleted);
            return _Query;
        }
    }
}
