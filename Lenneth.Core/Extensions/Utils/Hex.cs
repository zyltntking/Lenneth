namespace Lenneth.Core.Extensions.Utils
{
    public static class Hex
    {
        private static readonly byte[] MTransHexToBin = new byte[255];

        private static readonly char[] MTransBinToHex = new[] { '0', '1', '2', '3', '4', '5', '6', '7',
                                                            '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        static Hex()
        {
            for (int i = '0'; i <= '9'; i++)
                MTransHexToBin[i] = (byte)(i - '0');
            for (int i = 'a'; i <= 'f'; i++)
                MTransHexToBin[i] = (byte)(i - 'a' + 10);
            for (int i = 'A'; i <= 'F'; i++)
                MTransHexToBin[i] = (byte)(i - 'A' + 10);
        }

        public static byte HexToByte(string aStr, bool aAdded_0X = true)
        {
            var index = 0;
            if (aAdded_0X)
                index += 2;

            return (byte)(
                (MTransHexToBin[aStr[index++]] << 4) |
                MTransHexToBin[aStr[index++]]);
        }

        public static ushort HexToUShort(string aStr, bool aAdded_0X = true)
        {
            var index = 0;
            if (aAdded_0X)
                index += 2;

            return (ushort)(
                (MTransHexToBin[aStr[index++]] << 12) |
                (MTransHexToBin[aStr[index++]] << 8) |
                (MTransHexToBin[aStr[index++]] << 4) |
                MTransHexToBin[aStr[index++]]);
        }

        public static uint HexToUInt(string aStr, bool aAdded_0X = true)
        {
            var index = 0;
            if (aAdded_0X)
                index += 2;

            return (uint)(
                (MTransHexToBin[aStr[index++]] << 28) |
                (MTransHexToBin[aStr[index++]] << 24) |
                (MTransHexToBin[aStr[index++]] << 20) |
                (MTransHexToBin[aStr[index++]] << 16) |
                (MTransHexToBin[aStr[index++]] << 12) |
                (MTransHexToBin[aStr[index++]] << 8) |
                (MTransHexToBin[aStr[index++]] << 4) |
                MTransHexToBin[aStr[index++]]);
        }

        public static string ByteToHex(byte aValue, bool aAdd_0X = true)
        {
            if (aAdd_0X)
            {
                return new string(new[]
                {
                    '0', 'x',
                    MTransBinToHex[aValue >> 4],
                    MTransBinToHex[aValue & 0x0F]
                });
            }
            else
            {
                return new string(new[]
                {
                    MTransBinToHex[aValue >> 4],
                    MTransBinToHex[aValue & 0x0F]
                });
            }
        }

        public static string UShortToHex(ushort aValue, bool aAdd_0X = true)
        {
            if (aAdd_0X)
            {
                return new string(new[]
                {
                    '0', 'x',
                    MTransBinToHex[aValue >> 12],
                    MTransBinToHex[(aValue >> 8) & 0x0F],
                    MTransBinToHex[(aValue >> 4) & 0x0F],
                    MTransBinToHex[aValue & 0x0F]
                });
            }
            else
            {
                return new string(new[]
                {
                    MTransBinToHex[aValue >> 12],
                    MTransBinToHex[(aValue >> 8) & 0x0F],
                    MTransBinToHex[(aValue >> 4) & 0x0F],
                    MTransBinToHex[aValue & 0x0F]
                });
            }
        }

        public static string UIntToHex(uint aValue, bool aAdd_0X = true)
        {
            if (aAdd_0X)
            {
                return new string(new[]
                {
                    '0', 'x',
                    MTransBinToHex[aValue >> 28],
                    MTransBinToHex[(aValue >> 24) & 0x0F],
                    MTransBinToHex[(aValue >> 20) & 0x0F],
                    MTransBinToHex[(aValue >> 16) & 0x0F],
                    MTransBinToHex[(aValue >> 12) & 0x0F],
                    MTransBinToHex[(aValue >> 8) & 0x0F],
                    MTransBinToHex[(aValue >> 4) & 0x0F],
                    MTransBinToHex[aValue & 0x0F]
                });
            }
            else
            {
                return new string(new[]
                {
                    MTransBinToHex[aValue >> 28],
                    MTransBinToHex[(aValue >> 24) & 0x0F],
                    MTransBinToHex[(aValue >> 20) & 0x0F],
                    MTransBinToHex[(aValue >> 16) & 0x0F],
                    MTransBinToHex[(aValue >> 12) & 0x0F],
                    MTransBinToHex[(aValue >> 8) & 0x0F],
                    MTransBinToHex[(aValue >> 4) & 0x0F],
                    MTransBinToHex[aValue & 0x0F]
                });
            }
        }
    }
}