using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Lenneth.Core.Extensions
{
    [DebuggerStepThrough]
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// With virtual keyword. Also interface implementations even without virtual keyword.
        /// </summary>
        /// <param name="aPi"></param>
        /// <returns></returns>
        public static bool IsVirtual(this PropertyInfo aPi)
        {
            return aPi.GetAccessors(true).Length != 0 && aPi.GetAccessors(true)[0].IsVirtual();
        }

        /// <summary>
        /// With abstract keyword.
        /// </summary>
        /// <param name="aPi"></param>
        /// <returns></returns>
        public static bool IsAbstract(this PropertyInfo aPi)
        {
            return aPi.GetAccessors(true).Length != 0 && aPi.GetAccessors(true)[0].IsAbstract;
        }

        /// <summary>
        /// With override keyword.
        /// </summary>
        /// <param name="aPi"></param>
        /// <returns></returns>
        public static bool IsOverriden(this PropertyInfo aPi)
        {
            return aPi.GetAccessors(true).Length != 0 && aPi.GetAccessors(true)[0].IsOverriden();
        }

        public static bool IsDerivedFrom(this PropertyInfo aPi, PropertyInfo aBase,
            bool aWithThis = false)
        {
            if (aPi.Name != aBase.Name)
                return false;
            if (aPi.PropertyType != aBase.PropertyType)
                return false;
            if (!aPi.GetIndexParameters().Select(p => p.ParameterType).SequenceEqual(
                aBase.GetIndexParameters().Select(p => p.ParameterType)))
            {
                return false;
            }
            if (aPi.DeclaringType == aBase.DeclaringType)
                return aWithThis;

            var m1 = aPi.GetGetMethod(true);
            var m3 = aBase.GetGetMethod(true);

            if ((m1 != null) && (m3 != null))
            {
                if (m1.GetBaseDefinitions().ContainsAny(m3.GetBaseDefinitions(true)))
                    return true;
            }
            else if ((m1 != null) || (m3 != null))
                return false;

            var m2 = aPi.GetSetMethod(true);
            var m4 = aBase.GetSetMethod(true);

            if ((m2 != null) && (m4 != null))
            {
                if (m2.GetBaseDefinitions().ContainsAny(m4.GetBaseDefinitions(true)))
                    return true;
            }
            else if ((m2 != null) || (m4 != null))
                return false;

            return false;
        }
    }
}