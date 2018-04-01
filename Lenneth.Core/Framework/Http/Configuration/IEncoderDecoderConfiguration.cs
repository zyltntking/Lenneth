namespace Lenneth.Core.Framework.Http.Configuration
{
    using Codecs;

    public interface IEncoderDecoderConfiguration
    {
        IEncoder GetEncoder();

        IDecoder GetDecoder();
    }
}