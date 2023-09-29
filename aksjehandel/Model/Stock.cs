using System.ComponentModel.DataAnnotations;

namespace aksjehandel.Model
{
    public class Stock
    {
        public int id { get; set; }
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ ]{1,35}$")]
        public string name { get; set; }
        [RegularExpression(@"^(?=.*[1-9])[0-9]{1,9}(\.[0-9]{0,10})$")]
        public double price { get; set; }
        [RegularExpression(@"^\d + ([.]\d +)?%?$")]
        public double change { get; set; }
        [RegularExpression(@"^(?=.*[1-9])[0-9]{1,9}(\.[0-9]{0,10})$")]
        public double market_cap { get; set; }
    }
}
