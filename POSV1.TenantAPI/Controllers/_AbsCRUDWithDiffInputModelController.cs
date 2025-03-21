using System.Security.Claims;
using AutoMapper;
using BaseAppSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;
using RepoBaseModelCore;


namespace POSV1.TenantAPI.Controllers
{
    //[Route("[controller]")]
    public abstract class _AbsCRUDWithDiffInputModelController<ChildController, MainRepo, MainEntity, ReqModel, ListVMModel, DetailVMModel, PKType>
        : ControllerBase
        where ChildController : ControllerBase
        where MainRepo : IGeneralRepositories<MainEntity, PKType>
        where MainEntity : Auditable
        where ReqModel : class
        where ListVMModel : class
        where DetailVMModel : class
    {
        protected readonly ILogger<ChildController> _logger;
        protected readonly MainRepo _MainRepo;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        #region AuthUser
        public ClaimsPrincipal _ActiveUser => HttpContext.User;
        public string _ActiveUserName => _ActiveUser.Identity == null ? "Admin" : _ActiveUser.Identity.Name;
        #endregion
        public _AbsCRUDWithDiffInputModelController(ILogger<ChildController> log, MainRepo repo, IMapper mapper)
        {
            _logger = log;
            _MainRepo = repo;
            _mapper = mapper;
        }

        protected abstract IQueryable<ListVMModel> ProcessListData(IQueryable<MainEntity> data);


        // create custom attribute and inherit the authorization where we check the actions assigned to that active user role
        //[HttpGet("GetList")]
        //public virtual async Task<IEnumerable<ListVMModel>> GetList()
        //{
        //    //var temp = HttpContext;
        //    var _que = _MainRepo.GetList()  //get the data from db
        //        .AsNoTracking();

        //    var processQuery = ProcessListData(_que); // proecss the data as per the view model

        //    //noe load explicit using loop


        //    var resultList = await processQuery.ToListAsync();
        //    return resultList;
        //}

        public class PageResult<T>
        {
            public IEnumerable<T> Data { get; set; }
            public int TotalPages { get; set; }
            public int PageSize { get; set; }
            public int TotalData { get; set; }
            public int CurrentPage { get; set; }
        }


        [HttpGet("GetList")]
        public virtual async Task<ActionResult<PageResult<ListVMModel>>> GetList(int? pageNumber, int? pageSize)
        {
            try
            {
                pageNumber ??= 1;
                pageSize ??= 10;

                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest("Invalid page number or page size.");
                }

                var query = _MainRepo.GetList().AsNoTracking().OrderByDescending(x => x.DateCreated);
                var totalCount = await query.CountAsync();

                // Apply pagination before processing the data
                var pagedQuery = query
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);

                var processedQuery = ProcessListData(pagedQuery); // Ensure this does not materialize the query prematurely

                var resultList = await processedQuery.ToListAsync();

                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize.Value);

                var pageResult = new PageResult<ListVMModel>
                {
                    Data = resultList,
                    TotalPages = totalPages,
                    PageSize = pageSize.Value,
                    TotalData = totalCount,
                    CurrentPage = pageNumber.Value
                };

                return Ok(pageResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching data.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching data.");
            }
        }



        [HttpPost("Create")]
        public virtual async Task<IActionResult> Create(ReqModel Data)
        {
            try
            {
                MainEntity newData = AssignValues(Data);
                _MainRepo.Insert(newData);
                await _MainRepo.SaveAsync();
                return Ok("Created Succesfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        protected abstract MainEntity AssignValues(ReqModel Data);


        [HttpPatch("Update")]
        public async Task<IActionResult> Update(PKType id, ReqModel Data)
        {
            try
            {

                var oldData = await _MainRepo.GetDetailAsync(id);
                if (oldData == null) { throw new Exception("Invalid  ID"); }

                ReAssignValues(Data, oldData);

                _MainRepo.Update(oldData);
                await _MainRepo.SaveAsync();
                await AfterUpdate(oldData);
                //if(ChildController is SalesController)
                //{

                //}

                return Ok("Data Updated Successfully.");

            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message);
            }
        }
        protected virtual async Task AfterUpdate(MainEntity data)
        {
            return;
        }
        protected virtual void ReAssignValues(ReqModel Data, MainEntity oldData)
        {
            return;
        }

        protected abstract DetailVMModel ProcessDetailData(MainEntity data);

        [HttpGet("GetDetail")]
        public virtual async Task<DetailVMModel> GetDetail(PKType id)
        {
            var _que = await _MainRepo.GetDetailAsync(id);  //get the data from db

            var result = ProcessDetailData(_que); // proecss the data as per the view model

            return result;

        }

        [HttpDelete("Delete")]
        public virtual async Task<IActionResult> Delete(PKType id)
        {
            var oldData = await _MainRepo.GetDetailAsync(id);
            if (oldData == null) { throw new Exception("Invalid  ID"); }
            oldData.DateDeleted = DateTime.UtcNow;

            _MainRepo.Update(oldData);
            await _MainRepo.SaveAsync();
            return Ok("Data Deleted Sucessfully");

        }
    }
}