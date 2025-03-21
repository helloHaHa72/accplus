using BaseAppSettings;
using Microsoft.AspNetCore.Mvc;
using POSV1.TenantModel;
using RepoBaseModelCore;

namespace POSV1.TenantAPI.Controllers
{
    public abstract class _AbsBasicController<ChildController, MainRepo, MainEntity, PKType>
        : ControllerBase
        where ChildController : ControllerBase
        where MainRepo : IGeneralRepositories<MainEntity, PKType>
        where MainEntity : Auditable
    {
        protected readonly ILogger<ChildController> _logger;
        protected readonly MainRepo _MainRepo;
        protected readonly MainDbContext _context;
        public _AbsBasicController(ILogger<ChildController> log, MainDbContext dbContext, MainRepo repo)
        {
            _logger = log;
            _MainRepo = repo;
            _context = dbContext;
        }

    }
}