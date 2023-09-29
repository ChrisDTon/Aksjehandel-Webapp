using System.ComponentModel.DataAnnotations;

namespace aksjehandel.Model
{
    public class StockPurchase
    {
        public int id { get; set; }
        [RegularExpression(@"\d+")]
        public int stock_id { get; set; }
        [RegularExpression(@"\d+")]
        public int quantity { get; set; }
        [RegularExpression(@"\d+")]
        public double price { get; set; }
    }
}
