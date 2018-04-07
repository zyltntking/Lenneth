using System;
using System.Reflection;
using Lenneth.Core.Framework.ObjectMapper.Core.DataStructures;
using Lenneth.Core.Framework.ObjectMapper.Mappers.Classes.Members;
using Lenneth.Core.Framework.ObjectMapper.Reflection;

namespace Lenneth.Core.Framework.ObjectMapper.Mappers
{
    internal abstract class MapperBuilder
    {
        protected const MethodAttributes OverrideProtected = MethodAttributes.Family | MethodAttributes.Virtual;
        private const string AssemblyName = "DynamicTinyMapper";
        protected readonly IDynamicAssembly Assembly;
        protected readonly IMapperBuilderConfig Config;

        protected MapperBuilder(IMapperBuilderConfig config)
        {
            Config = config;
            Assembly = config.Assembly;
        }

        protected abstract string ScopeName { get; }

        public Mapper Build(TypePair typePair)
        {
            return BuildCore(typePair);
        }

        public Mapper Build(TypePair parentTypePair, MappingMember mappingMember)
        {
            return BuildCore(parentTypePair, mappingMember);
        }

        public bool IsSupported(TypePair typePair)
        {
            return IsSupportedCore(typePair);
        }

        protected abstract Mapper BuildCore(TypePair typePair);
        protected abstract Mapper BuildCore(TypePair parentTypePair, MappingMember mappingMember);

        protected MapperBuilder GetMapperBuilder(TypePair typePair)
        {
            return Config.GetMapperBuilder(typePair);
        }

        protected string GetMapperFullName()
        {
            var random = Guid.NewGuid().ToString("N");
            return $"{AssemblyName}.{ScopeName}.Mapper{random}";
        }

        protected abstract bool IsSupportedCore(TypePair typePair);
    }
}
