using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Models.EntityModels.Production;
using POSV1.TenantModel.Repo.Interface.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Implementation.Production
{
    public class ConsumeRawProductRepo : RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, prod02consumerawproduct, int>, IConsumeRawProductRepo
    {
        private readonly MainDbContext _context;
        public ConsumeRawProductRepo(MainDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task RemoveRangeAsync(IEnumerable<prod02consumerawproduct> products)
        {
            if (products == null || !products.Any())
                return;

            _context.Prod02Consumerawproducts.RemoveRange(products);
            await _context.SaveChangesAsync();
        }

    }
}
