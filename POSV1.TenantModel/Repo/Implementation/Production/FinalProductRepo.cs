using POSV1.TenantModel.Models.EntityModels.Production;
using POSV1.TenantModel.Repo.Interface.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Implementation.Production
{
    public class FinalProductRepo : RepoBaseModelCore._AbsGeneralRepositories<MainDbContext, prod04finalproducts, int>, IFinalProductRepo
    {
        public FinalProductRepo(MainDbContext context) : base(context)
        {
        }
    }
}
