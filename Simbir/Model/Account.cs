using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Simbir.Model
{
    public class Account
    {
        [Key]
        public string login { get; set; }
        public string password { get; set; }
        public bool is_admin { get; set; }
    }

    public class AccountOutput
    {
        public string login { get; set; }
        public string password { get; set; }
    }

    public class AccountInput
    {
        public string login { get; set; }
        public string password { get; set; }
    }
}
