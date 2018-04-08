using System.Diagnostics;

namespace Lenneth.Core.Framework.HashLib.Checksum
{
    public static class Crc64Polynomials
    {
        public const ulong Iso = 0xD800000000000000;
        public const ulong Ecma182 = 0xC96C5795D7870F42;
    }

    internal class Crc64Iso : Crc64
    {
        public Crc64Iso()
            : base(Crc64Polynomials.Iso)
        {
        }
    }

    internal class Crc64Ecma : Crc64
    {
        public Crc64Ecma()
            : base(Crc64Polynomials.Ecma182)
        {
        }
    }

    internal class Crc64 : Hash, IChecksum, IBlockHash, IHash64
    {
        private readonly ulong[] _mCrcTab = new ulong[256];

        private ulong _mHash;
        private readonly ulong _mInitialValue;
        private readonly ulong _mFinalXor;

        public Crc64(ulong aPolynomial, ulong aInitialValue = ulong.MaxValue, ulong aFinalXor = ulong.MaxValue)
            : base(8, 1)
        {
            _mInitialValue = aInitialValue;
            _mFinalXor = aFinalXor;

            GenerateCrcTable(aPolynomial);
        }

        private void GenerateCrcTable(ulong aPoly64)
        {
            for (uint i = 0; i < 256; ++i)
            {
                ulong crc = i;

                for (uint j = 0; j < 8; ++j)
                {
                    if ((crc & 1) == 1)
                        crc = (crc >> 1) ^ aPoly64;
                    else
                        crc >>= 1;
                }

                _mCrcTab[i] = crc;
            }
        }

        public override void Initialize()
        {
            _mHash = _mInitialValue;
        }

        public override void TransformBytes(byte[] aData, int aIndex, int aLength)
        {
            Debug.Assert(aIndex >= 0);
            Debug.Assert(aLength >= 0);
            Debug.Assert(aIndex + aLength <= aData.Length);

            for (int i = aIndex; aLength > 0; i++, aLength--)
                _mHash = (_mHash >> 8) ^ _mCrcTab[(byte)_mHash ^ aData[i]];
        }

        public override HashResult TransformFinal()
        {
            return new HashResult(_mHash ^ _mFinalXor);
        }
    }
}