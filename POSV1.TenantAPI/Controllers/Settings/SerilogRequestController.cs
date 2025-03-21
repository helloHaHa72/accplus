using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Repo;

namespace POSV1.TenantAPI.Controllers.Settings
{
    [ApiController]
    [Route("api/serilogRequest")]
    public class SerilogRequestController : ControllerBase
    {
        private readonly ISeriLogRepository _MainRepo;
        public SerilogRequestController(ISeriLogRepository mainRepo)
        {
            _MainRepo = mainRepo;
        }

        /*[HttpGet("[action]")]
        public async Task<IActionResult> GetList(string? ApplicationName, DateTime? FromDate, DateTime? ToDate, string? Path, int? statusCode)
        {
            try
            {
                var dataQuery = _MainRepo.GetList(false);

                if (dataQuery == null)
                {
                    Console.WriteLine("Data not found in GetList.");
                    throw new Exception("Data not found");
                }

                return Ok(await dataQuery.ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetList: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }*/

        [HttpGet("[action]")]
        public async Task<IActionResult> GetList(DateTime? FromDate, DateTime? ToDate, string level)
        {
            try
            {
                var dataQuery = _MainRepo.GetList(false);

                if (dataQuery == null)
                {
                    Console.WriteLine("Data not found in GetList.");
                    throw new Exception("Data not found");
                }

                // Apply filters based on parameters
                if (FromDate.HasValue)
                {
                    dataQuery = dataQuery.Where(item => item.TimeStamp >= FromDate.Value);
                }

                if (ToDate.HasValue)
                {
                    dataQuery = dataQuery.Where(item => item.TimeStamp <= ToDate.Value);
                }

                if (!string.IsNullOrEmpty(level))
                {
                    dataQuery = dataQuery.Where(item => item.Level == level);
                }

                // Apply ordering and take top 100
                dataQuery = dataQuery.OrderByDescending(apiLog => apiLog.Id).Take(100);

                return Ok(await dataQuery.ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetList: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var apiLog = await _MainRepo.GetById(id);

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
                await _MainRepo.TruncateTable("SeriLog.Logs");
                return Ok("Table Logs truncated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
