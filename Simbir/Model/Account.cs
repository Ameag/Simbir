using System.Numerics;

namespace Simbir.Model
{
    public class Account
    {
        public long id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public bool is_admin { get; set; }
    }
}
