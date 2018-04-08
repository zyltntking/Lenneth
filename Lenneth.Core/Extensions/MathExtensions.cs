using System;

namespace Lenneth.Core.Extensions
{
    public static class MathExtensions
    {
        public const double Pi = 3.1415926535897932384626433832795;
        public const double Sqrt2 = 1.4142135623730950488016887242097;
        public const double Sqrt3 = 1.7320508075688772935274463415059;

        private static readonly int[][] SPascalTriangle;

        static MathExtensions()
        {
            SPascalTriangle = new int[10][];
            SPascalTriangle[0] = new[] { 1 };

            for (var i = 1; i < SPascalTriangle.Length; i++)
                SPascalTriangle[i] = PascalTriangle(SPascalTriangle[i - 1]);
        }

        public static int[] PascalTriangle(int aRow)
        {
            if (aRow <= SPascalTriangle.Length)
                return SPascalTriangle[aRow - 1];
            else
            {
                var result = SPascalTriangle[SPascalTriangle.Length - 1];

                for (var i = SPascalTriangle.Length; i < aRow; i++)
                    result = PascalTriangle(result);

                return result;
            }
        }

        private static int[] PascalTriangle(int[] aStart)
        {
            var result = new int[aStart.Length + 1];

            result[0] = 1;
            result[result.Length - 1] = 1;

            for (var j = 0; j < aStart.Length - 1; j++)
                result[j + 1] = aStart[j] + aStart[j + 1];

            return result;
        }

        public static double ToRad(double aDeg)
        {
            return aDeg * Pi / 180;
        }

        public static double ToDeg(double aRad)
        {
            return aRad * 180 / Pi;
        }

        public static double Hypot(double aX, double aY)
        {
            return Math.Sqrt(aX * aX + aY * aY);
        }
    }
}