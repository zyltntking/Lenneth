using System.Drawing;

namespace Lenneth.Core.Framework.ImgHandler
{
    public interface IImageHandler
    {
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