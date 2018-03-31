using System;
using Unity;
using Unity.Injection;

namespace Lenneth.Core
{
    using Framework.Log;
    using Framework.MD;
    using Sample;

    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container

        private static readonly Lazy<IUnityContainer> LazyContainer =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => LazyContainer.Value;

        #endregion Unity Container

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        private static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            //Ìí¼ÓÀ¹½Ø²å¼þ
            // container.AddNewExtension<Interception>();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();

            container.RegisterType<ISample, Sample.Sample>();
            //Nlog
            container.RegisterType<ILogging, NLogWrapper>(new InjectionConstructor(Config.LogConfig, "log"));
            //MarkDown
            container.RegisterType<IMarkdown, Markdown>();
            // common
            // container.RegisterType<ISample, Sample.Sample>( new Interceptor<InterfaceInterceptor>(), new InterceptionBehavior<CommonInterception>());
            // call handler
            // container.RegisterType<ISample, Sample.Sample>().Configure<Interception>().SetInterceptorFor<ISample>(new InterfaceInterceptor());
        }
    }
}