using aksjehandel.Model;
using Microsoft.EntityFrameworkCore;

namespace aksjehandel.DAL
{
    public class Customers
    {
        public int id { get; set; }
        public string email { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public byte[] password_hash { get; set; }
        public byte[] salt { get; set; }
    }

    public class Stocks
    {
        public int id { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public double change { get; set; }
        public double market_cap { get; set; }
    }

    public class StockPurchases
    {
        public int id { get; set; }
        virtual public Customers Customer { get; set; }
        virtual public Stocks Stock { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
    }

    public class Context : DbContext
    {

        public Context(DbContextOptions<Context> options) : base(options)
        {
            Database.EnsureCreated();
        }

        // Definere tabellen
        public virtual DbSet<Stocks> Stocks { get; set; }
        public virtual DbSet<StockPurchases> Purchases { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }

    }
}
