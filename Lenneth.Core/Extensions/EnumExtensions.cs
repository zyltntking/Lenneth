using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Lenneth.Core.Extensions
{
    [DebuggerStepThrough]
    public static class EnumExtensions
    {
        public static T Parse<T>(string aStr)
        {
            return (T)Enum.Parse(typeof(T), aStr);
        }

        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}