using aksjehandel.DAL;
using aksjehandel.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aksjehandel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseRepository _db;
        // Logføring
        private readonly ILogger<PurchaseController> _logger;

        public PurchaseController(IPurchaseRepository db, ILogger<PurchaseController> logger)
        {
            _db = db;
            _logger = logger;       // Logføring
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            int? userId = HttpContext.Session.GetInt32(Helpers.userId);

            if (userId == null)
            {
                return Unauthorized();
            }

            List<StockPurchase> purchaes = await _db.GetAll((int) userId);

            return Ok(purchaes);
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody] StockPurchase purchase)
        {
            int? userId = HttpContext.Session.GetInt32(Helpers.userId);

            if (userId == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                bool returOK = await _db.Save(purchase, (int) userId);
                if (!returOK)
                {
                    _logger.LogInformation("Failed to save stock purchase");
                    return BadRequest("Failed to save stock purchase");
                }
                return Ok("Stock purchase successful");
            }

            _logger.LogInformation("Error in input validation");
            return BadRequest("Error in input validation");
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] StockPurchase newPurchase)
        {
            int? userId = HttpContext.Session.GetInt32(Helpers.userId);

            if (userId == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                bool returOK = await _db.Update(newPurchase, (int) userId);
                if (!returOK)
                {
                    _logger.LogInformation("Failed to update purchase");
                    return BadRequest("Failed to update purchase");
                }
                return Ok("Update successful");
            }

            _logger.LogInformation("Error in input validation");
            return BadRequest("Error in input validation");
        }

        [HttpDelete("{id:int}")]

        public async Task<ActionResult> Delete(int id)
        {
            int? userId = HttpContext.Session.GetInt32(Helpers.userId);

            if (userId == null)
            {
                return Unauthorized();
            }

            bool returOK = await _db.Delete(id, (int) userId);
            if (!returOK)
            {
                _logger.LogInformation("Failed to delete purchase");
                return NotFound("Failed to delete purchase");
            }
            return Ok("Purchase deleted");
        }
    }
}