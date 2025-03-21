using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantAPI.Models.EntityModels.Production;
using POSV1.TenantModel.Models.EntityModels.Production;
using POSV1.TenantModel.Repo.Interface.Production;
using System.Security.Claims;

namespace POSV1.TenantAPI.Controllers.Production
{
    [Route("api/PurchaseAdditionalCharges")]
    [ApiController]
    [Authorize]
    public class AdditionalChargesController : ControllerBase
    {
        private readonly ILogger<AdditionalChargesController> _logger;
        private readonly IAdditionalChargesRepo _additionalChargesRepo;

        public ClaimsPrincipal _ActiveUser => HttpContext.User;
        public string _ActiveUserName => _ActiveUser.Identity == null ? "User" : _ActiveUser.Identity.Name;

        public AdditionalChargesController(
            ILogger<AdditionalChargesController> logger,
            IAdditionalChargesRepo additionalChargesRepo)
        {
            _logger = logger;
            _additionalChargesRepo = additionalChargesRepo;
        }

        // GET: api/PurchaseAdditionalCharges
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _additionalChargesRepo.GetList().ToListAsync();
            var dtos = entities.Select(entity => new AdditionalChargesDto
            {
                Id = entity.add01uin,
                Title = entity.add01title,
                Description = entity.add01description,
            }).ToList();

            return Ok(dtos);
        }

        // GET: api/PurchaseAdditionalCharges/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _additionalChargesRepo.GetDetailAsync(id);
            if (entity == null) return NotFound();

            var dto = new AdditionalChargesDto
            {
                Id = entity.add01uin,
                Title = entity.add01title,
                Description = entity.add01description
            };

            return Ok(dto);
        }

        // POST: api/PurchaseAdditionalCharges
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AdditionalChargesCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = new add01additionalcharges
            {
                add01title = dto.Title,
                add01description = dto.Description,
                CreatedName = _ActiveUserName,
                DateCreated = DateTime.UtcNow
            };

            _additionalChargesRepo.Insert(entity);
            await _additionalChargesRepo.SaveAsync();

            return Ok("Data inserted !!!");
        }

        // PUT: api/PurchaseAdditionalCharges/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AdditionalChargesCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = await _additionalChargesRepo.GetDetailAsync(id);
            if (entity == null) return NotFound();

            //if(entity.DateDeleted != null) return NotFound();

            entity.add01title = dto.Title;
            entity.add01description = dto.Description;
            entity.CreatedName = _ActiveUserName;
            entity.DateCreated = DateTime.UtcNow;

            _additionalChargesRepo.Update(entity);
            await _additionalChargesRepo.SaveAsync();

            return Ok("Data updated !!!");
        }

        // DELETE: api/PurchaseAdditionalCharges/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _additionalChargesRepo.GetDetailAsync(id);
            if (entity == null) return NotFound();

            entity.DeletedName = _ActiveUserName;
            entity.DateDeleted = DateTime.UtcNow;

            _additionalChargesRepo.Update(entity);
            await _additionalChargesRepo.SaveAsync();

            return Ok("Data deleted !!");
        }
    }
}