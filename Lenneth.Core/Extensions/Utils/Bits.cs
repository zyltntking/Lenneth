using System.Diagnostics;

namespace Lenneth.Core.Extensions.Utils
{
    public static class Bits
    {
        public static bool IsSet(byte aByte, int aBitIndex)
        {
            Debug.Assert(aBitIndex >= 0);
            Debug.Assert(aBitIndex <= 7);

            return (aByte & (1 << aBitIndex)) != 0;
        }

        public static void SetBit(ref byte aByte, int aBitIndex, bool aBitValue)
        {
            Debug.Assert(aBitIndex >= 0);
            Debug.Assert(aBitIndex <= 7);

            if (aBitValue)
                aByte = (byte)(aByte | (1 << aBitIndex));
            else
                aByte = (byte)(aByte & ~(1 << aBitIndex));
        }

        public static bool IsSet(ushort aUshort, int aBitIndex)
        {
            Debug.Assert(aBitIndex >= 0);
            Debug.Assert(aBitIndex <= 15);

            return (aUshort & (1 << aBitIndex)) != 0;
        }

        public static void SetBit(ref ushort aUshort, int aBitIndex, bool aBitValue)
        {
            Debug.Assert(aBitIndex >= 0);
            Debug.Assert(aBitIndex <= 15);

            if (aBitValue)
                aUshort = (ushort)(aUshort | (1 << aBitIndex));
            else
                aUshort = (ushort)(aUshort & ~(1 << aBitIndex));
        }

        public static bool IsSet(uint aUint, int aBitIndex)
        {
            Debug.Assert(aBitIndex >= 0);
            Debug.Assert(aBitIndex <= 31);

            return (aUint & (1 << aBitIndex)) != 0;
        }

        public static void SetBit(ref uint aUint, int aBitIndex, bool aBitValue)
        {
            Debug.Assert(aBitIndex >= 0);
            Debug.Assert(aBitIndex <= 31);

            if (aBitValue)
                aUint = aUint | (1U << aBitIndex);
            else
                aUint = aUint & ~(1U << aBitIndex);
        }

        public static uint RotateLeft(uint aUint, int aN)
        {
            Debug.Assert(aN >= 0);

            return (aUint << aN) | (aUint >> (32 - aN));
        }

        public static ulong RotateLeft(ulong aUlong, int aN)
        {
            Debug.Assert(aN >= 0);

            return (aUlong << aN) | (aUlong >> (64 - aN));
        }

        public static uint RotateRight(uint aUint, int aN)
        {
            Debug.Assert(aN >= 0);

            return (aUint >> aN) | (aUint << (32 - aN));
        }

        public static ulong RotateRight(ulong aUlong, int aN)
        {
            Debug.Assert(aN >= 0);

            return (aUlong >> aN) | (aUlong << (64 - aN));
        }
    }
}