using JsonFx.Serialization;
using JsonFx.Serialization.Providers;
using System.Text;

namespace Lenneth.Core.Framework.Http.Codecs
{
    internal sealed class DefaultEncoder : IEncoder
    {
        private readonly IDataWriterProvider _dataWriterProvider;

        public DefaultEncoder(IDataWriterProvider dataWriterProvider)
        {
            _dataWriterProvider = dataWriterProvider;
        }

        public byte[] Encode(object input, string contentType)
        {
            if (input is string s)
            {
                return Encoding.UTF8.GetBytes(s);
            }

            var serializer = _dataWriterProvider.Find(contentType, contentType);

            if (serializer == null)
            {
                throw new SerializationException("The encoding requested does not have a corresponding encoder");
            }

            var serialized = serializer.Write(input);

            return Encoding.UTF8.GetBytes(serialized);
        }
    }
}