namespace Lenneth.Core.Framework.Hash
{
    /// <summary>
    /// 加密接口实例
    /// </summary>
    abstract class Hash:IHash
    {
        /// <summary>
        /// 计算字符串散列值
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public virtual string ComputeHash(string input) => input;
    }
}
