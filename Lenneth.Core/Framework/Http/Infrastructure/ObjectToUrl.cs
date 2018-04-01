using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;

namespace Lenneth.Core.Framework.Http.Infrastructure
{
    internal abstract class ObjectToUrl
    {
        protected abstract string PathStartCharacter { get; }
        protected abstract string PathSeparatorCharacter { get; }

        protected abstract string BuildParam(PropertyValue propertyValue);

        public string ParametersToUrl(object parameters)
        {
            var returnuri = "";
            var properties = GetProperties(parameters);
            if (parameters == null) return returnuri;
            var paramsList = properties.Select(BuildParam).ToList();
            if (paramsList.Count > 0)
            {
                returnuri = $"{PathStartCharacter}{string.Join(PathSeparatorCharacter, paramsList)}";
            }
            return returnuri;
        }

        private static IEnumerable<PropertyValue> GetProperties(object parameters)
        {
            if (parameters == null) yield break;
            if (parameters is ExpandoObject o)
            {
                var dictionary = (IDictionary<string, object>)o;
                foreach (var property in dictionary)
                {
                    yield return new PropertyValue { Name = property.Key, Value = property.Value.ToString() };
                }
            }
            else
            {
                var properties = TypeDescriptor.GetProperties(parameters);
                foreach (PropertyDescriptor propertyDescriptor in properties)
                {
                    var val = propertyDescriptor.GetValue(parameters);
                    if (val != null)
                    {
                        yield return new PropertyValue { Name = propertyDescriptor.Name, Value = val.ToString() };
                    }
                }
            }
        }

        protected class PropertyValue
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }
}