namespace Lenneth.WebApi.Core.Crypt
{
    /// <summary>
    /// 加密类型
    /// </summary>
    internal enum Type
    {
        Des,
        TripleDes,
        Aes
    }

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
        public ICrypt CreateCrypt(Type cryptType,string key = null)
        {
            ICrypt appCrypt;
            switch (cryptType)
            {
                case Type.Des:
                    appCrypt = new Des(key);
                    break;
                case Type.TripleDes:
                    appCrypt = new TripleDes(key);
                    break;
                case Type.Aes:
                    appCrypt = new Aes(key);
                    break;
                default:
                    appCrypt = new Des(key);
                    break;
            }
            return appCrypt;
        }
    }
}
