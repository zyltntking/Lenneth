using System.Text.RegularExpressions;

namespace Lenneth.Core.Framework.Utility
{
    /// <summary>
    /// 人民币转换类
    /// </summary>
    public class RmbUtility
    {
        /// <summary>
        /// 转换人民币数值为大写金额
        /// </summary>
        /// <param name="number">数值</param>
        /// <returns>金额字符串</returns>
        public string ConvertToChinese(decimal number)
        {
            var str = number.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var d = Regex.Replace(str, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var r = Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
            return r;
        }
    }
}