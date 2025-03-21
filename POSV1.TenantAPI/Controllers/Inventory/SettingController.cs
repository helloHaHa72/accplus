using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSV1.TenantAPI.Models;
using POSV1.TenantModel;
using POSV1.TenantModel.Models;

namespace POSV1.TenantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        public MainDbContext _context { get; set; }
        public SettingController(MainDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList()
        {
            var settingList = _context.settings.ToList();

            return Ok(settingList);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Setting model)
        {
            if (ModelState.IsValid)
            {
                _context.settings.Add(model);
                await _context.SaveChangesAsync();

                return Ok("Data created successfully");
            }

            return BadRequest("Invalid model data");
        }
    }
}
