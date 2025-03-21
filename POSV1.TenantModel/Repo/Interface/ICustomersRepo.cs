using POSV1.TenantModel.Models.EntityModels.Inventory;

namespace POSV1.TenantModel.Repo.Interface
{
    public interface ICustomersRepo : RepoBaseModelCore.IGeneralRepositories<cus01customers, int>
    {
        string GetCustomerName(int customerId);
    }
}
