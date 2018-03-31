namespace Lenneth.Core.Framework.MD
{
    public interface IMarkdown
    {
        string Version { get; }

        string Transform(string text);
    }
}