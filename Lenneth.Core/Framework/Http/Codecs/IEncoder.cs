namespace Lenneth.Core.Framework.Http.Codecs
{
    public interface IEncoder
    {
        byte[] Encode(object input, string contentType);
    }
}