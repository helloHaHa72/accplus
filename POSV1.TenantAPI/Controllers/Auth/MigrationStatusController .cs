using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POSV1.TenantAPI.Services;

namespace POSV1.TenantAPI.Controllers.Auth
{
    [ApiController]
    [Authorize]
    public class MigrationStatusController : ControllerBase
    {
        private readonly MigrationStatusService _migrationStatusService;
        private readonly ILogger<MigrationStatusController> _logger;

        public MigrationStatusController(MigrationStatusService migrationStatusService, ILogger<MigrationStatusController> logger)
        {
            _migrationStatusService = migrationStatusService;
            _logger = logger;
        }

        [HttpGet("status")]
        public async Task<ActionResult<MigrationStatusDto>> GetMigrationStatus()
        {
            try
            {
                var status = _migrationStatusService.GetMigrationStatus();
                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving migration status.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
