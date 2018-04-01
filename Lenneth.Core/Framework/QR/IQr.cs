namespace Lenneth.Core.Framework.QR
{
    public interface IQr
    {
        /// <summary>
        /// 生成为base64字符串
        /// </summary>
        /// <param name="content">二维码内容</param>
        /// <param name="pixelsPerModule">单元像素</param>
        /// <returns>base64字符串</returns>
        string GeneratorToBase64(string content, int pixelsPerModule = 20);

        /// <summary>
        /// 生成为Bitmap byte
        /// </summary>
        /// <param name="content">二维码内容</param>
        /// <param name="pixelsPerModule">单元像素</param>
        /// <returns></returns>
        byte[] GeneratorToBitmapByte(string content, int pixelsPerModule = 20);

        /// <summary>
        /// 生成为SVG字符串
        /// </summary>
        /// <param name="content">二维码内容</param>
        /// <param name="pixelsPerModule">单元像素</param>
        /// <returns></returns>
        string GeneratorToSvg(string content, int pixelsPerModule = 20);

        /// <summary>
        /// 生成为ascii字符串
        /// </summary>
        /// <param name="content">二维码内容</param>
        /// <param name="pixelsPerModule">单元像素</param>
        /// <returns></returns>
        string GeneratorToAscii(string content, int pixelsPerModule = 20);

        /// <summary>
        /// 生成为ascii字符串数组
        /// </summary>
        /// <param name="content">二维码内容</param>
        /// <param name="pixelsPerModule">单元像素</param>
        /// <returns></returns>
        string[] GeneratorToAsciiArr(string content, int pixelsPerModule = 20);
    }
}