using Lenneth.Core.Framework.ObjectMapper.CodeGenerators.Emitters;
using Lenneth.Core.Framework.ObjectMapper.Core.DataStructures;
using Lenneth.Core.Framework.ObjectMapper.Core.Extensions;
using Lenneth.Core.Framework.ObjectMapper.Mappers.Caches;

namespace Lenneth.Core.Framework.ObjectMapper.Mappers.Classes.Members
{
    internal sealed class MemberEmitterDescription
    {
        public MemberEmitterDescription(IEmitter emitter, MapperCache mappers)
        {
            Emitter = emitter;
            MapperCache = new Option<MapperCache>(mappers, mappers.IsEmpty);
        }

        public IEmitter Emitter { get; }
        public Option<MapperCache> MapperCache { get; private set; }

        public void AddMapper(MapperCache value)
        {
            MapperCache = value.ToOption();
        }
    }
}
