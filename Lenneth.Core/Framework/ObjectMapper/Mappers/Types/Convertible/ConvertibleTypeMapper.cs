using System;

namespace Lenneth.Core.Framework.ObjectMapper.Mappers.Types.Convertible
{
    internal sealed class ConvertibleTypeMapper : Mapper
    {
        private readonly Func<object, object> _converter;

        public ConvertibleTypeMapper(Func<object, object> converter)
        {
            _converter = converter;
        }

        protected override object MapCore(object source, object target)
        {
            if (_converter == null)
            {
                return source;
            }
            return source == null ? target : _converter(source);
        }
    }
}
