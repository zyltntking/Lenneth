﻿using System.Collections.Generic;
using System.Linq;
using Lenneth.Core.Framework.ObjectMapper.Core.DataStructures;
using Lenneth.Core.Framework.ObjectMapper.Core.Extensions;

namespace Lenneth.Core.Framework.ObjectMapper.Mappers.Caches
{
    internal sealed class MapperCache
    {
        private readonly Dictionary<TypePair, MapperCacheItem> _cache = new Dictionary<TypePair, MapperCacheItem>();

        public bool IsEmpty => _cache.Count == 0;

        public IEnumerable<Mapper> Mappers
        {
            get
            {
                return _cache.Values
                             .OrderBy(x => x.Id)
                             .ConvertAll(x => x.Mapper);
            }
        }

        public IEnumerable<MapperCacheItem> MapperCacheItems => _cache.Values.ToList();

        public MapperCacheItem AddStub(TypePair key)
        {
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }

            var mapperCacheItem = new MapperCacheItem { Id = GetId() };
            _cache[key] = mapperCacheItem;
            return mapperCacheItem;
        }

        public void ReplaceStub(TypePair key, Mapper mapper)
        {
            _cache[key].Mapper = mapper;
        }

        public MapperCacheItem Add(TypePair key, Mapper mapper)
        {
            if (_cache.TryGetValue(key, out var result))
            {
                return result;
            }
            result = new MapperCacheItem
            {
                Id = GetId(),
                Mapper = mapper
            };
            _cache[key] = result;
            return result;
        }

        public Option<MapperCacheItem> Get(TypePair key)
        {
            return _cache.TryGetValue(key, out var result) ? new Option<MapperCacheItem>(result) : Option<MapperCacheItem>.Empty;
        }

        private int GetId()
        {
            return _cache.Count;
        }
    }
}
