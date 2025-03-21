using BaseAppSettings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel;
using RepoBaseModelCore;

namespace POSV1.TenantAPI.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public abstract class _AbsCRUDController<ChildController, MainRepo, MainEntity, PKType>
        : ControllerBase
        where ChildController : ControllerBase
        where MainRepo : IGeneralRepositories<MainEntity, PKType>
        where MainEntity : Auditable
    {
        protected readonly ILogger<ChildController> _logger;
        protected readonly MainRepo _MainRepo;
        public _AbsCRUDController(ILogger<ChildController> log, MainRepo repo)
        {
            _logger = log;
            _MainRepo = repo;
        }
        [HttpGet("GetList")]
        public virtual async Task<IEnumerable<MainEntity>> GetList()
        {
            var resultList = await _MainRepo.GetList()
                .AsNoTracking()
                .ToListAsync();
            return resultList;
        }
        [HttpPost("Create")]
        public virtual async Task<MainEntity> Create(MainEntity Data)
        {
            _MainRepo.Insert(Data);
            await _MainRepo.SaveAsync();
            return Data;
        }
        [HttpPatch("Update")]
        public async Task<MainEntity> Update(PKType id, MainEntity Data)
        {

            var oldData = await _MainRepo.GetDetailAsync(id);
            if (oldData == null) { throw new Exception("Invalid Product ID"); }

            ReAssignValues(Data, oldData);

            _MainRepo.Update(oldData);
            await _MainRepo.SaveAsync();
            return oldData;
        }

        protected virtual void ReAssignValues(MainEntity Data, MainEntity oldData)
        {
            return;
        }

        [HttpGet("GetDetail")]
        public virtual async Task<MainEntity> GetDetail(PKType id)
        {
            return await _MainRepo.GetDetailAsync(id);
        }
        [HttpDelete("Delete")]
        public virtual async Task<MainEntity> Delete(PKType id)
        {
            var oldData = await _MainRepo.GetDetailAsync(id);
            if (oldData == null) { throw new Exception("Invalid  ID"); }
            oldData.DateDeleted = DateTime.UtcNow;

            _MainRepo.Delete(oldData);
            await _MainRepo.SaveAsync();
            return oldData;

        }
    }
}