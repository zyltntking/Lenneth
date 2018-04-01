namespace Lenneth.Core.Framework.Http.Codecs
{
    internal interface IMemberBehavior
    {
        void SetDefaultValue(string memberName, object value);
    }
}