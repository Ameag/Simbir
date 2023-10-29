using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;

namespace Simbir.Model
{
    public class Transport
    {
        [Key]
        public int id { get; set; }
        public string  transport_type { get; set; }
        public string identifier { get; set; }
        public bool can_be_rented { get; set; }
        public string model { get; set; }
        public string color { get; set; }
        public string description { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int minute_price { get; set; }
        public int day_price { get; set; }
        public int owner_id { get; set; }
    }

    public class TransportInput
    {
        public string transport_type { get; set; }
        public string identifier { get; set; }
        public bool can_be_rented { get; set; }
        public string model { get; set; }
        public string color { get; set; }
        public string description { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int minute_price { get; set; }
        public int day_price { get; set; }
    }

    public class TransportInputAdmin
    {
        public string transport_type { get; set; }
        public string identifier { get; set; }
        public bool can_be_rented { get; set; }
        public string model { get; set; }
        public string color { get; set; }
        public string description { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int minute_price { get; set; }
        public int day_price { get; set; }
        public int owner_id { get; set; }
    }
}
