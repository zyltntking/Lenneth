using System;
using System.Drawing;

namespace Lenneth.Core.Framework.ImgHandler
{
    public interface IImageHandler
    {
        /// <summary>
        /// 获取bitmap实例
        /// </summary>
        Bitmap Image { get; }

        /// <summary>
        /// 获取二进制值
        /// </summary>
        Byte[] GetBytes { get; }

        /// <summary>
        /// 获取base64字符串
        /// </summary>
        string GetBase64 { get; }

        /// <summary>
        /// 按比例缩放图片
        /// </summary>
        /// <param name="percent">缩放百分比</param>
        /// <returns>图片</returns>
        Bitmap Resize(int percent);

        /// <summary>
        /// 按固定尺寸缩放图片
        /// </summary>
        /// <param name="width">缩放目标宽</param>
        /// <param name="height">缩放目标高</param>
        /// <returns></returns>
        Bitmap Resize(int width, int height);

        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="quality">质量评估值</param>
        /// <returns></returns>
        byte[] Optimizer(int quality);

        /// <summary>
        /// 添加水印
        /// </summary>
        /// <param name="mark">水印图片</param>
        /// <param name="offsetX">横轴偏移</param>
        /// <param name="offsetY">纵轴偏移</param>
        /// <returns></returns>
        Bitmap MarkWater(Bitmap mark,int offsetX,int offsetY);
    }
}