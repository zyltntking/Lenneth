using System.Diagnostics;

namespace Lenneth.Core.Extensions.Utils
{
    [DebuggerDisplay("Value: {Value}")]
    public class ValueClass<T> where T : struct
    {
        public T Value;

        public ValueClass(T aValue)
        {
            Value = aValue;
        }

        public static implicit operator T(ValueClass<T> aBc)
        {
            return aBc.Value;
        }

        public static implicit operator ValueClass<T>(T aValue)
        {
            return new ValueClass<T>(aValue);
        }
    }
}