namespace Lenneth.Core.Framework.HashLib.Crypto
{
    internal class Md5 : MDBase
    {
        public Md5()
            : base(4, 16)
        {
        }

        protected override void TransformBlock(byte[] aData, int aIndex)
        {
            var data0 = Converters.ConvertBytesToUInt(aData, aIndex + 4 * 0);
            var data1 = Converters.ConvertBytesToUInt(aData, aIndex + 4 * 1);
            var data2 = Converters.ConvertBytesToUInt(aData, aIndex + 4 * 2);
            var data3 = Converters.ConvertBytesToUInt(aData, aIndex + 4 * 3);
            var data4 = Converters.ConvertBytesToUInt(aData, aIndex + 4 * 4);
            var data5 = Converters.ConvertBytesToUInt(aData, aIndex + 4 * 5);
            var data6 = Converters.ConvertBytesToUInt(aData, aIndex + 4 * 6);
            var data7 = Converters.ConvertBytesToUInt(aData, aIndex + 4 * 7);
            var data8 = Converters.ConvertBytesToUInt(aData, aIndex + 4 * 8);
            var data9 = Converters.ConvertBytesToUInt(aData, aIndex + 4 * 9);
            var data10 = Converters.ConvertBytesToUInt(aData, aIndex + 4 * 10);
            var data11 = Converters.ConvertBytesToUInt(aData, aIndex + 4 * 11);
            var data12 = Converters.ConvertBytesToUInt(aData, aIndex + 4 * 12);
            var data13 = Converters.ConvertBytesToUInt(aData, aIndex + 4 * 13);
            var data14 = Converters.ConvertBytesToUInt(aData, aIndex + 4 * 14);
            var data15 = Converters.ConvertBytesToUInt(aData, aIndex + 4 * 15);

            var a = m_state[0];
            var b = m_state[1];
            var c = m_state[2];
            var d = m_state[3];

            a = data0 + 0xd76aa478 + a + ((b & c) | (~b & d));
            a = ((a << 7) | (a >> (32 - 7))) + b;
            d = data1 + 0xe8c7b756 + d + ((a & b) | (~a & c));
            d = ((d << 12) | (d >> (32 - 12))) + a;
            c = data2 + 0x242070db + c + ((d & a) | (~d & b));
            c = ((c << 17) | (c >> (32 - 17))) + d;
            b = data3 + 0xc1bdceee + b + ((c & d) | (~c & a));
            b = ((b << 22) | (b >> (32 - 22))) + c;
            a = data4 + 0xf57c0faf + a + ((b & c) | (~b & d));
            a = ((a << 7) | (a >> (32 - 7))) + b;
            d = data5 + 0x4787c62a + d + ((a & b) | (~a & c));
            d = ((d << 12) | (d >> (32 - 12))) + a;
            c = data6 + 0xa8304613 + c + ((d & a) | (~d & b));
            c = ((c << 17) | (c >> (32 - 17))) + d;
            b = data7 + 0xfd469501 + b + ((c & d) | (~c & a));
            b = ((b << 22) | (b >> (32 - 22))) + c;
            a = data8 + 0x698098d8 + a + ((b & c) | (~b & d));
            a = ((a << 7) | (a >> (32 - 7))) + b;
            d = data9 + 0x8b44f7af + d + ((a & b) | (~a & c));
            d = ((d << 12) | (d >> (32 - 12))) + a;
            c = data10 + 0xffff5bb1 + c + ((d & a) | (~d & b));
            c = ((c << 17) | (c >> (32 - 17))) + d;
            b = data11 + 0x895cd7be + b + ((c & d) | (~c & a));
            b = ((b << 22) | (b >> (32 - 22))) + c;
            a = data12 + 0x6b901122 + a + ((b & c) | (~b & d));
            a = ((a << 7) | (a >> (32 - 7))) + b;
            d = data13 + 0xfd987193 + d + ((a & b) | (~a & c));
            d = ((d << 12) | (d >> (32 - 12))) + a;
            c = data14 + 0xa679438e + c + ((d & a) | (~d & b));
            c = ((c << 17) | (c >> (32 - 17))) + d;
            b = data15 + 0x49b40821 + b + ((c & d) | (~c & a));
            b = ((b << 22) | (b >> (32 - 22))) + c;

            a = data1 + 0xf61e2562 + a + ((b & d) | (c & ~d));
            a = ((a << 5) | (a >> (32 - 5))) + b;
            d = data6 + 0xc040b340 + d + ((a & c) | (b & ~c));
            d = ((d << 9) | (d >> (32 - 9))) + a;
            c = data11 + 0x265e5a51 + c + ((d & b) | (a & ~b));
            c = ((c << 14) | (c >> (32 - 14))) + d;
            b = data0 + 0xe9b6c7aa + b + ((c & a) | (d & ~a));
            b = ((b << 20) | (b >> (32 - 20))) + c;
            a = data5 + 0xd62f105d + a + ((b & d) | (c & ~d));
            a = ((a << 5) | (a >> (32 - 5))) + b;
            d = data10 + 0x2441453 + d + ((a & c) | (b & ~c));
            d = ((d << 9) | (d >> (32 - 9))) + a;
            c = data15 + 0xd8a1e681 + c + ((d & b) | (a & ~b));
            c = ((c << 14) | (c >> (32 - 14))) + d;
            b = data4 + 0xe7d3fbc8 + b + ((c & a) | (d & ~a));
            b = ((b << 20) | (b >> (32 - 20))) + c;
            a = data9 + 0x21e1cde6 + a + ((b & d) | (c & ~d));
            a = ((a << 5) | (a >> (32 - 5))) + b;
            d = data14 + 0xc33707d6 + d + ((a & c) | (b & ~c));
            d = ((d << 9) | (d >> (32 - 9))) + a;
            c = data3 + 0xf4d50d87 + c + ((d & b) | (a & ~b));
            c = ((c << 14) | (c >> (32 - 14))) + d;
            b = data8 + 0x455a14ed + b + ((c & a) | (d & ~a));
            b = ((b << 20) | (b >> (32 - 20))) + c;
            a = data13 + 0xa9e3e905 + a + ((b & d) | (c & ~d));
            a = ((a << 5) | (a >> (32 - 5))) + b;
            d = data2 + 0xfcefa3f8 + d + ((a & c) | (b & ~c));
            d = ((d << 9) | (d >> (32 - 9))) + a;
            c = data7 + 0x676f02d9 + c + ((d & b) | (a & ~b));
            c = ((c << 14) | (c >> (32 - 14))) + d;
            b = data12 + 0x8d2a4c8a + b + ((c & a) | (d & ~a));
            b = ((b << 20) | (b >> (32 - 20))) + c;

            a = data5 + 0xfffa3942 + a + (b ^ c ^ d);
            a = ((a << 4) | (a >> (32 - 4))) + b;
            d = data8 + 0x8771f681 + d + (a ^ b ^ c);
            d = ((d << 11) | (d >> (32 - 11))) + a;
            c = data11 + 0x6d9d6122 + c + (d ^ a ^ b);
            c = ((c << 16) | (c >> (32 - 16))) + d;
            b = data14 + 0xfde5380c + b + (c ^ d ^ a);
            b = ((b << 23) | (b >> (32 - 23))) + c;
            a = data1 + 0xa4beea44 + a + (b ^ c ^ d);
            a = ((a << 4) | (a >> (32 - 4))) + b;
            d = data4 + 0x4bdecfa9 + d + (a ^ b ^ c);
            d = ((d << 11) | (d >> (32 - 11))) + a;
            c = data7 + 0xf6bb4b60 + c + (d ^ a ^ b);
            c = ((c << 16) | (c >> (32 - 16))) + d;
            b = data10 + 0xbebfbc70 + b + (c ^ d ^ a);
            b = ((b << 23) | (b >> (32 - 23))) + c;
            a = data13 + 0x289b7ec6 + a + (b ^ c ^ d);
            a = ((a << 4) | (a >> (32 - 4))) + b;
            d = data0 + 0xeaa127fa + d + (a ^ b ^ c);
            d = ((d << 11) | (d >> (32 - 11))) + a;
            c = data3 + 0xd4ef3085 + c + (d ^ a ^ b);
            c = ((c << 16) | (c >> (32 - 16))) + d;
            b = data6 + 0x4881d05 + b + (c ^ d ^ a);
            b = ((b << 23) | (b >> (32 - 23))) + c;
            a = data9 + 0xd9d4d039 + a + (b ^ c ^ d);
            a = ((a << 4) | (a >> (32 - 4))) + b;
            d = data12 + 0xe6db99e5 + d + (a ^ b ^ c);
            d = ((d << 11) | (d >> (32 - 11))) + a;
            c = data15 + 0x1fa27cf8 + c + (d ^ a ^ b);
            c = ((c << 16) | (c >> (32 - 16))) + d;
            b = data2 + 0xc4ac5665 + b + (c ^ d ^ a);
            b = ((b << 23) | (b >> (32 - 23))) + c;

            a = data0 + 0xf4292244 + a + (c ^ (b | ~d));
            a = ((a << 6) | (a >> (32 - 6))) + b;
            d = data7 + 0x432aff97 + d + (b ^ (a | ~c));
            d = ((d << 10) | (d >> (32 - 10))) + a;
            c = data14 + 0xab9423a7 + c + (a ^ (d | ~b));
            c = ((c << 15) | (c >> (32 - 15))) + d;
            b = data5 + 0xfc93a039 + b + (d ^ (c | ~a));
            b = ((b << 21) | (b >> (32 - 21))) + c;
            a = data12 + 0x655b59c3 + a + (c ^ (b | ~d));
            a = ((a << 6) | (a >> (32 - 6))) + b;
            d = data3 + 0x8f0ccc92 + d + (b ^ (a | ~c));
            d = ((d << 10) | (d >> (32 - 10))) + a;
            c = data10 + 0xffeff47d + c + (a ^ (d | ~b));
            c = ((c << 15) | (c >> (32 - 15))) + d;
            b = data1 + 0x85845dd1 + b + (d ^ (c | ~a));
            b = ((b << 21) | (b >> (32 - 21))) + c;
            a = data8 + 0x6fa87e4f + a + (c ^ (b | ~d));
            a = ((a << 6) | (a >> (32 - 6))) + b;
            d = data15 + 0xfe2ce6e0 + d + (b ^ (a | ~c));
            d = ((d << 10) | (d >> (32 - 10))) + a;
            c = data6 + 0xa3014314 + c + (a ^ (d | ~b));
            c = ((c << 15) | (c >> (32 - 15))) + d;
            b = data13 + 0x4e0811a1 + b + (d ^ (c | ~a));
            b = ((b << 21) | (b >> (32 - 21))) + c;
            a = data4 + 0xf7537e82 + a + (c ^ (b | ~d));
            a = ((a << 6) | (a >> (32 - 6))) + b;
            d = data11 + 0xbd3af235 + d + (b ^ (a | ~c));
            d = ((d << 10) | (d >> (32 - 10))) + a;
            c = data2 + 0x2ad7d2bb + c + (a ^ (d | ~b));
            c = ((c << 15) | (c >> (32 - 15))) + d;
            b = data9 + 0xeb86d391 + b + (d ^ (c | ~a));
            b = ((b << 21) | (b >> (32 - 21))) + c;

            m_state[0] += a;
            m_state[1] += b;
            m_state[2] += c;
            m_state[3] += d;
        }
    }
}