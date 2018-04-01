using JsonFx.Serialization;
using JsonFx.Serialization.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lenneth.Core.Framework.Http.Codecs.JsonFXExtensions
{
    internal sealed class RegExBasedDataReaderProvider : IDataReaderProvider
    {
        private readonly IDictionary<string, IDataReader> _readersByMime = new Dictionary<string, IDataReader>(StringComparer.OrdinalIgnoreCase);

        public RegExBasedDataReaderProvider(IEnumerable<IDataReader> dataReaders)
        {
            if (dataReaders == null) return;
            foreach (var reader in dataReaders)
            {
                foreach (var contentType in reader.ContentType)
                {
                    if (string.IsNullOrEmpty(contentType) ||
                        _readersByMime.ContainsKey(contentType))
                    {
                        continue;
                    }

                    _readersByMime[contentType] = reader;
                }
            }
        }

        public IDataReader Find(string contentTypeHeader)
        {
            var type = DataProviderUtility.ParseMediaType(contentTypeHeader);

            var readers = _readersByMime.Where(reader => Regex.Match(type, reader.Key, RegexOptions.Singleline).Success);

            var keyValuePairs = readers as IList<KeyValuePair<string, IDataReader>> ?? readers.ToList();
            return keyValuePairs.Any() ? keyValuePairs.First().Value : null;
        }
    }
}