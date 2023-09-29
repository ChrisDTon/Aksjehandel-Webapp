using aksjehandel.Model;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace aksjehandel.DAL
{
    public class PurchaseRepository: IPurchaseRepository
    {
        private readonly Context _db;
        private readonly ILogger<PurchaseRepository> _logger;

        public PurchaseRepository(Context db, ILogger<PurchaseRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<StockPurchase>> GetAll(int userId)
        {
            try
            {
                List<StockPurchase> allPurchases = await _db.Purchases.Where(p => p.Customer.id == userId).Select(k => new StockPurchase
                {
                    id = k.id,
                    stock_id = k.Stock.id,
                    price = k.price,
                    quantity = k.quantity
                }).ToListAsync();

                return allPurchases;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return null;
            }
        }

        public async Task<bool> Save(StockPurchase purchase, int userId)
        {
            try
            {
                var checkStock = await _db.Stocks.FindAsync(purchase.stock_id);

                if (checkStock == null)
                {
                    // Stock does not exist. Handle error
                    return false;
                }

                var checkCustomer = await _db.Customers.FindAsync(userId);

                if (checkCustomer == null)
                {
                    // Customer does not exist. Handle error
                    return false;
                }

                var existingPurchase = await _db.Purchases.FirstOrDefaultAsync(p => p.Stock.id == purchase.stock_id && p.Customer.id == userId);

                if (existingPurchase != null)
                {
                    existingPurchase.quantity += purchase.quantity;
                    await _db.SaveChangesAsync();

                    return true;
                }

                var newPurchase = new StockPurchases();

                newPurchase.quantity = purchase.quantity;
                newPurchase.price = purchase.price;
                newPurchase.Stock = checkStock;
                newPurchase.Customer = checkCustomer;

                _db.Purchases.Add(newPurchase);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return false;
            }
        }

        public async Task<bool> Update(StockPurchase purchase, int userId)
        {
            try
            {
                var existingPurchase = await _db.Purchases.FirstOrDefaultAsync(p => p.id == purchase.id && p.Customer.id == userId);

                if (existingPurchase == null)
                {
                    // Purchase does not exist. Handle error
                    return false;
                }

                existingPurchase.quantity -= purchase.quantity;

                // If quantity is 0, delete
                if (existingPurchase.quantity <= 0)
                {
                    _db.Purchases.Remove(existingPurchase);
                }

                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return false;
            }

            return true;
        }

        public async Task<bool> Delete(int id, int userId)
        {
            try
            {
                StockPurchases purchase = await _db.Purchases.FindAsync(id);

                if (purchase == null)
                {
                    // Not found error
                    return false;
                }

                _db.Purchases.Remove(purchase);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return false;
            }
        }
    }
}
