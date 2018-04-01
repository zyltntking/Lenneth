namespace Lenneth.Core.Framework.QR
{
    using Base;
    using Formater;

    internal class QrWarpper : IQr
    {
        /// <summary>
        /// 转换为base64字符串
        /// </summary>
        /// <param name="content">二维码内容</param>
        /// <param name="pixelsPerModule">单元像素</param>
        /// <returns>base64字符串</returns>
        public string GeneratorToBase64(string content,int pixelsPerModule = 20)
        {
            using (var qrGenerator = new QrCodeGenerator())
            {
                using (var qrCodeData = qrGenerator.CreateQrCode(content, QrCodeGenerator.EccLevel.Q))
                {
                    using (var qrCode = new Base64QrCode(qrCodeData))
                    {
                        return qrCode.GetGraphic(pixelsPerModule);
                    }
                }
            }
        }

        /// <summary>
        /// 转换为Bitmap byte
        /// </summary>
        /// <param name="content">二维码内容</param>
        /// <param name="pixelsPerModule">单元像素</param>
        /// <returns></returns>
        public byte[] GeneratorToBitmapByte(string content, int pixelsPerModule = 20)
        {
            using (var qrGenerator = new QrCodeGenerator())
            {
                using (var qrCodeData = qrGenerator.CreateQrCode(content, QrCodeGenerator.EccLevel.Q))
                {
                    using (var qrCode = new BitmapByteQrCode(qrCodeData))
                    {
                        return qrCode.GetGraphic(pixelsPerModule);
                    }
                }
            }
        }

        /// <summary>
        /// 生成为SVG字符串
        /// </summary>
        /// <param name="content">二维码内容</param>
        /// <param name="pixelsPerModule">单元像素</param>
        /// <returns></returns>
        public string GeneratorToSvg(string content, int pixelsPerModule = 20)
        {
            using (var qrGenerator = new QrCodeGenerator())
            {
                using (var qrCodeData = qrGenerator.CreateQrCode(content, QrCodeGenerator.EccLevel.Q))
                {
                    using (var qrCode = new SvgQrCode(qrCodeData))
                    {
                        return qrCode.GetGraphic(pixelsPerModule);
                    }
                }
            }
        }

        /// <summary>
        /// 生成为ascii字符串
        /// </summary>
        /// <param name="content">二维码内容</param>
        /// <param name="pixelsPerModule">单元像素</param>
        /// <returns></returns>
        public string GeneratorToAscii(string content, int pixelsPerModule = 20)
        {
            using (var qrGenerator = new QrCodeGenerator())
            {
                using (var qrCodeData = qrGenerator.CreateQrCode(content, QrCodeGenerator.EccLevel.Q))
                {
                    using (var qrCode = new AsciiQrCode(qrCodeData))
                    {
                        return qrCode.GetGraphic(pixelsPerModule);
                    }
                }
            }
        }

        /// <summary>
        /// 生成为ascii字符串数组
        /// </summary>
        /// <param name="content">二维码内容</param>
        /// <param name="pixelsPerModule">单元像素</param>
        /// <returns></returns>
        public string[] GeneratorToAsciiArr(string content, int pixelsPerModule = 20)
        {
            using (var qrGenerator = new QrCodeGenerator())
            {
                using (var qrCodeData = qrGenerator.CreateQrCode(content, QrCodeGenerator.EccLevel.Q))
                {
                    using (var qrCode = new AsciiQrCode(qrCodeData))
                    {
                        return qrCode.GetLineByLineGraphic(pixelsPerModule);
                    }
                }
            }
        }
    }
}