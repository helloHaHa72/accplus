using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Models.EntityModels.Inventory;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Repo.Interface;
using System.Diagnostics.Metrics;

namespace POSV1.TenantAPI.Controllers.Inventory
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerTypeController : ControllerBase
    {
        private readonly ILogger<CustomerTypeController> _logger;
        private readonly ICustomerTypeRepo _customerTypeRepo;
        public CustomerTypeController(
            ILogger<CustomerTypeController> logger,
            ICustomerTypeRepo customerTypeRepo)
        {
            _customerTypeRepo = customerTypeRepo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var data = _customerTypeRepo.GetList();

                var finalData = data.Select(c => new ViewCustomerType
                {
                    Id = c.cus02Id,
                    Title = c.cus02Name,
                    DiscountPercentage = c.cus02DiscountPercenatge,
                }).ToList();

                return Ok(finalData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var data = await _customerTypeRepo.GetDetailAsync(id);

                var finalData = new ViewCustomerType()
                {
                    Id = data.cus02Id,
                    Title = data.cus02Name,
                    DiscountPercentage = data.cus02DiscountPercenatge
                };

                return Ok(finalData);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to get data by ID {id}. {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] VMCustomerType data)
        {
            try
            {
                var addData = new cus02customerType()
                {
                    cus02Name = data.Title,
                    cus02DiscountPercenatge = data.DiscountPercentage,
                };

                _customerTypeRepo.Insert(addData);
                await _customerTypeRepo.SaveAsync();

                return Ok($"{data.Title} added");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to add data. {ex.Message}");
            }
        }

        [HttpPut, Route("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VMCustomerType data)
        {
            try
            {
                var cusData = await _customerTypeRepo.GetDetailAsync(id);

                cusData.cus02Name = data.Title;
                cusData.cus02DiscountPercenatge = data.DiscountPercentage;

                var updateData = _customerTypeRepo.Update(cusData);
                await _customerTypeRepo.SaveAsync();

                return Ok($"{data.Title} updated");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update data. {ex.Message}");
            }
        }

        [HttpDelete, Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var countryData = await _customerTypeRepo.GetDetailAsync(id);

                string cusName = countryData.cus02Name;

                countryData.DateDeleted = DateTime.Now;
                countryData.DeletedName = "ADMIN";

                var updateData = _customerTypeRepo.Update(countryData);
                await _customerTypeRepo.SaveAsync();

                return Ok($"{cusName} deleted");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete data. {ex.Message}");
            }
        }
    }
}
