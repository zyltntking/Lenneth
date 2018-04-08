using System;
using System.Diagnostics;

namespace Lenneth.Core.Extensions
{
    [DebuggerStepThrough]
    public static class DoubleExtensions
    {
        public static bool IsNumber(this double aD)
        {
            return !double.IsInfinity(aD) && !double.IsNaN(aD);
        }

        public static double Fraction(this double aD)
        {
            return aD - Math.Truncate(aD);
        }

        public static int Round(this double aD)
        {
            return (int)Math.Round(aD);
        }

        public static int Ceiling(this double aD)
        {
            return (int)Math.Ceiling(aD);
        }

        public static int Floor(this double aD)
        {
            return (int)Math.Floor(aD);
        }

        public static bool IsAlmostRelativeEquals(this double aD1, double aD2, double aPrecision)
        {
            var mid = Math.Max(Math.Abs(aD1), Math.Abs(aD2));

            if (double.IsInfinity(mid))
                return false;
            
            if (mid > aPrecision)
                return Math.Abs(aD1 - aD2) <= aPrecision * mid;
            else
                return aD1 < aPrecision;
        }

        /// <summary>
        /// Extension method may cause problem: -d.Limit(a, b) means -(d.Limit(a,b))
        /// </summary>
        /// <param name="aD"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <returns></returns>
        public static double Limit(double aD, double aMinInclusive,
            double aMaxInclusive)
        {
            Debug.Assert(aMinInclusive <= aMaxInclusive);

            if (aD < aMinInclusive)
                return aMinInclusive;
            else if (aD > aMaxInclusive)
                return aMaxInclusive;
            else
                return aD;
        }

        /// <summary>
        /// Extension method may cause problem: -d.InRange(a, b) means -(d.InRange(a,b))
        /// </summary>
        /// <param name="aD"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <returns></returns>
        public static bool InRange(double aD, double aMinInclusive,
            double aMaxInclusive)
        {
            Debug.Assert(aMinInclusive <= aMaxInclusive);

            return (aD >= aMinInclusive) && (aD <= aMaxInclusive);
        }

        /// <summary>
        /// Extension method may cause problem: -d.InRange(a, b) means -(d.AlmostRelvativeInRange(a,b))
        /// </summary>
        /// <param name="aD"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <param name="aPrecision"></param>
        /// <returns></returns>
        public static bool AlmostRelvativeInRange(double aD, double aMinInclusive,
            double aMaxInclusive, double aPrecision)
        {
            Debug.Assert(aMinInclusive <= aMaxInclusive);

            return aD.IsAlmostRelativeGreaterOrEqualThen(aMinInclusive, aPrecision) &&
                aD.IsAlmostRelativeLessOrEqualThen(aMaxInclusive, aPrecision);
        }

        /// <summary>
        /// Extension method may cause problem: -d.InRange(a, b) means -(d.AlmostInRange(a,b))
        /// </summary>
        /// <param name="aD"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <param name="aPrecision"></param>
        /// <returns></returns>
        public static bool AlmostInRange(double aD, double aMinInclusive,
            double aMaxInclusive, double aPrecision)
        {
            Debug.Assert(aMinInclusive <= aMaxInclusive);

            return aD.IsAlmostGreaterOrEqualThen(aMinInclusive, aPrecision) &&
                aD.IsAlmostLessOrEqualThen(aMaxInclusive, aPrecision);
        }

        public static bool IsAlmostRelativeLessThen(this double aD1, double aD2, double aPrecision)
        {
            if (IsAlmostRelativeEquals(aD1, aD2, aPrecision))
                return true;

            return aD1 < aD2;
        }

        public static bool IsAlmostRelativeLessOrEqualThen(this double aD1, double aD2, double aPrecision)
        {
            if (IsAlmostRelativeEquals(aD1, aD2, aPrecision))
                return true;

            return aD1 <= aD2;
        }

        public static bool IsAlmostRelativeGreaterThen(this double aD1, double aD2, double aPrecision)
        {
            if (IsAlmostRelativeEquals(aD1, aD2, aPrecision))
                return true;

            return aD1 > aD2;
        }

        public static bool IsAlmostRelativeGreaterOrEqualThen(this double aD1, double aD2, double aPrecision)
        {
            if (IsAlmostRelativeEquals(aD1, aD2, aPrecision))
                return true;

            return aD1 >= aD2;
        }

        public static double Min(double aD1, double aD2, double aD3)
        {
            if (aD1 < aD2)
            {
                if (aD1 < aD3)
                    return aD1;
                else
                    return aD3;
            }
            else
            {
                if (aD2 < aD3)
                    return aD2;
                else
                    return aD3;
            }
        }

        public static double Min(double aD1, double aD2, double aD3, double aD4)
        {
            if (aD1 < aD2)
            {
                if (aD1 < aD3)
                {
                    if (aD1 < aD4)
                        return aD1;
                    else
                        return aD4;
                }
                else
                {
                    if (aD3 < aD4)
                        return aD3;
                    else
                        return aD4;
                }
            }
            else
            {
                if (aD2 < aD3)
                {
                    if (aD2 < aD4)
                        return aD2;
                    else
                        return aD4;
                }
                else
                {
                    if (aD3 < aD4)
                        return aD3;
                    else
                        return aD4;
                }
            }
        }

        public static double Max(double aD1, double aD2, double aD3)
        {
            if (aD1 > aD2)
            {
                if (aD1 > aD3)
                    return aD1;
                else
                    return aD3;
            }
            else
            {
                if (aD2 > aD3)
                    return aD2;
                else
                    return aD3;
            }
        }

        public static double Max(double aD1, double aD2, double aD3, double aD4)
        {
            if (aD1 > aD2)
            {
                if (aD1 > aD3)
                {
                    if (aD1 > aD4)
                        return aD1;
                    else
                        return aD4;
                }
                else
                {
                    if (aD3 > aD4)
                        return aD3;
                    else
                        return aD4;
                }
            }
            else
            {
                if (aD2 > aD3)
                {
                    if (aD2 > aD4)
                        return aD2;
                    else
                        return aD4;
                }
                else
                {
                    if (aD3 > aD4)
                        return aD3;
                    else
                        return aD4;
                }
            }
        }

        public static bool IsAlmostLessThen(this double aD1, double aD2, double aPrecision)
        {
            return aD1 < aD2 + aPrecision;
        }

        public static bool IsAlmostLessOrEqualThen(this double aD1, double aD2, double aPrecision)
        {
            return aD1 <= aD2 + aPrecision;
        }

        public static bool IsAlmostGreaterThen(this double aD1, double aD2, double aPrecision)
        {
            return aD1 > aD2 - aPrecision;
        }

        public static bool IsAlmostGreaterOrEqualThen(this double aD1, double aD2, double aPrecision)
        {
            return aD1 >= aD2 - aPrecision;
        }

        public static bool IsAlmostEquals(this double aD1, double aD2, double aPrecision)
        {
            return Math.Abs(aD1 - aD2) < aPrecision;
        }
    }
}