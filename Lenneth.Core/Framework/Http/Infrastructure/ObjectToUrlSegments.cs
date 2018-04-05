using static System.Web.HttpUtility;

namespace Lenneth.Core.Framework.Http.Infrastructure
{
    internal class ObjectToUrlSegments : ObjectToUrl
    {
        protected override string PathStartCharacter => "/";

        protected override string PathSeparatorCharacter => "/";

        protected override string BuildParam(PropertyValue propertyValue) => UrlEncode(propertyValue.Value);
    }
}