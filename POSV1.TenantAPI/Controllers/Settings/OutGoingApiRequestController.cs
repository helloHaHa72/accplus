using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Repo;

namespace POSV1.TenantAPI.Controllers.Settings
{
    [ApiController]
    [Route("api/outgoingapirequest")]
    public class OutGoingApiRequestController : ControllerBase
    {
        private readonly IOutGoingApiRequestRepository _outGoingApiRequest;
        public OutGoingApiRequestController(IOutGoingApiRequestRepository outGoingApiRequest)
        {
            _outGoingApiRequest = outGoingApiRequest;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(string ApplicationName, DateTime? FromDate, DateTime? ToDate, string Path, int? statusCode)
        {
            try
            {
                var dataQuery = _outGoingApiRequest.GetList(false);

                if (!string.IsNullOrEmpty(ApplicationName))
                {
                    dataQuery = dataQuery.Where(apiLog => apiLog.ApplicationName == ApplicationName);
                }

                if (FromDate.HasValue)
                {
                    dataQuery = dataQuery.Where(apiLog => apiLog.Timestamp >= FromDate);
                }

                if (ToDate.HasValue)
                {
                    dataQuery = dataQuery.Where(apiLog => apiLog.Timestamp <= ToDate);
                }

                if (!string.IsNullOrEmpty(Path))
                {
                    dataQuery = dataQuery.Where(apiLog => apiLog.Path.Contains(Path));
                }

                if (statusCode.HasValue)
                {
                    dataQuery = dataQuery.Where(apiLog => apiLog.ResponseStatusCode == statusCode);
                }

                return Ok(await dataQuery.ToListAsync());
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Error in GetList: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var apiLog = await _outGoingApiRequest.GetById(id);

                if (apiLog == null)
                {
                    return NotFound(); // or return a specific error message if needed
                }

                return Ok(apiLog);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("TruncateTable")]
        public async Task<IActionResult> TruncateLogsTable()
        {
            try
            {
                await _outGoingApiRequest.TruncateTable("ApiLogs");
                return Ok("Table Logs truncated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
