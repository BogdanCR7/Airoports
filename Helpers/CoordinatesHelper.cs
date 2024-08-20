using WebApplication1.Models;

namespace WebApplication1.Helpers
{
    public static class CoordinatesHelper
    {
        public static double CalculateDistance(Location loc1, Location loc2)
        {
            const double EarthRadiusMiles = 3958.8;

            double lat1Rad = DegreesToRadians(loc1.Lat);
            double lon1Rad = DegreesToRadians(loc1.Lon);
            double lat2Rad = DegreesToRadians(loc2.Lat);
            double lon2Rad = DegreesToRadians(loc2.Lon);

            double deltaLat = lat2Rad - lat1Rad;
            double deltaLon = lon2Rad - lon1Rad;

            double a = Math.Pow(Math.Sin(deltaLat / 2), 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Pow(Math.Sin(deltaLon / 2), 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusMiles * c;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }


    }
}
