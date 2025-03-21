using POSV1.TenantModel.Models.EntityModels.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Interface.Production
{
    public interface IFinalProductRepo : RepoBaseModelCore.IGeneralRepositories<prod04finalproducts, int>
    {
    }
}
