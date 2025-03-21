using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POSV1.TenantAPI.Models;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;

namespace POSV1.TenantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnitsController :
        _AbsCRUDWithDiffInputModelController<UnitsController, IUnitsRepo, un01units,VMUnit,VMUnit,VMUnit, int>
    {
        public UnitsController(ILogger<UnitsController> logger, IUnitsRepo ProductRepo, IMapper mapper)
            : base(logger, ProductRepo, mapper)
        {
        }

        protected override IQueryable<VMUnit> ProcessListData(IQueryable<un01units> data)
        {
            return data.Select(unit => new VMUnit
            {
                ID = unit.un01uin,
                //Name = unit.un01name_eng,
                Name = string.IsNullOrEmpty(unit.un01desc) ? unit.un01name_eng : $"{unit.un01name_eng} ({unit.un01desc})",
                Status = unit.un01status,
                Ratio = unit.un01ratio, 
                Dexcription = unit.un01desc
            });
        }

        protected override VMUnit ProcessDetailData(un01units data)
        {
            return new VMUnit
            {
                ID = data.un01uin,
                Name = data.un01name_eng,
                Status = data.un01status,
                Ratio = data.un01ratio,
                Dexcription = data.un01desc
            };
        }

        protected override un01units AssignValues(VMUnit Data)
        {
            var unitEntity = new un01units
            {
                un01name_eng = Data.Name,
                un01status = Data.Status,
                un01ratio = Data.Ratio,
                un01desc = Data.Dexcription,

                CreatedName = _ActiveUserName,
                DateCreated = DateTime.Now,
                UpdatedName = " ",
                DateUpdated = DateTime.Now,
                DeletedName = "",
                un01name_nep = "श"
            };
            return unitEntity;
        }

        protected override void ReAssignValues(VMUnit Data, un01units oldData)
        {
            oldData.un01name_nep = "श";
            oldData.un01name_eng = Data.Name;
            oldData.un01status = Data.Status;
            oldData.un01ratio = Data.Ratio;
            oldData.un01desc = Data.Dexcription;

            oldData.DateUpdated = DateTime.Now;
            oldData.UpdatedName = _ActiveUserName;
        }
    }
}