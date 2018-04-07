using System;
using System.Collections.Generic;
using Lenneth.Core.Framework.ObjectMapper.Core.DataStructures;
using Lenneth.Core.Framework.ObjectMapper.Core.Extensions;

namespace Lenneth.Core.Framework.ObjectMapper.Bindings
{
    internal class BindingConfig
    {
        private readonly Dictionary<string, string> _oneToOneBindFields = new Dictionary<string, string>();
        private readonly Dictionary<string, List<BindingFieldPath>> _bindFieldsPath = new Dictionary<string, List<BindingFieldPath>>();
        private readonly Dictionary<string, Type> _bindTypes = new Dictionary<string, Type>();
        private readonly Dictionary<string, Func<object, object>> _customTypeConverters = new Dictionary<string, Func<object, object>>();
        private readonly HashSet<string> _ignoreFields = new HashSet<string>();

        internal void BindConverter(string targetName, Func<object, object> func)
        {
            _customTypeConverters[targetName] = func;
        }

        internal void BindFields(List<string> sourcePath, List<string> targetPath)
        {
            var bindingFieldPath = new BindingFieldPath(sourcePath, targetPath);

            if (!bindingFieldPath.HasPath)
            {
                _oneToOneBindFields[bindingFieldPath.SourceHead] = bindingFieldPath.TargetHead;
            }
            else
            {
                if (_bindFieldsPath.ContainsKey(bindingFieldPath.SourceHead))
                {
                    _bindFieldsPath[bindingFieldPath.SourceHead].Add(bindingFieldPath);
                }
                else
                {
                    _bindFieldsPath[bindingFieldPath.SourceHead] = new List<BindingFieldPath> { bindingFieldPath };
                }
            }
        }

        internal void BindType(string targetName, Type value)
        {
            _bindTypes[targetName] = value;
        }

        internal Option<string> GetBindField(string sourceName)
        {
            var exsist = _oneToOneBindFields.TryGetValue(sourceName, out var result);
            return new Option<string>(result, exsist);
        }

        internal Option<List<BindingFieldPath>> GetBindFieldPath(string fieldName)
        {
            var exsist = _bindFieldsPath.TryGetValue(fieldName, out var result);
            return new Option<List<BindingFieldPath>>(result, exsist);
        }

        internal Option<Type> GetBindType(string targetName)
        {
            var exsist = _bindTypes.TryGetValue(targetName, out var result);
            return new Option<Type>(result, exsist);
        }

        internal Option<Func<object, object>> GetCustomTypeConverter(string targetName)
        {
            return _customTypeConverters.GetValue(targetName);
        }

        internal bool HasCustomTypeConverter(string targetName)
        {
            return _customTypeConverters.ContainsKey(targetName);
        }

        internal void IgnoreSourceField(string sourceName)
        {
            _ignoreFields.Add(sourceName);
        }

        internal bool IsIgnoreSourceField(string sourceName)
        {
            if (string.IsNullOrEmpty(sourceName))
            {
                return true;
            }
            return _ignoreFields.Contains(sourceName);
        }
    }
}
