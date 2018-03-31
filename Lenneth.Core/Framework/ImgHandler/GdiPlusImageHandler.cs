using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Lenneth.Core.Framework.ImgHandler
{
    internal sealed class GdiPlusImageHandler : ImageHandler
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="image">图像对象</param>
        public GdiPlusImageHandler(Bitmap image) : base(image)
        {
        }

        /// <summary>
        /// 按比例缩放图片
        /// </summary>
        /// <param name="percent">缩放百分比</param>
        /// <returns>图片</returns>
        public override Bitmap Resize(int percent)
        {
            var width = Image.Width * percent;
            var height = Image.Height * percent;
            using (var result = new Bitmap(width, height))
            {
                using (var outImgG = Graphics.FromImage(result))
                {
                    outImgG.Clear(Color.Transparent);
                    outImgG.DrawImage(Image, new Rectangle(0, 0, width, height), new Rectangle(0, 0, Image.Width, Image.Height), GraphicsUnit.Pixel);
                }
                return result;
            }
        }

        /// <summary>
        /// 按固定尺寸缩放图片
        /// </summary>
        /// <param name="width">缩放目标宽</param>
        /// <param name="height">缩放目标高</param>
        /// <returns></returns>
        public override Bitmap Resize(int width, int height)
        {
            using (var result = new Bitmap(width, height))
            {
                using (var outImgG = Graphics.FromImage(result))
                {
                    outImgG.Clear(Color.Transparent);
                    outImgG.DrawImage(Image, new Rectangle(0, 0, width, height), new Rectangle(0, 0, Image.Width, Image.Height), GraphicsUnit.Pixel);
                }
                return result;
            }
        }

        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="quality">质量评估值</param>
        /// <returns></returns>
        public override byte[] Optimizer(int quality)
        {
            using (var ep = new EncoderParameters())
            {
                var qy = new long[1];
                qy[0] = quality;

                var eParam = new EncoderParameter(Encoder.Quality, qy);
                ep.Param[0] = eParam;

                try
                {
                    var arrayIci = ImageCodecInfo.GetImageDecoders();
                    var jpegIcIinfo = arrayIci.FirstOrDefault(t => t.FormatDescription.Equals(Image.RawFormat.ToString()));

                    using (var ms = new MemoryStream())
                    {
                        if (jpegIcIinfo != null)
                        {
                            Image.Save(ms, jpegIcIinfo, ep);
                        }
                        else
                        {
                            Image.Save(ms, Image.RawFormat);
                        }
                        return ms.ToArray();
                    }
                }
                catch
                {
                    return null;
                }
            }

        }

        public override Bitmap MarkWater(Bitmap mark, int offsetX, int offsetY)
        {
            using (var result = new Bitmap(Image))
            {
                using (var outImgG = Graphics.FromImage(result))
                {
                    outImgG.DrawImage(mark, new Rectangle(offsetX, offsetY, mark.Width, mark.Height));
                }
                return result;
            }

        }
    }
}