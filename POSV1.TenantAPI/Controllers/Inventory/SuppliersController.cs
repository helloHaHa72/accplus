using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSV1.TenantAPI.Models;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Implementation;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController :
        _AbsCRUDWithDiffInputModelController<SuppliersController, ISuppliersRepo, sup01suppliers, VMSupplier, VMSupplier, VMSupplier, int>
    {
        public SuppliersController(ILogger<SuppliersController> logger, ISuppliersRepo suppliersRepo, IMapper mapper)
            : base(logger, suppliersRepo, mapper)
        {
            
        }

        protected override IQueryable<VMSupplier> ProcessListData(IQueryable<sup01suppliers> data)
        {
            return data.Select(supplier => new VMSupplier
            {
                ID = supplier.sup01uin,
                Address = supplier.sup01address,
                TelPhone_No = supplier.sup01tel,
                Balance = supplier.sup01balance,
                RegNo = supplier.sup01regNo,
                RegDate = supplier.sup01regDate
            });
        }

        protected override VMSupplier ProcessDetailData(sup01suppliers data)
        {
            return new VMSupplier
            {
                ID = data.sup01uin,
                Address = data.sup01address,
                TelPhone_No = data.sup01tel,
                Balance = data.sup01balance,
                RegNo = data.sup01regNo,
                RegDate = data.sup01regDate,
            };
        }

        protected override sup01suppliers AssignValues(VMSupplier Data)
        {
            var supplierEntity = new sup01suppliers
            {
                sup01address = Data.Address,
                sup01tel = Data.TelPhone_No,
                sup01balance = Data.Balance,
                sup01regNo = Data.RegNo,
                sup01regDate = Data.RegDate,

                CreatedName = _ActiveUserName,
                DateCreated = DateTime.Now,
                UpdatedName = " ",
                DateUpdated = DateTime.Now,
                DeletedName = "",
                
            };
            return supplierEntity;
        }

        protected override void ReAssignValues(VMSupplier Data, sup01suppliers oldData)
        {
            oldData.sup01address = Data.Address;
            oldData.sup01tel = Data.TelPhone_No;
            oldData.sup01regDate = Data.RegDate;
            oldData.sup01regNo = Data.RegNo;
            oldData.sup01balance = Data.Balance;

            oldData.DateUpdated = DateTime.Now;
            oldData.UpdatedName = _ActiveUserName;
        }
    }
}
