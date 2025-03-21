using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;
using RepoBaseModelCore;

namespace POSV1.TenantModel.Repo.Implementation
{
    public interface _IAbsGeneralRepo<MainEntity, PKType> : IGeneralRepositories<MainEntity, PKType>
        where MainEntity : class
    {
        bool ExclicitLoad<ChildType>(MainEntity Data);
    }
    public class CustomersRepo :
     _AbsGeneralRepositories<MainDbContext, cus01customers, int>,
        ICustomersRepo
    {
        private readonly MainDbContext _context;
        public CustomersRepo(MainDbContext context) : base(context)
        {
            _context = context;
        }

        public override IQueryable<cus01customers> GetList(bool IncludeDeleted = false)
        {
            return base.GetList(IncludeDeleted).Include(x => x.CustomerType);
        }
        public override async Task<cus01customers> GetDetailAsync(int id)
        {
            var query = base.GetList();

            if (query.Any(x => x.cus01uin == id && x.CustomerType != null))
            {
                query = query.Include(x => x.CustomerType);
            }

            var customer = await query.FirstOrDefaultAsync(x => x.cus01uin == id);

            return customer;
        }


        //public override IQueryable<cus01customers> FilterActive()
        //{
        //    _Query = base.FilterActive().Where(x => x.cus01status);
        //    return _Query;
        //}
        public override IQueryable<cus01customers> FilterDeleted()
        {
            _Query = base.FilterActive().Where(x => !x.DateDeleted.HasValue);
            return _Query;
        }

        public string GetCustomerName(int customerId)
        {
            var customer = _context.cus01customers
                                 .FirstOrDefault(x => x.cus01uin == customerId && x.cus01status);

            if (customer != null)
            {
                return customer.cus01name_eng;
            }

            return null;
        }

    }
}
