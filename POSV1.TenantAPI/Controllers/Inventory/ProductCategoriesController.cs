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
    public class ProductCategoriesController :
        _AbsCRUDWithDiffInputModelController<ProductCategoriesController, IProductCategoriesRepo, pro01categories,VMProductCategory,VMProductCategory,VMProductCategory,int>
    {
        public ProductCategoriesController(ILogger<ProductCategoriesController> logger, IProductCategoriesRepo ProductRepo, IMapper mapper)
            : base(logger, ProductRepo, mapper)
        {
        }

        protected override IQueryable<VMProductCategory> ProcessListData(IQueryable<pro01categories> data)
        {
            return data.Select(category => new VMProductCategory
            {
                ID = category.pro01uin,
                Name = category.pro01name_eng,
                Code = category.pro01code,
                Status = category.pro01status,
            });
        }

        protected override VMProductCategory ProcessDetailData(pro01categories data)
        {
            return new VMProductCategory
            {
                ID = data.pro01uin,
                Name = data.pro01name_eng,
                Code = data.pro01code,
                Status = data.pro01status
            };
        }

        protected override pro01categories AssignValues(VMProductCategory Data)
        {
            var categoryEntity = new pro01categories
            {
                pro01name_eng = Data.Name,
                pro01code = Data.Code,
                pro01status = Data.Status,

                CreatedName = _ActiveUserName,
                DateCreated = DateTime.Now,
                UpdatedName = " ",
                DateUpdated = DateTime.Now,
                DeletedName = " ",
                pro01name_nep = "रु"
            };
            return categoryEntity;
        }
        protected override void ReAssignValues(VMProductCategory Data, pro01categories oldData)
        {
            oldData.pro01name_nep = "रु";
            oldData.pro01name_eng = Data.Name;
            oldData.pro01status = Data.Status;
            oldData.pro01code = Data.Code;

            oldData.DateUpdated = DateTime.Now;
            oldData.UpdatedName = _ActiveUserName;
        }
    }
}