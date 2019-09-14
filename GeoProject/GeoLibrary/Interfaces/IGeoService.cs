using GeoAPI.Geometries;
using GeoLibrary.Enums;

namespace GeoLibrary.Interfaces
{
    public interface IGeoService
    {
        double CalculateDistance(double Latitude1, double Longitude1, double Latitude2, double Longitude2, DistanceUnitEnum unit);
        bool IsInPolygon(double latitude, double longitude, IGeometry polygon);
    }
}