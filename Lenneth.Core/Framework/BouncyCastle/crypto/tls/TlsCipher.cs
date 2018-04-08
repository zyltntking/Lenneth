using System.IO;

namespace Lenneth.Core.FrameWork.BouncyCastle.crypto.tls
{
    public interface TlsCipher
    {
        int GetPlaintextLimit(int ciphertextLimit);

        /// <exception cref="IOException"></exception>
        byte[] EncodePlaintext(long seqNo, byte type, byte[] plaintext, int offset, int len);

        /// <exception cref="IOException"></exception>
        byte[] DecodeCiphertext(long seqNo, byte type, byte[] ciphertext, int offset, int len);
    }
}
