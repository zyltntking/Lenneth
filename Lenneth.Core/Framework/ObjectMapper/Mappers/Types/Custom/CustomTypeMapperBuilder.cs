using Lenneth.Core.Framework.ObjectMapper.Core.DataStructures;
using Lenneth.Core.Framework.ObjectMapper.Mappers.Classes.Members;
using System;

namespace Lenneth.Core.Framework.ObjectMapper.Mappers.Types.Custom
{
    internal sealed class CustomTypeMapperBuilder : MapperBuilder
    {
        public CustomTypeMapperBuilder(IMapperBuilderConfig config) : base(config)
        {
        }

        protected override string ScopeName => "CustomTypeMapper";

        public bool IsSupported(TypePair parentTypePair, MappingMember mappingMember)
        {
            var bindingConfig = Config.GetBindingConfig(parentTypePair);
            if (bindingConfig.HasNoValue)
            {
                return false;
            }
            return bindingConfig.Value.HasCustomTypeConverter(mappingMember.Target.Name);
        }

        protected override Mapper BuildCore(TypePair typePair)
        {
            throw new NotSupportedException();
        }

        protected override Mapper BuildCore(TypePair parentTypePair, MappingMember mappingMember)
        {
            var bindingConfig = Config.GetBindingConfig(parentTypePair);
            var converter = bindingConfig.Value.GetCustomTypeConverter(mappingMember.Target.Name).Value;
            return new CustomTypeMapper(converter);
        }

        protected override bool IsSupportedCore(TypePair typePair)
        {
            throw new NotSupportedException();
        }
    }
}