namespace Lenneth.Core.FrameWork.BouncyCastle.crypto.tls
{
    public interface TlsPskIdentityManager
    {
        byte[] GetHint();

        byte[] GetPsk(byte[] identity);
    }
}
