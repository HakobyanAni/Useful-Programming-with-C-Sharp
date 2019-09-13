using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoLibrary.Helpers
{
    public static class Commons
    {
        public static double ToRadians(double val)
        {
            return (Math.PI / 180) * val;
        }
    }
}