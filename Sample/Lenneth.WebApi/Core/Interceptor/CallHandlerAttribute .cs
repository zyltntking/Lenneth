using Unity;
using Unity.Interception.PolicyInjection.Pipeline;
using Unity.Interception.PolicyInjection.Policies;

namespace Lenneth.WebApi.Core.Interceptor
{
    internal class CallHandlerAttribute : HandlerAttribute
    {
        #region Overrides of HandlerAttribute

        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new CallHandlerInterecptor();
        }

        #endregion
    }
}