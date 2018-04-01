using System.Collections.Generic;
using System.Linq;

namespace Lenneth.Core.Framework.QR.Formater
{
    using Base;

    // ReSharper disable once InconsistentNaming
    public class BitmapByteQrCode : AbstractQrCode
    {
        /// <summary>
        /// Constructor without params to be used in COM Objects connections
        /// </summary>
        public BitmapByteQrCode() { }

        public BitmapByteQrCode(QrCodeData data) : base(data)
        {
        }

        public byte[] GetGraphic(int pixelsPerModule) => GetGraphic(pixelsPerModule, new byte[] { 0x00, 0x00, 0x00 }, new byte[] { 0xFF, 0xFF, 0xFF });

        public byte[] GetGraphic(int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex) => GetGraphic(pixelsPerModule, HexColorToByteArray(darkColorHtmlHex), HexColorToByteArray(lightColorHtmlHex));

        public byte[] GetGraphic(int pixelsPerModule, byte[] darkColorRgb, byte[] lightColorRgb)
        {
            var sideLength = QrCodeData.ModuleMatrix.Count * pixelsPerModule;

            var moduleDark = darkColorRgb.Reverse();
            var moduleLight = lightColorRgb.Reverse();

            var bmp = new List<byte>();

            //header
            bmp.AddRange(new byte[] { 0x42, 0x4D, 0x4C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1A, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00 });

            //width
            bmp.AddRange(IntTo4Byte(sideLength));
            //height
            bmp.AddRange(IntTo4Byte(sideLength));

            //header end
            bmp.AddRange(new byte[] { 0x01, 0x00, 0x18, 0x00 });

            //draw qr code
            var collection = moduleDark as byte[] ?? moduleDark.ToArray();
            var enumerable = moduleLight as byte[] ?? moduleLight.ToArray();
            for (var x = sideLength - 1; x >= 0; x = x - pixelsPerModule)
            {
                for (var pm = 0; pm < pixelsPerModule; pm++)
                {
                    for (var y = 0; y < sideLength; y = y + pixelsPerModule)
                    {
                        var module =
                            QrCodeData.ModuleMatrix[(x + pixelsPerModule) / pixelsPerModule - 1][(y + pixelsPerModule) / pixelsPerModule - 1];
                        for (var i = 0; i < pixelsPerModule; i++)
                        {
                            bmp.AddRange(module ? collection : enumerable);
                        }
                    }
                    if (sideLength % 4 != 0)
                    {
                        for (var i = 0; i < sideLength % 4; i++)
                        {
                            bmp.Add(0x00);
                        }
                    }
                }
            }

            //finalize with terminator
            bmp.AddRange(new byte[] { 0x00, 0x00 });

            return bmp.ToArray();
        }

        private byte[] HexColorToByteArray(string colorString)
        {
            if (colorString.StartsWith("#"))
                colorString = colorString.Substring(1);
            var byteColor = new byte[colorString.Length / 2];
            for (var i = 0; i < byteColor.Length; i++)
                byteColor[2 - i] = byte.Parse(colorString.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture);
            return byteColor;
        }

        private byte[] IntTo4Byte(int inp)
        {
            var bytes = new byte[2];
            unchecked
            {
                bytes[1] = (byte)(inp >> 8);
                bytes[0] = (byte)(inp);
            }
            return bytes;
        }
    }
}