using Lenneth.Core.Framework.Crypt;
using Unity;

namespace Lenneth.Core
{
    using Framework.Log;
    using Framework.MD;
    using Framework.Mail;

    public static class Facade
    {
        public static string Test => $"Facade Test";

        /// <summary>
        /// 日志门面
        /// </summary>
        public static ILogging Logger => UnityConfig.Container.Resolve<ILogging>();

        /// <summary>
        /// MD门面
        /// </summary>
        public static IMarkdown Markdown => UnityConfig.Container.Resolve<IMarkdown>();

        /// <summary>
        /// Mail门面
        /// </summary>
        public static IMail Mail => UnityConfig.Container.Resolve<IMail>();

        /// <summary>
        /// Crypt门面
        /// </summary>
        public static ICrypt Crypt => UnityConfig.Container.Resolve<ICrypt>();
    }
}