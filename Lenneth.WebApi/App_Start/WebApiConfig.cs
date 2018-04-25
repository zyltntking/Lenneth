using Microsoft.Web.Http.Routing;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Routing;

namespace Lenneth.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Web API 版本化管理
            var constraintResolver = new DefaultInlineConstraintResolver
            {
                ConstraintMap = { ["apiVersion"] = typeof(ApiVersionRouteConstraint) }
            };
            config.MapHttpAttributeRoutes(constraintResolver);

            config.AddApiVersioning(opinion => opinion.ReportApiVersions = true);

            var apiExplorer = config.AddVersionedApiExplorer(
                opinion =>
                {
                    opinion.GroupNameFormat = "'v'VVV";
                    opinion.SubstituteApiVersionInUrl = true;
                });

            var thisAssembly = typeof(WebApiConfig).Assembly;
            // Web API 配置和服务
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API 文档配置
            config.EnableSwagger("apiInfo/{apiVersion}", c =>
            {
                //c.RootUrl(req => GetRootUrlFromAppConfig());
                c.Schemes(new[] { "http", "https" });
                //c.SingleApiVersion("v1", "Swashbuckle Dummy");
                c.PrettyPrint();
                c.MultipleApiVersions(
                    (apiDescription, version) => apiDescription.GetGroupName() == version,
                    info =>
                    {
                        foreach (var group in apiExplorer.ApiDescriptions)
                        {
                            var description = $@"WebApi 测试工具, Api版本:{group.ApiVersion}";
                            if (group.IsDeprecated)
                            {
                                description += $@" <b> 该版本已停止维护</b>";
                            }

                            info.Version(group.Name, $"WebClient API {group.ApiVersion}")
                                .Description(description)
                                .TermsOfService("TravelSky CO.");
                        }
                    });
                //c.BasicAuth("basic")
                //    .Description("Basic HTTP Authentication");
                //
                // NOTE: You must also configure 'EnableApiKeySupport' below in the SwaggerUI section
                //c.ApiKey("apiKey")
                //    .Description("API Key Authentication")
                //    .Name("apiKey")
                //    .In("header");
                //
                //c.OAuth2("oauth2")
                //    .Description("OAuth2 Implicit Grant")
                //    .Flow("implicit")
                //    .AuthorizationUrl("http://petstore.swagger.wordnik.com/api/oauth/dialog")
                //    //.TokenUrl("https://tempuri.org/token")
                //    .Scopes(scopes =>
                //    {
                //        scopes.Add("read", "Read access to protected resources");
                //        scopes.Add("write", "Write access to protected resources");
                //    });
                c.IgnoreObsoleteActions();
                //c.GroupActionsBy(apiDesc => apiDesc.HttpMethod.ToString());
                //c.OrderActionGroupsBy(new DescendingAlphabeticComparer());
                //c.IncludeXmlComments(XmlCommentsFilePath);
                //c.MapType<ProductType>(() => new Schema { type = "integer", format = "int32" });
                //c.SchemaFilter<ApplySchemaVendorExtensions>();
                //c.UseFullTypeNameInSchemaIds();
                //c.SchemaId(t => t.FullName.Contains('`') ? t.FullName.Substring(0, t.FullName.IndexOf('`')) : t.FullName);
                //c.IgnoreObsoleteProperties();
                //c.DescribeAllEnumsAsStrings();
                //c.OperationFilter<AddDefaultResponse>();
                //c.OperationFilter<SwaggerDefaultValues>();
                //c.OperationFilter<AssignOAuth2SecurityRequirements>();
                c.OperationFilter<SwaggerDefaultValues>();
                //c.DocumentFilter<ApplyDocumentVendorExtensions>();
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                //c.CustomProvider((defaultProvider) => new CachingSwaggerProvider(defaultProvider));
            }).EnableSwaggerUi("Document/{*assetPath}", c =>
            {
                c.DocumentTitle("WebClient Api Document");
                //c.InjectStylesheet(containingAssembly, "Swashbuckle.Dummy.SwaggerExtensions.testStyles1.css");
                //c.InjectJavaScript(thisAssembly, "WebClient.App_Data.swagger-zh-cn.js");
                //c.BooleanValues(new[] { "0", "1" });
                //c.DisableValidator();
                //c.DocExpansion(DocExpansion.List);
                c.SupportedSubmitMethods("GET", "POST");
                //c.CustomAsset("index", containingAssembly, "YourWebApiProject.SwaggerExtensions.index.html");
                c.EnableDiscoveryUrlSelector();
                //c.EnableOAuth2Support(
                //    clientId: "test-client-id",
                //    clientSecret: null,
                //    realm: "test-realm",
                //    appName: "Swagger UI"
                //    //additionalQueryStringParams: new Dictionary<string, string>() { { "foo", "bar" } }
                //);
                //c.EnableApiKeySupport("apiKey", "header");
            });

            // 默认 Web API 路由
            //config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }

        /// <summary>
        /// XML Doc 路径
        /// </summary>
        private static string XmlCommentsFilePath
        {
            get
            {
                var basePath = System.AppDomain.CurrentDomain.RelativeSearchPath;
                const string filename = "WebApi.xml";

                return Path.Combine(basePath, filename);
            }
        }
    }

    /// <summary>
    /// 默认操作过滤器
    /// </summary>
    internal class SwaggerDefaultValues : IOperationFilter
    {
        /// <summary>
        /// Applies the filter to the specified operation using the given context.
        /// </summary>
        /// <param name="operation">The operation to apply the filter to.</param>
        /// <param name="schemaRegistry">The API schema registry.</param>
        /// <param name="apiDescription">The API description being filtered.</param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
            {
                return;
            }

            foreach (var parameter in operation.parameters)
            {
                var description = apiDescription.ParameterDescriptions
                    .First(p => p.Name == parameter.name);

                if (parameter.description == null)
                {
                    parameter.description = description.Documentation;
                }

                if (parameter.@default == null)
                {
                    parameter.@default = description.ParameterDescriptor?.DefaultValue;
                }
            }
        }
    }
}