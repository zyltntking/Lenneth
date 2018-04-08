namespace Lenneth.Core.FrameWork.BouncyCastle.crypto.tls
{
    public interface TlsHandshakeHash
        :   IDigest
    {
        void Init(TlsContext context);

        TlsHandshakeHash NotifyPrfDetermined();

        void TrackHashAlgorithm(byte hashAlgorithm);

        void SealHashAlgorithms();

        TlsHandshakeHash StopTracking();

        IDigest ForkPrfHash();

        byte[] GetFinalHash(byte hashAlgorithm);
    }
}
