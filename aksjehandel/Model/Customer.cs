
using System.ComponentModel.DataAnnotations;

namespace aksjehandel.Model
{
     // Kunde variabler
     public class Customer
    {
        public int id { get; set; }
        [RegularExpression(@"^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+.)+[a-z]{2,5}$")]
        public string email { get; set; }
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ ]{1,35}$")]
        public string firstname { get; set; }
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ ]{1,35}$")]
        public string lastname { get; set;}
    }
}