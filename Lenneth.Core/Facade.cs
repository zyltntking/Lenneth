using Unity;

namespace Lenneth.Core
{
    using Framework.Log;
    using Framework.MD;
    using Framework.Mail;
    using Sample;

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
        public static IMail Mail = UnityConfig.Container.Resolve<IMail>();
    }
}