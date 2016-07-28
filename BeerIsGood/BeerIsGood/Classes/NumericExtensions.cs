using System;

namespace BeerIsGood
{
    public static class NumericExtensions
    {
        public static double toRadians(this double Angle)
        {
            return (Math.PI / 180) * Angle;
        }
    }
}
