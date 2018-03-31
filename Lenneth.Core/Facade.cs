using Unity;

namespace Lenneth.Core
{
    using Framework.Log.Interface;

    public static class Facade
    {
        public static string Test => "Facade Test";

        public static ILogging Logger => UnityConfig.Container.Resolve<ILogging>();
    }
}