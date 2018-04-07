using Lenneth.Core.Framework.ObjectMapper.CodeGenerators;
using Lenneth.Core.Framework.ObjectMapper.CodeGenerators.Emitters;
using Lenneth.Core.Framework.ObjectMapper.Core;
using Lenneth.Core.Framework.ObjectMapper.Core.DataStructures;
using Lenneth.Core.Framework.ObjectMapper.Core.Extensions;
using Lenneth.Core.Framework.ObjectMapper.Mappers.Caches;
using Lenneth.Core.Framework.ObjectMapper.Mappers.Classes.Members;
using System;
using System.Collections;
using System.Reflection.Emit;

namespace Lenneth.Core.Framework.ObjectMapper.Mappers.Collections
{
    internal sealed class CollectionMapperBuilder : MapperBuilder
    {
        private readonly MapperCache _mapperCache;
        private const string ConvertItemKeyMethod = "ConvertItemKey";
        private const string ConvertItemMethod = "ConvertItem";
        private const string DictionaryToDictionaryMethod = "DictionaryToDictionary";
        private const string DictionaryToDictionaryTemplateMethod = "DictionaryToDictionaryTemplate";
        private const string EnumerableToArrayMethod = "EnumerableToArray";
        private const string EnumerableToArrayTemplateMethod = "EnumerableToArrayTemplate";
        private const string EnumerableToListMethod = "EnumerableToList";
        private const string EnumerableToListTemplateMethod = "EnumerableToListTemplate";
        private const string EnumerableOfDeepCloneableToListTemplateMethod = "EnumerableOfDeepCloneableToListTemplate";

        public CollectionMapperBuilder(MapperCache mapperCache, IMapperBuilderConfig config) : base(config)
        {
            _mapperCache = mapperCache;
        }

        protected override string ScopeName => "CollectionMappers";

        protected override Mapper BuildCore(TypePair typePair)
        {
            var parentType = typeof(CollectionMapper<,>).MakeGenericType(typePair.Source, typePair.Target);
            var typeBuilder = Assembly.DefineType(GetMapperFullName(), parentType);

            _mapperCache.AddStub(typePair);

            if (IsIEnumerableToList(typePair))
            {
                EmitEnumerableToList(parentType, typeBuilder, typePair);
            }
            else if (IsIEnumerableToArray(typePair))
            {
                EmitEnumerableToArray(parentType, typeBuilder, typePair);
            }
            else if (IsDictionaryToDictionary(typePair))
            {
                EmitDictionaryToDictionary(parentType, typeBuilder, typePair);
            }
            else if (IsEnumerableToEnumerable(typePair))
            {
                EmitEnumerableToEnumerable(parentType, typeBuilder, typePair);
            }

            var rootMapper = (Mapper)Activator.CreateInstance(Helpers.CreateType(typeBuilder));

            _mapperCache.ReplaceStub(typePair, rootMapper);
            rootMapper.AddMappers(_mapperCache.Mappers);

            return rootMapper;
        }

        protected override Mapper BuildCore(TypePair parentTypePair, MappingMember mappingMember)
        {
            return BuildCore(mappingMember.TypePair);
        }

        protected override bool IsSupportedCore(TypePair typePair)
        {
            return typePair.IsEnumerableTypes;
        }

        private static bool IsDictionaryToDictionary(TypePair typePair)
        {
            return typePair.Source.IsDictionaryOf() && typePair.Target.IsDictionaryOf();
        }

        private static bool IsIEnumerableToArray(TypePair typePair)
        {
            return typePair.Source.IsIEnumerable() && typePair.Target.IsArray;
        }

        private static bool IsIEnumerableToList(TypePair typePair)
        {
            return typePair.Source.IsIEnumerable() && typePair.Target.IsListOf();
        }

        private bool IsEnumerableToEnumerable(TypePair typePair)
        {
            return typePair.Source.IsIEnumerable() && typePair.Target.IsIEnumerable();
        }

        private MapperCacheItem CreateMapperCacheItem(TypePair typePair)
        {
            var mapperCacheItemOption = _mapperCache.Get(typePair);
            if (mapperCacheItemOption.HasValue)
            {
                return mapperCacheItemOption.Value;
            }

            var mapperBuilder = GetMapperBuilder(typePair);
            var mapper = mapperBuilder.Build(typePair);
            var mapperCacheItem = _mapperCache.Add(typePair, mapper);
            return mapperCacheItem;
        }

        private void EmitConvertItem(TypeBuilder typeBuilder, TypePair typePair, string methodName = ConvertItemMethod)
        {
            var mapperCacheItem = CreateMapperCacheItem(typePair);

            var methodBuilder = typeBuilder.DefineMethod(methodName, OverrideProtected, typeof(object), new[] { typeof(object) });

            var sourceMemeber = EmitArgument.Load(typeof(object), 1);
            var targetMember = EmitNull.Load();

            var callMapMethod = mapperCacheItem.EmitMapMethod(sourceMemeber, targetMember);

            EmitReturn.Return(callMapMethod).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
        }

        private void EmitDictionaryToDictionary(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            EmitDictionaryToTarget(parentType, typeBuilder, typePair, DictionaryToDictionaryMethod, DictionaryToDictionaryTemplateMethod);
        }

        private void EmitDictionaryToTarget(
            Type parentType,
            TypeBuilder typeBuilder,
            TypePair typePair,
            string methodName,
            string templateMethodName)
        {
            var methodBuilder = typeBuilder.DefineMethod(methodName, OverrideProtected, typePair.Target, new[] { typeof(IEnumerable) });

            var sourceTypes = typePair.Source.GetDictionaryItemTypes();
            var targetTypes = typePair.Target.GetDictionaryItemTypes();

            EmitConvertItem(typeBuilder, new TypePair(sourceTypes.Key, targetTypes.Key), ConvertItemKeyMethod);
            EmitConvertItem(typeBuilder, new TypePair(sourceTypes.Value, targetTypes.Value));

            var arguments = new[] { sourceTypes.Key, sourceTypes.Value, targetTypes.Key, targetTypes.Value };
            var methodTemplate = parentType.GetGenericMethod(templateMethodName, arguments);

            var returnValue = EmitMethod.Call(methodTemplate, EmitThis.Load(parentType), EmitArgument.Load(typeof(IEnumerable), 1));
            EmitReturn.Return(returnValue).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
        }

        private void EmitEnumerableToArray(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            var collectionItemTypePair = GetCollectionItemTypePair(typePair);

            EmitEnumerableToTarget(parentType, typeBuilder, typePair, collectionItemTypePair, EnumerableToArrayMethod, EnumerableToArrayTemplateMethod);
        }

        private void EmitEnumerableToList(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            var collectionItemTypePair = GetCollectionItemTypePair(typePair);
            var templateMethod = collectionItemTypePair.IsDeepCloneable ? EnumerableOfDeepCloneableToListTemplateMethod : EnumerableToListTemplateMethod;

            EmitEnumerableToTarget(parentType, typeBuilder, typePair, collectionItemTypePair, EnumerableToListMethod, templateMethod);
        }

        private void EmitEnumerableToEnumerable(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            var collectionItemTypePair = GetCollectionItemTypePair(typePair);
            var templateMethod = collectionItemTypePair.IsDeepCloneable ? EnumerableOfDeepCloneableToListTemplateMethod : EnumerableToListTemplateMethod;

            EmitEnumerableToTarget(parentType, typeBuilder, typePair, collectionItemTypePair, EnumerableToListMethod, templateMethod);
        }

        private static TypePair GetCollectionItemTypePair(TypePair typePair)
        {
            var sourceItemType = typePair.Source.GetCollectionItemType();
            var targetItemType = typePair.Target.GetCollectionItemType();

            return new TypePair(sourceItemType, targetItemType);
        }

        private void EmitEnumerableToTarget(
            Type parentType,
            TypeBuilder typeBuilder,
            TypePair typePair,
            TypePair collectionItemTypePair,
            string methodName,
            string templateMethodName)
        {
            var methodBuilder = typeBuilder.DefineMethod(methodName, OverrideProtected, typePair.Target, new[] { typeof(IEnumerable) });

            EmitConvertItem(typeBuilder, collectionItemTypePair);

            var methodTemplate = parentType.GetGenericMethod(templateMethodName, collectionItemTypePair.Target);

            var returnValue = EmitMethod.Call(methodTemplate, EmitThis.Load(parentType), EmitArgument.Load(typeof(IEnumerable), 1));
            EmitReturn.Return(returnValue).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
        }
    }
}