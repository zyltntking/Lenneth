namespace Lenneth.Core.Framework.Http.Client
{
    public enum CacheControl
    {
        NoCache,
        MaxAge,
        MaxAgeWithMaxStale,
        MaxAgeWithMinFresh
    }
}