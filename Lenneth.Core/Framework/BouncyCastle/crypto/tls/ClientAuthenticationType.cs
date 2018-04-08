namespace Lenneth.Core.FrameWork.BouncyCastle.crypto.tls
{
    public abstract class ClientAuthenticationType
    {
        /*
         * RFC 5077 4
         */
        public const byte anonymous = 0;
        public const byte certificate_based = 1;
        public const byte psk = 2;
    }
}
