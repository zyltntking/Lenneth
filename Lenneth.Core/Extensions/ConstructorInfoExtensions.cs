using System.Diagnostics;
using System.Reflection;

namespace Lenneth.Core.Extensions
{
    [DebuggerStepThrough]
    public static class ConstructorInfoExtensions
    {
        public static object Invoke(this ConstructorInfo aCi)
        {
            return aCi.Invoke(null);
        }

        public static object Invoke(this ConstructorInfo aCi, params object[] aParams)
        {
            return aCi.Invoke(aParams);
        }
    }
}