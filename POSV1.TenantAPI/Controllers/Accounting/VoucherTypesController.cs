using BaseAppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSV1.TenantAPI.Models;
using POSV1.TenantModel;

namespace POSV1.TenantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherTypesController : ControllerBase
    {
        private readonly MainDbContext _context;

        public VoucherTypesController(MainDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var voucherTypes = _context.vou01voucher_types.ToList().OrderByDescending(x => x.DateCreated);

            return Ok(voucherTypes);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.vou01voucher_types.FindAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            _context.vou01voucher_types.Remove(data);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
