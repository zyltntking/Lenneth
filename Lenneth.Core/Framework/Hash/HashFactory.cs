namespace Lenneth.Core.Framework.Hash
{
    /// <summary>
    /// 散列类型
    /// </summary>
    internal enum Type
    {
        MD5,
        Sha1,
        Sha256,
        Sha384,
        Sha512,
        RipEmd160
    }

    /// <summary>
    /// 散列工厂
    /// </summary>
    internal sealed class HashFactory
    {
        /// <summary>
        /// 散列服务创造器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IHash CreateHash(Type type)
        {
            IHash hash;
            switch (type)
            {
                case Type.MD5:
                    hash = new MD5Hash();
                    break;

                case Type.Sha1:
                    hash = new Sha1Hash();
                    break;

                case Type.Sha256:
                    hash = new Sha256Hash();
                    break;

                case Type.Sha384:
                    hash = new Sha384Hash();
                    break;

                case Type.Sha512:
                    hash = new Sha512Hash();
                    break;

                case Type.RipEmd160:
                    hash = new RipEmd160Hash();
                    break;

                default:
                    hash = new MD5Hash();
                    break;
            }
            return hash;
        }
    }
}