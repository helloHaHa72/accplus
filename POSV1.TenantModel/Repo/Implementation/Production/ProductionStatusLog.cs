using POSV1.TenantModel.Models.EntityModels.Production;
using POSV1.TenantModel.Repo.Interface.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Implementation.Production
{
    public class ProductionStatusLog : RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, prod03statuslog, int>, IProductionStatusLog
    {
        public ProductionStatusLog(MainDbContext context) : base(context)
        {
            
        }
    }
}
