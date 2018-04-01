using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Lenneth.Core.Framework.QR.Formater
{
    using Base;

    internal class Base64QrCode : AbstractQrCode
    {
        private QrCode Qr { get; }

        /// <summary>
        /// Constructor without params to be used in COM Objects connections
        /// </summary>
        public Base64QrCode() => Qr = new QrCode();

        public Base64QrCode(QrCodeData data) : base(data) => Qr = new QrCode(data);

        public override void SetQrCodeData(QrCodeData data) => Qr.SetQrCodeData(data);

        public string GetGraphic(int pixelsPerModule) => GetGraphic(pixelsPerModule, Color.Black, Color.White);

        public string GetGraphic(int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex, bool drawQuietZones = true, ImageType imgType = ImageType.Png) => GetGraphic(pixelsPerModule, ColorTranslator.FromHtml(darkColorHtmlHex), ColorTranslator.FromHtml(lightColorHtmlHex), drawQuietZones, imgType);

        public string GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, bool drawQuietZones = true, ImageType imgType = ImageType.Png)
        {
            string base64;
            using (var bmp = Qr.GetGraphic(pixelsPerModule, darkColor, lightColor, drawQuietZones))
            {
                base64 = BitmapToBase64(bmp, imgType);
            }
            return base64;
        }

        public string GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, Bitmap icon, int iconSizePercent = 15, int iconBorderWidth = 6, bool drawQuietZones = true, ImageType imgType = ImageType.Png)
        {
            string base64;
            using (var bmp = Qr.GetGraphic(pixelsPerModule, darkColor, lightColor, icon, iconSizePercent, iconBorderWidth, drawQuietZones))
            {
                base64 = BitmapToBase64(bmp, imgType);
            }
            return base64;
        }

        private string BitmapToBase64(Image bmp, ImageType imgType)
        {
            string base64;
            ImageFormat iFormat;
            switch (imgType)
            {
                case ImageType.Png:
                    iFormat = ImageFormat.Png;
                    break;

                case ImageType.Jpeg:
                    iFormat = ImageFormat.Jpeg;
                    break;

                case ImageType.Gif:
                    iFormat = ImageFormat.Gif;
                    break;

                default:
                    iFormat = ImageFormat.Png;
                    break;
            }
            using (var memoryStream = new MemoryStream())
            {
                bmp.Save(memoryStream, iFormat);
                base64 = Convert.ToBase64String(memoryStream.ToArray(), Base64FormattingOptions.None);
            }
            return base64;
        }

        internal enum ImageType
        {
            Gif,
            Jpeg,
            Png
        }
    }
}