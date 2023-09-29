using aksjehandel.Model;

namespace aksjehandel.DAL
{
    public class DBInit
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<Context>();

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var stock1 = new Stocks { name = "Bitcoin", price = 16183, change = 4, market_cap = 3.13 };
                var stock2 = new Stocks { name = "Apple", price = 148, change = 11, market_cap = 2.36 };
                var stock3 = new Stocks { name = "Microsoft", price = 247, change = 4, market_cap = 1.74 };
                var stock4 = new Stocks { name = "Tesla", price = 182, change = -6, market_cap = 0.572 };

                byte[] salt = Helpers.LagSalt();
                byte[] hash = Helpers.LagHash("123", salt);

                var customer1 = new Customers { firstname = "Ola", lastname = "Nordmann", email = "ola@test.no", salt = salt, password_hash = hash };

                context.Stocks.Add(stock1);
                context.Stocks.Add(stock2);
                context.Stocks.Add(stock3);
                context.Stocks.Add(stock4);

                context.Customers.Add(customer1);

                context.SaveChanges();
            }
        }
    }
}
