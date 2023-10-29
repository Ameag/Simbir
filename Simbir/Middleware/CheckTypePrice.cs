using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Simbir.Middleware
{
    public class CheckTypePrice
    {
        private readonly string[] ArrayType = { "Minutes", "Days" };
        public bool Chek (string type_price)
        {
            foreach (var type in ArrayType)
            {
                if (type_price == type) return true;
            }
            return false;
        }
    }
}
