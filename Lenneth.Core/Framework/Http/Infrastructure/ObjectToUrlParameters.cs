using static System.Web.HttpUtility;

namespace Lenneth.Core.Framework.Http.Infrastructure
{
    internal class ObjectToUrlParameters : ObjectToUrl
    {
        protected override string PathStartCharacter => "?";

        protected override string PathSeparatorCharacter => "&";

        protected override string BuildParam(PropertyValue propertyValue) => string.Join("=", propertyValue.Name, UrlEncode(propertyValue.Value));
    }
}