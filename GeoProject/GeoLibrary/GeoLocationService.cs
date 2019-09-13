using GeoLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoLibrary
{
    public class GeoLocationService : IGeoLocationService
    {  
        /// <summary>
        /// This method calculates the distance in miles or kilometers of any two
        /// latitude/longitude points by haversine formula. The haversine formula 
        /// determines the great-circle distance between two points on a sphere
        /// given their longitudes and latitudes. Important in navigation, it is 
        /// a special case of a more general formula in spherical trigonometry, 
        /// the law of haversines, that relates the 
        /// sides and angles of spherical triangles.
        /// </summary>
        /// <param name="pos1">Location 1</param>
        /// <param name="pos2">Location 2</param>
        /// <param name="unit">Miles or Kilometers</param>
        /// <returns>Distance in the requested unit</returns>
        public static double CalculateDistance(LatLng pos1, LatLng pos2, DistanceUnitEnum unit)
        {
            double R = (unit == DistanceUnitEnum.Miles) ? 3960 : 6371;
            double lat = (pos2.Latitude - pos1.Latitude).ToRadians();
            double lng = (pos2.Longitude - pos1.Longitude).ToRadians();
            double h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                          Math.Cos(pos1.Latitude.ToRadians()) * Math.Cos(pos2.Latitude.ToRadians()) *
                          Math.Sin(lng / 2) * Math.Sin(lng / 2);
            double h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
            return R * h2;
        }
    }
}
