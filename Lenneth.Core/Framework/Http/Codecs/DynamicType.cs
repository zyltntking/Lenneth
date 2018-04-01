using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;

namespace Lenneth.Core.Framework.Http.Codecs
{
    using Infrastructure;

    internal sealed class DynamicType : DynamicObject
    {
        private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var binderName = binder.Name.ToLower(CultureInfo.InvariantCulture);

            if (!_properties.TryGetValue(binderName, out var value))
                throw new PropertyNotFoundException(binder.Name);

            result = value;
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _properties[binder.Name.ToLower(CultureInfo.InvariantCulture)] = value;
            return true;
        }
    }
}