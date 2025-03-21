using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantAPI.Models.EntityModels.Production;
using POSV1.TenantModel.Models.EntityModels.Production;
using POSV1.TenantModel.Repo.Implementation.Production;
using POSV1.TenantModel.Repo.Interface.Production;
using System.Security.Claims;

namespace POSV1.TenantAPI.Controllers.Production
{
    [Route("api/AdditionalCharges")]
    [ApiController]
    [Authorize]
    public class PurchaseAdditionalChargesController : ControllerBase
    {
        private readonly ILogger<PurchaseAdditionalChargesController> _logger;
        private readonly IPurchaseAdditionalCharges _purchaseAdditionalCharges;
        private readonly IAdditionalChargesPurchaseRelation _additionalChargesPurchaseRelation;

        public ClaimsPrincipal _ActiveUser => HttpContext.User;
        public string _ActiveUserName => _ActiveUser.Identity == null ? "User" : _ActiveUser.Identity.Name;
        public PurchaseAdditionalChargesController(
            ILogger<PurchaseAdditionalChargesController> logger,
            IPurchaseAdditionalCharges purchaseAdditionalCharges,
            IAdditionalChargesPurchaseRelation additionalChargesPurchaseRelation)
        {
            _logger = logger;
            _purchaseAdditionalCharges = purchaseAdditionalCharges;
            _additionalChargesPurchaseRelation = additionalChargesPurchaseRelation;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PurchaseAdditionalChargesDto dtos)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // List to hold the entities to be inserted
                add02purchaseadditionalcharges insertData = await CreateCharges(dtos);

                foreach (var id in dtos.PurchaseIds)
                {
                    var insertRelData = new add04chargepurchaserel()
                    {
                        add04puraddchargeuin = insertData.add02uin,
                        add04purchaseuin = id,
                    };

                    _additionalChargesPurchaseRelation.Insert(insertRelData);
                }
                await _additionalChargesPurchaseRelation.SaveAsync();


                return Ok("Data inserted successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating production");
                return StatusCode(500, "Internal Server Error");
            }
        }

        private async Task<add02purchaseadditionalcharges> CreateCharges(PurchaseAdditionalChargesDto dtos)
        {
            var entitiesToInsert = new List<add03purchaseadditionalchargesdetail>();

            foreach (var d in dtos.ChargeData)
            {
                var data = new add03purchaseadditionalchargesdetail()
                {
                    add03title = d.Title,
                    add03amount = d.Amount,
                    add03remarks = d.Remarks,
                };

                entitiesToInsert.Add(data);
            }

            var insertData = new add02purchaseadditionalcharges
            {
                DateCreated = DateTime.UtcNow,
                CreatedName = _ActiveUserName,
                // Map other properties from dto as needed
                AdditionalChargesDetails = entitiesToInsert,
            };

            _purchaseAdditionalCharges.Insert(insertData);
            await _purchaseAdditionalCharges.SaveAsync();
            return insertData;
        }
    }
}
