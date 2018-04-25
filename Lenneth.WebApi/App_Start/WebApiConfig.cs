using System;
using Microsoft.Web.Http.Routing;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Routing;

namespace Lenneth.WebApi
{
    /// <summary>
    /// WebApi配置
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// 配置注册
        /// </summary>
        /// <param name="config">Http配置</param>
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
            config.EnableSwagger("apiInfo/{apiVersion}", swagger =>
            {
                //swagger.RootUrl(req => GetRootUrlFromAppConfig());
                swagger.RootUrl(req =>
                    req.RequestUri.GetLeftPart(UriPartial.Authority) +
                    req.GetRequestContext().VirtualPathRoot.TrimEnd('/'));
                swagger.Schemes(new[] { "http", "https" });
                //swagger.SingleApiVersion("v1", "Swashbuckle Dummy");
                swagger.PrettyPrint();
                swagger.MultipleApiVersions(
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
                                .Contact(
                                    co => co.Name("Zyl")
                                        .Email("zyltntking@live.cn")
                                        .Url("https://github.com/zyltntking/Lenneth")
                                    )
                                .Description(description)
                                .TermsOfService("Lenneth Community");
                        }
                    });
                //swagger.BasicAuth("basic")
                //    .Description("Basic HTTP Authentication");
                //
                // NOTE: You must also configure 'EnableApiKeySupport' below in the SwaggerUI section
                swagger.ApiKey("apiKey")
                    .Description("API Key Authentication")
                    .Name("WebApi-key")
                    .In("header");
                //
                //swagger.OAuth2("oauth2")
                //    .Description("OAuth2 Implicit Grant")
                //    .Flow("implicit")
                //    .AuthorizationUrl("http://petstore.swagger.wordnik.com/api/oauth/dialog")
                //    //.TokenUrl("https://tempuri.org/token")
                //    .Scopes(scopes =>
                //    {
                //        scopes.Add("read", "Read access to protected resources");
                //        scopes.Add("write", "Write access to protected resources");
                //    });
                swagger.IgnoreObsoleteActions();
                //swagger.GroupActionsBy(apiDesc => apiDesc.HttpMethod.ToString());
                //swagger.OrderActionGroupsBy(new DescendingAlphabeticComparer());
                swagger.IncludeXmlComments(XmlCommentsFilePath);
                //swagger.MapType<ProductType>(() => new Schema { type = "integer", format = "int32" });
                //swagger.SchemaFilter<ApplySchemaVendorExtensions>();
                //swagger.UseFullTypeNameInSchemaIds();
                //swagger.SchemaId(t => t.FullName.Contains('`') ? t.FullName.Substring(0, t.FullName.IndexOf('`')) : t.FullName);
                swagger.IgnoreObsoleteProperties();
                //swagger.DescribeAllEnumsAsStrings();
                //swagger.OperationFilter<AddDefaultResponse>();
                //swagger.OperationFilter<SwaggerDefaultValues>();
                //swagger.OperationFilter<AssignOAuth2SecurityRequirements>();
                swagger.OperationFilter<SwaggerDefaultValues>();
                //swagger.DocumentFilter<ApplyDocumentVendorExtensions>();
                swagger.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                //swagger.CustomProvider((defaultProvider) => new CachingSwaggerProvider(defaultProvider));
            }).EnableSwaggerUi("Document/{*assetPath}"/*"Swagger/{*assetPath}"*/, swaggerui =>
            {
                swaggerui.DocumentTitle("WebClient Api Document");
                //swaggerui.InjectStylesheet(containingAssembly, "Swashbuckle.Dummy.SwaggerExtensions.testStyles1.css");
                swaggerui.InjectJavaScript(thisAssembly, "Lenneth.WebApi.Resources.swagger-zh-cn.js");
                //swaggerui.BooleanValues(new[] { "0", "1" });
                //swaggerui.DisableValidator();
                swaggerui.DocExpansion(DocExpansion.None);
                swaggerui.SupportedSubmitMethods("GET", "POST");
                swaggerui.CustomAsset("Default", thisAssembly, "Lenneth.WebApi.Resources.swagger-index.html");
                swaggerui.EnableDiscoveryUrlSelector();
                //swaggerui.EnableOAuth2Support(
                //    clientId: "test-client-id",
                //    clientSecret: null,
                //    realm: "test-realm",
                //    appName: "Swagger UI"
                //    //additionalQueryStringParams: new Dictionary<string, string>() { { "foo", "bar" } }
                //);
                swaggerui.EnableApiKeySupport("WebApi-key", "header");
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
                var basePath = AppDomain.CurrentDomain.RelativeSearchPath;
                const string filename = "Lenneth.WebApi.xml";

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