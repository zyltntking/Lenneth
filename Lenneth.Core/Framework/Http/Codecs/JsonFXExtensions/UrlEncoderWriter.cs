using JsonFx.Model;
using JsonFx.Serialization;
using System.Collections.Generic;
using Lenneth.Core.Framework.Http.Client;

namespace Lenneth.Core.Framework.Http.Codecs.JsonFXExtensions
{
    internal sealed class UrlEncoderWriter : ModelWriter
    {
        private readonly string[] _contentTypes;

        public UrlEncoderWriter(DataWriterSettings settings, params string[] contentTypes) : base(settings)
        {
            _contentTypes = contentTypes;
        }

        protected override ITextFormatter<ModelTokenType> GetFormatter()
        {
            return new UrlEncoderTextFormatter();
        }

        public override IEnumerable<string> ContentType
        {
            get
            {
                if (_contentTypes != null)
                {
                    foreach (var contentType in _contentTypes)
                    {
                        yield return contentType;
                    }
                    yield break;
                }

                yield return HttpContentTypes.ApplicationXWwwFormUrlEncoded;
            }
        }

        public override IEnumerable<string> FileExtension => new List<string>();
    }
}