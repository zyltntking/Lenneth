using System;
using System.Diagnostics;

/* C# Version Copyright (C) 2001-2004 Akihilo Kramot (Takel).  */
/* C# porting from a C-program for MT19937, originaly coded by */
/* Takuji Nishimura, considering the suggestions by            */
/* Topher Cooper and Marc Rieffel in July-Aug. 1997.           */
/* This library is free software under the Artistic license:   */
/*                                                             */
/* You can find the original C-program at                      */
/*     http://www.math.keio.ac.jp/~matumoto/mt.html            */
/*                                                             */

namespace Lenneth.Core.Extensions.Utils
{
    public sealed class MersenneTwister : Random
    {
        /* Period parameters */
        private const int N = 624;
        private const int M = 397;
        private const uint MatrixA = 0x9908b0df; /* constant vector a */
        private const uint UpperMask = 0x80000000; /* most significant w-r bits */
        private const uint LowerMask = 0x7fffffff; /* least significant r bits */

        /* Tempering parameters */
        private const uint TemperingMaskB = 0x9d2c5680;
        private const uint TemperingMaskC = 0xefc60000;

        private static uint TEMPERING_SHIFT_U(uint y) { return (y >> 11); }

        private static uint TEMPERING_SHIFT_S(uint y) { return (y << 7); }

        private static uint TEMPERING_SHIFT_T(uint y) { return (y << 15); }

        private static uint TEMPERING_SHIFT_L(uint y) { return (y >> 18); }

        private readonly uint[] _mt = new uint[N]; /* the array for the state vector  */

        private short _mti;

        private static readonly uint[] Mag01 = { 0x0, MatrixA };

        /* initializing the array with a NONZERO seed */

        public MersenneTwister(uint seed)
        {
            /* setting initial seeds to mt[N] using         */
            /* the generator Line 25 of Table 1 in          */
            /* [KNUTH 1981, The Art of Computer Programming */
            /*    Vol. 2 (2nd Ed.), pp102]                  */
            _mt[0] = seed & 0xffffffffU;
            for (_mti = 1; _mti < N; ++_mti)
            {
                _mt[_mti] = (69069 * _mt[_mti - 1]) & 0xffffffffU;
            }
        }

        public MersenneTwister()
            : this((uint)Environment.TickCount)
        {
        }

        private uint GenerateUInt()
        {
            uint y;

            /* mag01[x] = x * MATRIX_A  for x=0,1 */
            if (_mti >= N) /* generate N words at one time */
            {
                short kk = 0;

                for (; kk < N - M; ++kk)
                {
                    y = (_mt[kk] & UpperMask) | (_mt[kk + 1] & LowerMask);
                    _mt[kk] = _mt[kk + M] ^ (y >> 1) ^ Mag01[y & 0x1];
                }

                for (; kk < N - 1; ++kk)
                {
                    y = (_mt[kk] & UpperMask) | (_mt[kk + 1] & LowerMask);
                    _mt[kk] = _mt[kk + (M - N)] ^ (y >> 1) ^ Mag01[y & 0x1];
                }

                y = (_mt[N - 1] & UpperMask) | (_mt[0] & LowerMask);
                _mt[N - 1] = _mt[M - 1] ^ (y >> 1) ^ Mag01[y & 0x1];

                _mti = 0;
            }

            y = _mt[_mti++];
            y ^= TEMPERING_SHIFT_U(y);
            y ^= TEMPERING_SHIFT_S(y) & TemperingMaskB;
            y ^= TEMPERING_SHIFT_T(y) & TemperingMaskC;
            y ^= TEMPERING_SHIFT_L(y);

            return y;
        }

        public uint NextUInt()
        {
            return GenerateUInt();
        }

        public uint NextUInt(uint maxValue)
        {
            return (uint)(GenerateUInt() / ((double)uint.MaxValue / maxValue));
        }

        public ushort NextUShort(ushort maxValue)
        {
            return (ushort)(GenerateUInt() / ((double)ushort.MaxValue / maxValue));
        }

        public ushort NextUShort(int maxValue)
        {
            if (maxValue > ushort.MaxValue)
                throw new ArgumentOutOfRangeException();
            if (maxValue <= 0)
                throw new ArgumentOutOfRangeException();

            return (ushort)(GenerateUInt() / ((double)ushort.MaxValue / maxValue));
        }

        public uint NextUInt(uint minValue, uint maxValue) /* throws ArgumentOutOfRangeException */
        {
            Debug.Assert(minValue < maxValue);

            return (uint)(GenerateUInt() / ((double)uint.MaxValue / (maxValue - minValue)) + minValue);
        }

        public override int Next()
        {
            return Next(int.MaxValue);
        }

        public override int Next(int maxValue) /* throws ArgumentOutOfRangeException */
        {
            Debug.Assert(maxValue > 0);

            return (int)(NextDouble() * maxValue);
        }

        public override int Next(int minValue, int maxValue)
        {
            Debug.Assert(maxValue >= minValue);

            if (maxValue == minValue)
            {
                return minValue;
            }
            else
            {
                return Next(maxValue - minValue) + minValue;
            }
        }

        public override void NextBytes(byte[] buffer) /* throws ArgumentNullException*/
        {
            int bufLen = buffer.Length;

            for (int idx = 0; idx < bufLen; ++idx)
                buffer[idx] = (byte)Next(256);
        }

        public override double NextDouble()
        {
            return (double)GenerateUInt() / ((ulong)uint.MaxValue + 1);
        }

        public float NextFloat()
        {
            return (float)GenerateUInt() / ((ulong)uint.MaxValue + 1);
        }

        public byte NextByte()
        {
            return (byte)NextUInt(byte.MaxValue);
        }

        public char NextChar()
        {
            return (char)NextUInt(char.MaxValue);
        }

        public short NextShort()
        {
            return (short)Next(short.MinValue, short.MaxValue);
        }

        public ushort NextUShort()
        {
            return (ushort)NextUInt(ushort.MaxValue);
        }

        public int NextInt()
        {
            return (int)GenerateUInt();
        }

        public long NextLong()
        {
            return ((long)NextUInt() << 32) | NextUInt();
        }

        public ulong NextULong()
        {
            return ((ulong)NextUInt() << 32) | NextUInt();
        }

        public byte[] NextBytes(int aLength)
        {
            byte[] result = new byte[aLength];
            for (int i = 0; i < aLength; i++)
                result[i] = NextByte();
            return result;
        }

        public char[] NextChars(int aLength)
        {
            char[] result = new char[aLength];
            for (int i = 0; i < aLength; i++)
                result[i] = NextChar();
            return result;
        }

        public short[] NextShorts(int aLength)
        {
            short[] result = new short[aLength];
            for (int i = 0; i < aLength; i++)
                result[i] = NextShort();
            return result;
        }

        public ushort[] NextUShorts(int aLength)
        {
            ushort[] result = new ushort[aLength];
            for (int i = 0; i < aLength; i++)
                result[i] = NextUShort();
            return result;
        }

        public int[] NextInts(int aLength)
        {
            int[] result = new int[aLength];
            for (int i = 0; i < aLength; i++)
                result[i] = Next();
            return result;
        }

        public uint[] NextUInts(int aLength)
        {
            uint[] result = new uint[aLength];
            for (int i = 0; i < aLength; i++)
                result[i] = NextUInt();
            return result;
        }

        public long[] NextLongs(int aLength)
        {
            long[] result = new long[aLength];
            for (int i = 0; i < aLength; i++)
                result[i] = NextLong();
            return result;
        }

        public ulong[] NextULongs(int aLength)
        {
            ulong[] result = new ulong[aLength];
            for (int i = 0; i < aLength; i++)
                result[i] = NextULong();
            return result;
        }

        public string NextString(int aLength)
        {
            return new string(NextChars(aLength));
        }

        public double NextDoubleFull()
        {
            return BitConverter.Int64BitsToDouble(NextLong());
        }

        public float NextFloatFull()
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(NextUInt()), 0);
        }

        public double[] NextDoublesFull(int aLength)
        {
            double[] result = new double[aLength];
            for (int i = 0; i < aLength; i++)
                result[i] = NextDoubleFull();
            return result;
        }

        public double[] NextDoublesFullSafe(int aLength)
        {
            double[] result = new double[aLength];
            for (int i = 0; i < aLength; i++)
                result[i] = NextDoubleFullSafe();
            return result;
        }

        public double[] NextDoubles(int aLength)
        {
            double[] result = new double[aLength];
            for (int i = 0; i < aLength; i++)
                result[i] = NextDouble();
            return result;
        }

        public float[] NextFloatsFull(int aLength)
        {
            float[] result = new float[aLength];
            for (int i = 0; i < aLength; i++)
                result[i] = NextFloatFull();
            return result;
        }

        public float[] NextFloatsFullSafe(int aLength)
        {
            float[] result = new float[aLength];
            for (int i = 0; i < aLength; i++)
                result[i] = NextFloatFullSafe();
            return result;
        }

        public float[] NextFloats(int aLength)
        {
            float[] result = new float[aLength];
            for (int i = 0; i < aLength; i++)
                result[i] = NextFloat();
            return result;
        }

        public double NextDoubleFullSafe()
        {
            for (; ; )
            {
                double d = NextDoubleFull();

                if (Double.IsNaN(d))
                    continue;

                return d;
            }
        }

        public float NextFloatFullSafe()
        {
            for (; ; )
            {
                float f = NextFloatFull();

                if (!Single.IsNaN(f))
                    return f;
            }
        }
    }
}