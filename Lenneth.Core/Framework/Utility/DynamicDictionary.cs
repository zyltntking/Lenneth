using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Lenneth.Core.Framework.Utility
{
    public sealed class DynamicDictionary : DynamicObject
    {
        /// <summary>
        /// 动态类型字典
        /// </summary>
        private Dictionary<string, object> Dictionary { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dictionary"></param>
        public DynamicDictionary(Dictionary<string, object> dictionary)
        {
            Dictionary = dictionary;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public DynamicDictionary()
        {
            Dictionary = new Dictionary<string, object>();
        }

        /// <summary>提供用于获取成员值的操作的实现。 类派生自 <see cref="T:System.Dynamic.DynamicObject" /> 类可以重写此方法以指定动态行为的操作，如获取的属性的值。</summary>
        /// <param name="binder">提供有关调用动态操作的对象信息。binder.Name 属性提供对其执行动态操作的成员的名称。 例如，对于 Console.WriteLine(sampleObject.SampleProperty) 语句，其中 sampleObject 是派生自的类的实例 <see cref="T:System.Dynamic.DynamicObject" /> 类， binder.Name 返回"SampleProperty"。binder.IgnoreCase 属性指定的成员名称是否区分大小写。</param>
        /// <param name="result">获取操作的结果。 例如，如果为属性调用方法，您可以将属性值赋给 <paramref name="result" />。</param>
        /// <returns>如果操作成功，则为 true；否则为 false。 如果此方法返回 false, ，语言运行时联编程序确定的行为。 （在大多数情况下，运行时异常引发。）</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!Dictionary.ContainsKey(binder.Name)) return base.TryGetMember(binder, out result);
            result = Dictionary[binder.Name];
            return true;
        }

        /// <summary>提供设置成员值的操作的实现。 类派生自 <see cref="T:System.Dynamic.DynamicObject" /> 类可以重写此方法以指定动态行为的操作，如设置属性的值。</summary>
        /// <param name="binder">提供有关调用动态操作的对象信息。binder.Name 属性提供向其分配值的成员的名称。 例如，对于该语句 sampleObject.SampleProperty = "Test", ，其中 sampleObject 是派生自的类的实例 <see cref="T:System.Dynamic.DynamicObject" /> 类， binder.Name 返回"SampleProperty"。binder.IgnoreCase 属性指定的成员名称是否区分大小写。</param>
        /// <param name="value">要设置为成员的值。 例如，对于 sampleObject.SampleProperty = "Test", ，其中 sampleObject 是派生自的类的实例 <see cref="T:System.Dynamic.DynamicObject" /> 类， <paramref name="value" /> 是"测试"。</param>
        /// <returns>如果操作成功，则为 true；否则为 false。 如果此方法返回 false, ，语言运行时联编程序确定的行为。 （在大多数情况下，特定于语言的运行时异常引发。）</returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!Dictionary.ContainsKey(binder.Name))
            {
                Dictionary.Add(binder.Name, value);
            }
            else
            {
                Dictionary[binder.Name] = value;
            }
                
            return true;
        }

        /// <summary>提供按索引设置一个值的操作的实现。 类派生自 <see cref="T:System.Dynamic.DynamicObject" /> 类可以重写此方法以指定动态行为的操作，按指定的索引访问的对象。</summary>
        /// <param name="binder">提供有关操作的信息。</param>
        /// <param name="indexes">此操作中使用的索引。 例如，对于 sampleObject[3] = 10 C# 中的操作 (sampleObject(3) = 10 在 Visual Basic 中)，其中 sampleObject 派生自 <see cref="T:System.Dynamic.DynamicObject" /> 类， <paramref name="indexes[0]" /> 等于 3。</param>
        /// <param name="value">要设置为具有指定的索引的对象的值。 例如，对于 sampleObject[3] = 10 C# 中的操作 (sampleObject(3) = 10 在 Visual Basic 中)，其中 sampleObject 派生自 <see cref="T:System.Dynamic.DynamicObject" /> 类， <paramref name="value" /> 等于 10。</param>
        /// <returns>如果操作成功，则为 true；否则为 false。 如果此方法返回 false, ，语言运行时联编程序确定的行为。 （在大多数情况下，特定于语言的运行时异常引发。</returns>
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (!(indexes[0] is string key)) return base.TrySetIndex(binder, indexes, value);
            if (Dictionary.ContainsKey(key))
            {
                Dictionary[key] = value;
            }
            else
            {
                Dictionary.Add(key, value);
            }
            return true;
        }

        /// <summary>提供按索引获取一个值的操作的实现。 类派生自 <see cref="T:System.Dynamic.DynamicObject" /> 类可以重写此方法以指定为索引操作的动态行为。</summary>
        /// <param name="binder">提供有关操作的信息。</param>
        /// <param name="indexes">此操作中使用的索引。 例如，对于 sampleObject[3] C# 中的操作 (sampleObject(3) 在 Visual Basic 中)，其中 sampleObject 派生自 DynamicObject 类， <paramref name="indexes[0]" /> 等于 3。</param>
        /// <param name="result">索引操作的结果。</param>
        /// <returns>如果操作成功，则为 true；否则为 false。 如果此方法返回 false, ，语言运行时联编程序确定的行为。 （在大多数情况下，运行时异常引发。）</returns>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (!(indexes[0] is string)) return base.TryGetIndex(binder, indexes, out result);
            var index = (string)indexes[0];
            return Dictionary.TryGetValue(index, out result);
        }

        /// <summary>提供用于调用成员的操作的实现。 类派生自 <see cref="T:System.Dynamic.DynamicObject" /> 类可以重写此方法以指定动态行为的操作，如调用方法。</summary>
        /// <param name="binder">提供有关动态操作的信息。binder.Name 属性提供对其执行动态操作的成员的名称。 例如，对于该语句 sampleObject.SampleMethod(100), ，其中 sampleObject 是派生自的类的实例 <see cref="T:System.Dynamic.DynamicObject" /> 类， binder.Name 返回"SampleMethod"。binder.IgnoreCase 属性指定的成员名称是否区分大小写。</param>
        /// <param name="args">调用操作期间传递给对象成员的参数。 例如，对于该语句 sampleObject.SampleMethod(100), ，其中 sampleObject 派生自 <see cref="T:System.Dynamic.DynamicObject" /> 类， <paramref name="args[0]" /> 等于 100。</param>
        /// <param name="result">该成员的调用的结果。</param>
        /// <returns>如果操作成功，则为 true；否则为 false。 如果此方法返回 false, ，语言运行时联编程序确定的行为。 （在大多数情况下，特定于语言的运行时异常引发。）</returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            Type dictType = typeof(Dictionary<string, object>);
            try
            {
                result = dictType.InvokeMember(
                    binder.Name,
                    BindingFlags.InvokeMethod,
                    null, Dictionary, args);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }

        }

        /// <summary>返回的所有动态成员名称的枚举。</summary>
        /// <returns>一个包含动态成员名称的序列。</returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return Dictionary.Keys;
        }
    }
}