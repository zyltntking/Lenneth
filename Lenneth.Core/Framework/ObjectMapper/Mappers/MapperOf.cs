﻿namespace Lenneth.Core.Framework.ObjectMapper.Mappers
{
    internal abstract class MapperOf<TSource, TTarget> : Mapper
    {
        protected override object MapCore(object source, object target)
        {
            return source == null ? default(TTarget) : MapCore((TSource)source, (TTarget)target);
        }

        protected abstract TTarget MapCore(TSource source, TTarget target);
    }
}
