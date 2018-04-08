using System.Diagnostics;

namespace Lenneth.Core.Framework.HashLib.Checksum
{
    internal class Adler32 : Hash, IChecksum, IBlockHash, IHash32
    {
        private const uint ModAdler = 65521;

        private uint _mA;
        private uint _mB;

        public Adler32()
            : base(4, 1)
        {
        }

        public override void Initialize()
        {
            _mA = 1;
            _mB = 0;
        }

        public override void TransformBytes(byte[] aData, int aIndex, int aLength)
        {
            Debug.Assert(aIndex >= 0);
            Debug.Assert(aLength >= 0);
            Debug.Assert(aIndex + aLength <= aData.Length);

            for (int i = aIndex; aLength > 0; i++, aLength--)
            {
                _mA = (_mA + aData[i]) % ModAdler;
                _mB = (_mB + _mA) % ModAdler;
            }
        }

        public override HashResult TransformFinal()
        {
            return new HashResult((_mB << 16) | _mA);
        }
    }
}