using System;
using System.Diagnostics;

namespace Lenneth.Core.Extensions
{
    [DebuggerStepThrough]
    public static class SingleExtensions
    {
        public static bool IsNumber(this float aD)
        {
            return !float.IsInfinity(aD) && !float.IsNaN(aD);
        }

        public static float Fraction(this float aD)
        {
            return aD - (float)Math.Truncate(aD);
        }

        public static int Round(this float aD)
        {
            return (int)Math.Round(aD);
        }

        public static int Ceiling(this float aD)
        {
            return (int)Math.Ceiling(aD);
        }

        public static int Floor(this float aD)
        {
            return (int)Math.Floor(aD);
        }

        public static bool IsAlmostRelativeEquals(this float aD1, float aD2, float aPrecision)
        {
            double mid = Math.Max(Math.Abs(aD1), Math.Abs(aD2));

            if (double.IsInfinity(mid))
                return false;

            if (mid > aPrecision)
                return Math.Abs(aD1 - aD2) <= aPrecision * mid;
            else
                return aD1 < aPrecision;
        }

        public static bool IsAlmostEquals(this float aD1, float aD2, float aPrecision)
        {
            return Math.Abs(aD1 - aD2) < aPrecision;
        }

        /// <summary>
        /// Extension method may cause problem: -d.Limit(a, b) means -(d.Limit(a,b))
        /// </summary>
        /// <param name="aD"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <returns></returns>
        public static float Limit(float aD, float aMinInclusive,
            float aMaxInclusive)
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
        public static bool InRange(float aD, float aMinInclusive,
            float aMaxInclusive)
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
        public static bool AlmostRelvativeInRange(float aD, float aMinInclusive,
            float aMaxInclusive, float aPrecision)
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
        public static bool AlmostInRange(float aD, float aMinInclusive,
            float aMaxInclusive, float aPrecision)
        {
            Debug.Assert(aMinInclusive <= aMaxInclusive);

            return aD.IsAlmostGreaterOrEqualThen(aMinInclusive, aPrecision) &&
                aD.IsAlmostLessOrEqualThen(aMaxInclusive, aPrecision);
        }

        public static bool IsAlmostRelativeLessThen(this float aD1, float aD2, float aPrecision)
        {
            if (IsAlmostRelativeEquals(aD1, aD2, aPrecision))
                return true;

            return aD1 < aD2;
        }

        public static bool IsAlmostRelativeLessOrEqualThen(this float aD1, float aD2, float aPrecision)
        {
            if (IsAlmostRelativeEquals(aD1, aD2, aPrecision))
                return true;

            return aD1 <= aD2;
        }

        public static bool IsAlmostRelativeGreaterThen(this float aD1, float aD2, float aPrecision)
        {
            if (IsAlmostRelativeEquals(aD1, aD2, aPrecision))
                return true;

            return aD1 > aD2;
        }

        public static bool IsAlmostRelativeGreaterOrEqualThen(this float aD1, float aD2, float aPrecision)
        {
            if (IsAlmostRelativeEquals(aD1, aD2, aPrecision))
                return true;

            return aD1 >= aD2;
        }

        public static float Min(float aD1, float aD2, float aD3)
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

        public static float Min(float aD1, float aD2, float aD3, float aD4)
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

        public static float Max(float aD1, float aD2, float aD3)
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

        public static float Max(float aD1, float aD2, float aD3, float aD4)
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

        public static bool IsAlmostLessThen(this float aD1, float aD2, float aPrecision)
        {
            return aD1 < aD2 + aPrecision;
        }

        public static bool IsAlmostLessOrEqualThen(this float aD1, float aD2, float aPrecision)
        {
            return aD1 <= aD2 + aPrecision;
        }

        public static bool IsAlmostGreaterThen(this float aD1, float aD2, float aPrecision)
        {
            return aD1 > aD2 - aPrecision;
        }

        public static bool IsAlmostGreaterOrEqualThen(this float aD1, float aD2, float aPrecision)
        {
            return aD1 >= aD2 - aPrecision;
        }
    }
}