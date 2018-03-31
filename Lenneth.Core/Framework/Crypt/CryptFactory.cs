namespace Lenneth.Core.Framework.Crypt
{
    /// <summary>
    /// 加密器工厂
    /// </summary>
    internal sealed class CryptFactory
    {
        /// <summary>
        /// 创建加密程序
        /// </summary>
        /// <param name="cryptType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public ICrypt CreateCrypt(string cryptType,string key = null)
        {
            ICrypt appCrypt;
            switch (cryptType)
            {
                case "DES":
                    appCrypt = new DES(key);
                    break;
                case "TripleDES":
                    appCrypt = new TripleDES(key);
                    break;
                case "AES":
                    appCrypt = new AES(key);
                    break;
                default:
                    appCrypt = new DES(key);
                    break;
            }
            return appCrypt;
        }
    }
}
