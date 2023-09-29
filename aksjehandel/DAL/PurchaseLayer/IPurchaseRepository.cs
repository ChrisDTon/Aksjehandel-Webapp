using aksjehandel.Model;

namespace aksjehandel.DAL;

public interface IPurchaseRepository
{
    Task<List<StockPurchase>> GetAll(int userId);
    Task<bool> Save(StockPurchase stockPurchase, int userId);
    Task<bool> Update(StockPurchase stockPurchase, int userId);
    Task<bool> Delete(int id, int userId);
}
