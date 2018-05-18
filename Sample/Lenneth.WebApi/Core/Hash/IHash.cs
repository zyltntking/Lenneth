namespace Lenneth.WebApi.Core.Hash
{
    /// <summary>
    /// 散列类接口
    /// </summary>
    public interface IHash
    {
        /// <summary>
        /// 计算字符串散列值
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        string ComputeHash(string input);
    }
}