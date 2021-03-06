using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Lenneth.Core.Framework.ObjectMapper.CodeGenerators;
using Lenneth.Core.Framework.ObjectMapper.CodeGenerators.Emitters;
using Lenneth.Core.Framework.ObjectMapper.Core;
using Lenneth.Core.Framework.ObjectMapper.Core.DataStructures;
using Lenneth.Core.Framework.ObjectMapper.Core.Extensions;
using Lenneth.Core.Framework.ObjectMapper.Mappers.Caches;
using Lenneth.Core.Framework.ObjectMapper.Mappers.Classes.Members;

namespace Lenneth.Core.Framework.ObjectMapper.Mappers.Classes
{
    internal sealed class ClassMapperBuilder : MapperBuilder
    {
        private readonly MapperCache _mapperCache;
        private const string CreateTargetInstanceMethod = "CreateTargetInstance";
        private const string MapClassMethod = "MapClass";
        private readonly MappingMemberBuilder _mappingMemberBuilder;
        private readonly MemberMapper _memberMapper;

        public ClassMapperBuilder(MapperCache mapperCache, IMapperBuilderConfig config) : base(config)
        {
            _mapperCache = mapperCache;
            _memberMapper = new MemberMapper(mapperCache, config);
            _mappingMemberBuilder = new MappingMemberBuilder(config);
        }

        protected override string ScopeName => "ClassMappers";

        protected override Mapper BuildCore(TypePair typePair)
        {
            var parentType = typeof(ClassMapper<,>).MakeGenericType(typePair.Source, typePair.Target);
            var typeBuilder = Assembly.DefineType(GetMapperFullName(), parentType);
            EmitCreateTargetInstance(typePair.Target, typeBuilder);

            var rootMapperCacheItem = _mapperCache.AddStub(typePair);
            var mappers = EmitMapClass(typePair, typeBuilder);

            var rootMapper = (Mapper)Activator.CreateInstance(Helpers.CreateType(typeBuilder));

            UpdateMappers(mappers, rootMapperCacheItem.Id, rootMapper);

            _mapperCache.ReplaceStub(typePair, rootMapper);

            mappers.Do(x => rootMapper.AddMappers(x.Mappers));

            return rootMapper;
        }

        private static void UpdateMappers(Option<MapperCache> mappers, int rootMapperId, Mapper rootMapper)
        {
            if (mappers.HasValue)
            {
                var result = mappers.Value.MapperCacheItems.Select(item => item.Id != rootMapperId ? item.Mapper : null).ToList();
                result[rootMapperId] = rootMapper;
                rootMapper.AddMappers(result);
                foreach (var item in mappers.Value.MapperCacheItems)
                {
                    if (item.Id == rootMapperId)
                    {
                        continue;
                    }
                    item.Mapper?.UpdateRootMapper(rootMapperId, rootMapper);
                }
            }
        }

        protected override Mapper BuildCore(TypePair parentTypePair, MappingMember mappingMember)
        {
            return BuildCore(mappingMember.TypePair);
        }

        protected override bool IsSupportedCore(TypePair typePair)
        {
            return true;
        }

        private static void EmitCreateTargetInstance(Type targetType, TypeBuilder typeBuilder)
        {
            var methodBuilder = typeBuilder.DefineMethod(CreateTargetInstanceMethod, OverrideProtected, targetType, Type.EmptyTypes);
            var codeGenerator = new CodeGenerator(methodBuilder.GetILGenerator());

            var result = Helpers.IsValueType(targetType) ? EmitValueType(targetType, codeGenerator) : EmitRefType(targetType);

            EmitReturn.Return(result, targetType).Emit(codeGenerator);
        }

        private static IEmitterType EmitRefType(Type type)
        {
            return type.HasDefaultCtor() ? EmitNewObj.NewObj(type) : EmitNull.Load();
        }

        private static IEmitterType EmitValueType(Type type, CodeGenerator codeGenerator)
        {
            var builder = codeGenerator.DeclareLocal(type);
            EmitLocalVariable.Declare(builder).Emit(codeGenerator);
            return EmitBox.Box(EmitLocal.Load(builder));
        }

        private Option<MapperCache> EmitMapClass(TypePair typePair, TypeBuilder typeBuilder)
        {
            var methodBuilder = typeBuilder.DefineMethod(MapClassMethod,
                OverrideProtected,
                typePair.Target,
                new[] { typePair.Source, typePair.Target });
            var codeGenerator = new CodeGenerator(methodBuilder.GetILGenerator());

            var emitterComposite = new EmitComposite();

            var emitterDescription = EmitMappingMembers(typePair);

            emitterComposite.Add(emitterDescription.Emitter);
            emitterComposite.Add(EmitReturn.Return(EmitArgument.Load(typePair.Target, 2)));
            emitterComposite.Emit(codeGenerator);
            return emitterDescription.MapperCache;
        }

        private MemberEmitterDescription EmitMappingMembers(TypePair typePair)
        {
            var members = _mappingMemberBuilder.Build(typePair);
            var result = _memberMapper.Build(typePair, members);
            return result;
        }
    }
}
