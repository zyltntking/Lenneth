namespace Lenneth.Core.FrameWork.BouncyCastle.util.encoders
{
    /// <summary>
    /// Translator interface.
    /// </summary>
    public interface ITranslator
    {
        int GetEncodedBlockSize();

        int Encode(byte[] input, int inOff, int length, byte[] outBytes, int outOff);

        int GetDecodedBlockSize();

        int Decode(byte[] input, int inOff, int length, byte[] outBytes, int outOff);
    }

}
