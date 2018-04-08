namespace Lenneth.Core.Framework.QueryBuilder
{
    public partial class Query
    {
        public Query AsDelete()
        {
            Method = "delete";
            return this;
        }

    }
}