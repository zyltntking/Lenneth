using JsonFx.Json.Resolvers;
using JsonFx.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lenneth.Core.Framework.Http.Codecs.JsonFXExtensions
{
    internal sealed class RemoveAmpersandFromNameJsonResolverStrategy : JsonResolverStrategy
    {
        public override IEnumerable<DataName> GetName(MemberInfo member)
        {
            if (!member.Name.StartsWith("@", StringComparison.InvariantCulture)) return base.GetName(member);

            var nameWithoutAmpersand = member.Name.Remove(0);

            return new List<DataName> { new DataName(nameWithoutAmpersand) };
        }
    }
}