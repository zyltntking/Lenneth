using System.Diagnostics;

namespace Lenneth.Core.Framework.HashLib.Checksum
{
    public static class Crc32Polynomials
    {
        public const uint Ieee8023 = 0xEDB88320;
        public const uint Castagnoli = 0x82F63B78;
        public const uint Koopman = 0xEB31D82E;
        public const uint Crc32Q = 0xD5828281;
    }

    internal class Crc32Ieee : Crc32
    {
        public Crc32Ieee()
            : base(Crc32Polynomials.Ieee8023)
        {
        }
    }

    internal class Crc32Castagnoli : Crc32
    {
        public Crc32Castagnoli()
            : base(Crc32Polynomials.Castagnoli)
        {
        }
    }

    internal class Crc32Koopman : Crc32
    {
        public Crc32Koopman()
            : base(Crc32Polynomials.Koopman)
        {
        }
    }

    internal class Crc32Q : Crc32
    {
        public Crc32Q()
            : base(Crc32Polynomials.Crc32Q)
        {
        }
    }

    internal class Crc32 : Hash, IChecksum, IBlockHash, IHash32
    {
        private readonly uint[] _mCrcTab = new uint[256];

        private uint _mHash;
        private readonly uint _mInitialValue;
        private readonly uint _mFinalXor;

        public Crc32(uint aPolynomial, uint aInitialValue = uint.MaxValue, uint aFinalXor = uint.MaxValue)
            : base(4, 1)
        {
            _mInitialValue = aInitialValue;
            _mFinalXor = aFinalXor;

            GenerateCrcTable(aPolynomial);
        }

        private void GenerateCrcTable(uint aPoly32)
        {
            for (uint i = 0; i < 256; ++i)
            {
                var crc = i;

                for (var j = 0; j < 8; j++)
                {
                    if ((crc & 1) == 1)
                        crc = (crc >> 1) ^ aPoly32;
                    else
                        crc = crc >> 1;
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

            for (var i = aIndex; aLength > 0; i++, aLength--)
                _mHash = (_mHash >> 8) ^ _mCrcTab[(byte)_mHash ^ aData[i]];
        }

        public override HashResult TransformFinal()
        {
            return new HashResult(_mHash ^ _mFinalXor);
        }
    }
}