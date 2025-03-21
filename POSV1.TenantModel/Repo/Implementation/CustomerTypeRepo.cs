using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;
using RepoBaseModelCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Implementation
{
    public class CustomerTypeRepo : _AbsGeneralRepositories<MainDbContext, cus02customerType, int>, ICustomerTypeRepo
    {
        private readonly MainDbContext _context;
        public CustomerTypeRepo(MainDbContext context) : base(context)
        {
            _context = context;
        }

        public override IQueryable<cus02customerType> GetList(bool IncludeDeleted = false)
        {
            var query = base.GetList(IncludeDeleted);

            if (!IncludeDeleted)
            {
                query = query.Where(c => c.DateDeleted == null);
            }

            // Example filter for specific conditions if needed
            // query = query.Where(c => c.Name != "SomeCountryName");

            return query;
        }
    }
}
