using System;
using GeoAPI.Geometries;

namespace GeoLibrary
{
    class Program
    {
        public static IGeometry polygon { get; set; }
        public static IGeometry Coord { get; set; }
        public static double Lat { get; set; }
        public static double Lng { get; set; }

        static void Main(string[] args)
        {
            Lat = 34.1468658447266;
            Lng = -118.065460205078;

            bool isInPolygon = polygon.Contains(Coord);
            Console.WriteLine(isInPolygon);


            Console.ReadKey();
        }
    }
}