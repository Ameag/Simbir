using System.ComponentModel.DataAnnotations;

namespace Simbir.Model
{
    public class BlackList
    {
        [Key]
        public string token { get; set; }
    }
}
