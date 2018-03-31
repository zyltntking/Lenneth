using Lenneth.Core.Sample;
using Unity;

namespace Lenneth.Core
{
    using Framework.Log.Interface;

    public static class Facade
    {
        public static string Test
        {
            get
            {
                var sample = UnityConfig.Container.Resolve<ISample>();
                sample.SampleMethod();
                return $"Facade Test";
            }
        }

        public static ILogging Logger => UnityConfig.Container.Resolve<ILogging>();
    }
}