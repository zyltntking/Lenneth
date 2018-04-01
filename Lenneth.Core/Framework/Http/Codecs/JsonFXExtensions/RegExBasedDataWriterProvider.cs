using JsonFx.Serialization;
using JsonFx.Serialization.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lenneth.Core.Framework.Http.Codecs.JsonFXExtensions
{
    // TODO: This is a copy of the DataWriterProvider in JsonFX. Need to clean it up and move things elsewhere
    internal sealed class RegExBasedDataWriterProvider : IDataWriterProvider
    {
        private readonly IDataWriter _defaultWriter;
        private readonly IDictionary<string, IDataWriter> _writersByExt = new Dictionary<string, IDataWriter>(StringComparer.OrdinalIgnoreCase);
        private readonly IDictionary<string, IDataWriter> _writersByMime = new Dictionary<string, IDataWriter>(StringComparer.OrdinalIgnoreCase);

        public RegExBasedDataWriterProvider(IEnumerable<IDataWriter> writers)
        {
            if (writers == null) return;
            foreach (var writer in writers)
            {
                if (_defaultWriter == null)
                {
                    // TODO: decide less arbitrary way to choose default
                    // without hardcoding value into IDataWriter.
                    // Currently first DataWriter wins default.
                    _defaultWriter = writer;
                }

                foreach (var contentType in writer.ContentType)
                {
                    if (string.IsNullOrEmpty(contentType) ||
                        _writersByMime.ContainsKey(contentType))
                    {
                        continue;
                    }

                    _writersByMime[contentType] = writer;
                }

                foreach (var fileExt in writer.FileExtension)
                {
                    if (string.IsNullOrEmpty(fileExt) ||
                        _writersByExt.ContainsKey(fileExt))
                    {
                        continue;
                    }

                    var ext = NormalizeExtension(fileExt);
                    _writersByExt[ext] = writer;
                }
            }
        }

        public IDataWriter DefaultDataWriter => _defaultWriter;

        public IDataWriter Find(string extension)
        {
            extension = NormalizeExtension(extension);

            return _writersByExt.TryGetValue(extension, out var writer) ? writer : null;
        }

        public IDataWriter Find(string acceptHeader, string contentTypeHeader)
        {
            return (from type in ParseHeaders(acceptHeader, contentTypeHeader) select (from writer in _writersByMime where Regex.Match(type, writer.Key, RegexOptions.Singleline).Success select writer) into readers select readers as KeyValuePair<string, IDataWriter>[] ?? readers.ToArray() into keyValuePairs where keyValuePairs.Any() select keyValuePairs.First().Value).FirstOrDefault();
        }

        private static IEnumerable<string> ParseHeaders(string accept, string contentType)
        {
            string mime;

            // check for a matching accept type
            foreach (var type in SplitTrim(accept, ','))
            {
                mime = DataProviderUtility.ParseMediaType(type);
                if (!string.IsNullOrEmpty(mime))
                {
                    yield return mime;
                }
            }

            // fallback on content-type
            mime = DataProviderUtility.ParseMediaType(contentType);
            if (!string.IsNullOrEmpty(mime))
            {
                yield return mime;
            }
        }

        private static IEnumerable<string> SplitTrim(string source, char ch)
        {
            if (string.IsNullOrEmpty(source))
            {
                yield break;
            }

            var length = source.Length;
            for (int prev = 0, next = 0; prev < length && next >= 0; prev = next + 1)
            {
                next = source.IndexOf(ch, prev);
                if (next < 0)
                {
                    next = length;
                }

                var part = source.Substring(prev, next - prev).Trim();
                if (part.Length > 0)
                {
                    yield return part;
                }
            }
        }

        private static string NormalizeExtension(string extension)
        {
            return string.IsNullOrEmpty(extension) ? string.Empty : Path.GetExtension(extension);

            // ensure is only extension with leading dot
        }
    }
}