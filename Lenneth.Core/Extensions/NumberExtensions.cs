using System.Diagnostics;

namespace Lenneth.Core.Extensions
{
    [DebuggerStepThrough]
    public static class NumberExtensions
    {
        /// <summary>
        /// Extension method may cause problem: -d.InRange(a, b) means -(d.InRange(a,b))
        /// </summary>
        /// <param name="aValue"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <returns></returns>
        public static bool InRange(int aValue, int aMinInclusive, int aMaxInclusive)
        {
            Debug.Assert(aMinInclusive <= aMaxInclusive);

            return (aValue >= aMinInclusive) && (aValue <= aMaxInclusive);
        }

        /// <summary>
        /// Extension method may cause problem: -d.InRange(a, b) means -(d.InRange(a,b))
        /// </summary>
        /// <param name="aValue"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <returns></returns>
        public static bool InRange(uint aValue, uint aMinInclusive, uint aMaxInclusive)
        {
            Debug.Assert(aMinInclusive <= aMaxInclusive);

            return (aValue >= aMinInclusive) && (aValue <= aMaxInclusive);
        }

        /// <summary>
        /// Extension method may cause problem: -d.InRange(a, b) means -(d.InRange(a,b))
        /// </summary>
        /// <param name="aValue"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <returns></returns>
        public static bool InRange(byte aValue, byte aMinInclusive, byte aMaxInclusive)
        {
            Debug.Assert(aMinInclusive <= aMaxInclusive);

            return (aValue >= aMinInclusive) && (aValue <= aMaxInclusive);
        }

        /// <summary>
        /// Extension method may cause problem: -d.InRange(a, b) means -(d.InRange(a,b))
        /// </summary>
        /// <param name="aValue"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <returns></returns>
        public static bool InRange(sbyte aValue, sbyte aMinInclusive, sbyte aMaxInclusive)
        {
            Debug.Assert(aMinInclusive <= aMaxInclusive);

            return (aValue >= aMinInclusive) && (aValue <= aMaxInclusive);
        }

        /// <summary>
        /// Extension method may cause problem: -d.InRange(a, b) means -(d.InRange(a,b))
        /// </summary>
        /// <param name="aValue"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <returns></returns>
        public static bool InRange(short aValue, short aMinInclusive, short aMaxInclusive)
        {
            Debug.Assert(aMinInclusive <= aMaxInclusive);

            return (aValue >= aMinInclusive) && (aValue <= aMaxInclusive);
        }

        /// <summary>
        /// Extension method may cause problem: -d.InRange(a, b) means -(d.InRange(a,b))
        /// </summary>
        /// <param name="aValue"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <returns></returns>
        public static bool InRange(ushort aValue, ushort aMinInclusive, ushort aMaxInclusive)
        {
            Debug.Assert(aMinInclusive <= aMaxInclusive);

            return (aValue >= aMinInclusive) && (aValue <= aMaxInclusive);
        }

        /// <summary>
        /// Extension method may cause problem: -d.InRange(a, b) means -(d.InRange(a,b))
        /// </summary>
        /// <param name="aValue"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <returns></returns>
        public static bool InRange(long aValue, long aMinInclusive, long aMaxInclusive)
        {
            Debug.Assert(aMinInclusive <= aMaxInclusive);

            return (aValue >= aMinInclusive) && (aValue <= aMaxInclusive);
        }

        /// <summary>
        /// Extension method may cause problem: -d.InRange(a, b) means -(d.InRange(a,b))
        /// </summary>
        /// <param name="aValue"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <returns></returns>
        public static bool InRange(ulong aValue, ulong aMinInclusive, ulong aMaxInclusive)
        {
            Debug.Assert(aMinInclusive <= aMaxInclusive);

            return (aValue >= aMinInclusive) && (aValue <= aMaxInclusive);
        }

        /// <summary>
        /// Extension method may cause problem: -d.Limit(a, b) means -(d.Limit(a,b))
        /// </summary>
        /// <param name="aD"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <returns></returns>
        public static int Limit(int aD, int aMinInclusive,
            int aMaxInclusive)
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
        /// Extension method may cause problem: -d.Limit(a, b) means -(d.Limit(a,b))
        /// </summary>
        /// <param name="aD"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <returns></returns>
        public static uint Limit(uint aD, uint aMinInclusive,
            uint aMaxInclusive)
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
        /// Extension method may cause problem: -d.Limit(a, b) means -(d.Limit(a,b))
        /// </summary>
        /// <param name="aD"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <returns></returns>
        public static byte Limit(byte aD, byte aMinInclusive,
            byte aMaxInclusive)
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
        /// Extension method may cause problem: -d.Limit(a, b) means -(d.Limit(a,b))
        /// </summary>
        /// <param name="aD"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <returns></returns>
        public static sbyte Limit(sbyte aD, sbyte aMinInclusive,
            sbyte aMaxInclusive)
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
        /// Extension method may cause problem: -d.Limit(a, b) means -(d.Limit(a,b))
        /// </summary>
        /// <param name="aD"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <returns></returns>
        public static short Limit(short aD, short aMinInclusive,
            short aMaxInclusive)
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
        /// Extension method may cause problem: -d.Limit(a, b) means -(d.Limit(a,b))
        /// </summary>
        /// <param name="aD"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <returns></returns>
        public static ushort Limit(ushort aD, ushort aMinInclusive,
            ushort aMaxInclusive)
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
        /// Extension method may cause problem: -d.Limit(a, b) means -(d.Limit(a,b))
        /// </summary>
        /// <param name="aD"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <returns></returns>
        public static long Limit(long aD, long aMinInclusive,
            long aMaxInclusive)
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
        /// Extension method may cause problem: -d.Limit(a, b) means -(d.Limit(a,b))
        /// </summary>
        /// <param name="aD"></param>
        /// <param name="aMinInclusive"></param>
        /// <param name="aMaxInclusive"></param>
        /// <returns></returns>
        public static ulong Limit(ulong aD, ulong aMinInclusive,
            ulong aMaxInclusive)
        {
            Debug.Assert(aMinInclusive <= aMaxInclusive);

            if (aD < aMinInclusive)
                return aMinInclusive;
            else if (aD > aMaxInclusive)
                return aMaxInclusive;
            else
                return aD;
        }
    }
}