using System;
using GeoAPI.Geometries;
using GeoLibrary.Enums;
using GeoLibrary.Helpers;
using GeoLibrary.Interfaces;
using NetTopologySuite.Geometries;

namespace GeoLibrary.Implementations
{
    public class GeoService : IGeoService
    {
        #region Constructors
        public GeoService()
        {
        }
        #endregion

        #region Methods
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
        public double CalculateDistance(double Latitude1, double Longitude1, double Latitude2, double Longitude2, DistanceUnitEnum unit)
        {
            double R = (unit == DistanceUnitEnum.Miles) ? 3960 : 6371;
            double lat = Commons.ToRadians(Latitude2 - Latitude1);
            double lng = Commons.ToRadians(Longitude2 - Longitude1);
            double h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                          Math.Cos(Commons.ToRadians(Latitude1)) * Math.Cos(Commons.ToRadians(Latitude2)) *
                          Math.Sin(lng / 2) * Math.Sin(lng / 2);
            double h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
            return R * h2;
        }

        public bool IsInPolygon(double latitude, double longitude, IGeometry polygon)
        {
            //Coordinate coord = new Coordinate(longitude, latitude);
            //IPoint point = new Point(coord) as IPoint;

            //or 
            Point point = new Point(latitude, longitude);

            bool isInPolygon = polygon != null ? polygon.Contains(point) : false;
            return isInPolygon;
        }
        #endregion
    }
}