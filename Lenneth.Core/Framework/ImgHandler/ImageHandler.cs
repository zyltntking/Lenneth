using System;
using System.Drawing;
using System.IO;

namespace Lenneth.Core.Framework.ImgHandler
{
    internal abstract class ImageHandler : IImageHandler, IDisposable
    {
        /// <summary>
        /// Image访问器
        /// </summary>
        public Bitmap Image { get; }

        /// <summary>
        /// 获取图片二进制值
        /// </summary>
        public Byte[] GetBytes
        {
            get
            {
                using (var ms = new MemoryStream())
                {
                    Image.Save(ms, Image.RawFormat);
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// 获取图片Base64字符串
        /// </summary>
        public string GetBase64 => Convert.ToBase64String(GetBytes);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="image">图像对象</param>
        public ImageHandler(Bitmap image)
        {
            Image = image;
        }

        /// <summary>
        /// 按比例缩放图片
        /// </summary>
        /// <param name="percent">缩放百分比</param>
        /// <returns>图片</returns>
        public virtual Bitmap Resize(int percent)
        {
            return null;
        }

        /// <summary>
        /// 按固定尺寸缩放图片
        /// </summary>
        /// <param name="width">缩放目标宽</param>
        /// <param name="height">缩放目标高</param>
        /// <returns></returns>
        public virtual Bitmap Resize(int width, int height)
        {
            return null;
        }

        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="quality">质量评估值</param>
        /// <returns></returns>
        public virtual byte[] Optimizer(int quality)
        {
            return null;
        }

        /// <summary>
        /// 添加水印
        /// </summary>
        /// <param name="mark">水印图片</param>
        /// <param name="offsetX">横轴偏移</param>
        /// <param name="offsetY">纵轴偏移</param>
        /// <returns></returns>
        public virtual Bitmap MarkWater(Bitmap mark, int offsetX, int offsetY)
        {
            return null;
        }


        /// <summary>
        /// 执行与释放或重置非托管资源关联的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            Image?.Dispose();
        }

    }
}