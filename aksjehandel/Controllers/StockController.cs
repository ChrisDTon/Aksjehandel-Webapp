using aksjehandel.DAL.StockLayer;
using aksjehandel.DAL;
using aksjehandel.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace aksjehandel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _db;
        // Logføring
        private readonly ILogger<StockController> _logger;

        public StockController(IStockRepository db, ILogger<StockController> logger)
        {
            _db = db;
            _logger = logger;       // Logføring
        }

        public async Task<List<Stock>> GetAll()
        {
            return await _db.GetAll();
        }
    }
}
