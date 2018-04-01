using JsonFx.Json;
using JsonFx.Json.Resolvers;
using JsonFx.Model.Filters;
using JsonFx.Serialization;
using JsonFx.Serialization.Resolvers;
using JsonFx.Xml;
using JsonFx.Xml.Resolvers;

using System.Collections.Generic;

namespace Lenneth.Core.Framework.Http.Configuration
{
    using Codecs;
    using Codecs.JsonFXExtensions;

    internal sealed class DefaultEncoderDecoderConfiguration : IEncoderDecoderConfiguration
    {
        public IEncoder GetEncoder()
        {
            var jsonWriter = new JsonWriter(new DataWriterSettings(CombinedResolverStrategy()), "application/.*json", "text/.*json");
            var xmlWriter = new XmlWriter(new DataWriterSettings(CombinedResolverStrategy()), "application/xml", "text/.*xhtml", "text/xml", "text/html");

            var urlEncoderWriter = new UrlEncoderWriter(new DataWriterSettings(CombinedResolverStrategy()), "application/x-www-form-urlencoded");

            var writers = new List<IDataWriter> { jsonWriter, xmlWriter, urlEncoderWriter };

            var dataWriterProvider = new RegExBasedDataWriterProvider(writers);

            return new DefaultEncoder(dataWriterProvider);
        }

        public IDecoder GetDecoder()
        {
            var jsonReader = new JsonReader(new DataReaderSettings(CombinedResolverStrategy(), new Iso8601DateFilter()), "application/.*json", "text/.*json");
            var xmlReader = new XmlReader(new DataReaderSettings(CombinedResolverStrategy(), new Iso8601DateFilter()), "application/.*xml", "text/.*xhtml", "text/xml", "text/html");

            var readers = new List<IDataReader> { jsonReader, xmlReader };

            var dataReaderProvider = new RegExBasedDataReaderProvider(readers);
            return new DefaultDecoder(dataReaderProvider);
        }

        private static CombinedResolverStrategy CombinedResolverStrategy()
        {
            return new CombinedResolverStrategy(
                new JsonResolverStrategy(),
                new DataContractResolverStrategy(),
                new XmlResolverStrategy(),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.PascalCase),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.CamelCase),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.NoChange),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.NoChange, "_"),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.NoChange, "-"),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.Lowercase, "-"),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.Uppercase, "_"));
        }
    }
}