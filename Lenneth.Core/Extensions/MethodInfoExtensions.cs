using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Lenneth.Core.Extensions
{
    [DebuggerStepThrough]
    public static class MethodInfoExtensions
    {
        /// <summary>
        /// With virtual keyword. Also interface implementations even without virtual keyword.
        /// </summary>
        /// <param name="aMi"></param>
        /// <returns></returns>
        public static bool IsVirtual(this MethodInfo aMi)
        {
            return aMi.IsVirtual && !aMi.IsAbstract && !aMi.IsOverriden();
        }

        /// <summary>
        /// With override keyword.
        /// </summary>
        /// <param name="aMi"></param>
        /// <returns></returns>
        public static bool IsOverriden(this MethodInfo aMi)
        {
            return aMi.DeclaringType != aMi.GetBaseDefinition().DeclaringType;
        }

        public static IEnumerable<MethodInfo> GetBaseDefinitions(this MethodInfo aMi,
            bool aWithThis = false)
        {
            if (aWithThis)
                yield return aMi;

            MethodInfo t = aMi;

            while ((t.GetBaseDefinition() != null) && (t.GetBaseDefinition() != t))
            {
                t = t.GetBaseDefinition();
                yield return t;
            }
        }

        public static bool IsDerivedFrom(this MethodInfo aMi, MethodInfo aBase,
            bool aWithThis = false)
        {
            if (aMi.Name != aBase.Name)
                return false;
            if (aMi.DeclaringType == aBase.DeclaringType)
            {
                if (!aMi.GetParameters().Select(p => p.ParameterType).SequenceEqual(
                    aBase.GetParameters().Select(p => p.ParameterType)))
                {
                    return false;
                }

                return aWithThis;
            }

            return aMi.GetBaseDefinitions().Contains(aBase);
        }
    }
}