using aksjehandel.Model;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace aksjehandel.DAL.StockLayer
{
    public class StockRepository: IStockRepository
    {
        private readonly Context _db;
        private readonly ILogger<StockRepository> _logger;

        public StockRepository(Context db, ILogger<StockRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<Stock>> GetAll()
        {
            try
            {
                List<Stock> allStocks = await _db.Stocks.Select(k => new Stock
                {
                    id = k.id,
                    name = k.name,
                    price = k.price,
                    change = k.change,
                    market_cap = k.market_cap,
                }).ToListAsync();

                return allStocks;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return null;
            }
        }
    }
}
