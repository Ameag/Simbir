using System.ComponentModel.DataAnnotations;

namespace Simbir.Model
{
    public class Rent
    {
        [Key]
        public int id { get; set; }
        public int user_id { get; set; }
        public int transport_id { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public double radius { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set;}
        public double price_of_unit { get; set; }
        public string price_type { get; set; }
        public int final_price { get; set; }
    }

    public class InputAdminRent
    {
        public int user_id { get; set; }
        public int transport_id { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
        public double price_of_unit { get; set; }
        public string price_type { get; set; }
        public int final_price { get; set; }
    }
}
