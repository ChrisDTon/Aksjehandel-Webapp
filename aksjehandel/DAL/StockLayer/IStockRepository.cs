using aksjehandel.Model;

namespace aksjehandel.DAL.StockLayer;

public interface IStockRepository
{
    Task<List<Stock>> GetAll();
}
