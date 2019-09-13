using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoLibrary.Enums;

namespace GeoLibrary.Interfaces
{
    public interface IGeoService
    {
        double CalculateDistance(double Latitude1, double Longitude1, double Latitude2, double Longitude2, DistanceUnitEnum unit)
    }
}
