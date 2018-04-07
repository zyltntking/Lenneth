using System.Reflection;
using Lenneth.Core.Framework.ObjectMapper.CodeGenerators.Emitters;

namespace Lenneth.Core.Framework.ObjectMapper.Mappers.Caches
{
    internal sealed class MapperCacheItem
    {
        public int Id { get; set; }
        public Mapper Mapper { get; set; }

        public IEmitterType EmitMapMethod(IEmitterType sourceMemeber, IEmitterType targetMember)
        {
            var mapperType = typeof(Mapper);
            var mapMethod = mapperType.GetMethod(Mapper.MapMethodName, BindingFlags.Instance | BindingFlags.Public);
            var mappersField = mapperType.GetField(Mapper.MappersFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            var mappers = EmitField.Load(EmitThis.Load(mapperType), mappersField);
            var mapper = EmitArray.Load(mappers, Id);
            var result = EmitMethod.Call(mapMethod, mapper, sourceMemeber, targetMember);
            return result;
        }
    }
}
