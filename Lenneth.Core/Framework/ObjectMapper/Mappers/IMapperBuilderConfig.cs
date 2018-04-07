using System;
using Lenneth.Core.Framework.ObjectMapper.Bindings;
using Lenneth.Core.Framework.ObjectMapper.Core.DataStructures;
using Lenneth.Core.Framework.ObjectMapper.Mappers.Classes.Members;
using Lenneth.Core.Framework.ObjectMapper.Reflection;

namespace Lenneth.Core.Framework.ObjectMapper.Mappers
{
    internal interface IMapperBuilderConfig
    {
        IDynamicAssembly Assembly { get; }
        Func<string, string, bool> NameMatching { get; }
        Option<BindingConfig> GetBindingConfig(TypePair typePair);
        MapperBuilder GetMapperBuilder(TypePair typePair);
        MapperBuilder GetMapperBuilder(TypePair parentTypePair, MappingMember mappingMember);
    }
}
