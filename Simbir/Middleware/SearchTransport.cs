using Simbir.Model;
using System.Data.Entity.Spatial;

namespace Simbir.Middleware
{
    public class SearchTransport
    {
        public List<Transport> GetTransports(List<Transport> transports, double lat2 ,double lon2, double radius)
        {


            var sortTransports = new List<Transport>();
            for (int i = 0; i < transports.Count; i++)
            {
                var R = 6371e3; // радиус Земли в метрах
                var φ1 = transports[i].latitude * Math.PI / 180;
                var φ2 = lat2 * Math.PI / 180;
                var Δφ = (lat2 - transports[i].latitude) * Math.PI / 180;
                var Δλ = (lon2 - transports[i].longitude) * Math.PI / 180;

                // Вычисляем гаверсинус центрального угла
                var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                        Math.Cos(φ1) * Math.Cos(φ2) *
                        Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

                // Вычисляем расстояние
                var d = R * c;

                if (d <= radius)
                {
                    sortTransports.Add(transports[i]);
                }
            }


            return sortTransports;
        }
    }
}
