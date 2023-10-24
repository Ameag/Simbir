namespace Simbir.Middleware
{
    public class CheckTypeTransport
    {
        private readonly string[] typeTransport = { "Car", "Bike", "Scooter" };
        public bool Chek(string inputType)
        {
            foreach (var type in typeTransport) 
            {
                if(inputType == type) return true;
            }
            return false;
        }
    }
}
