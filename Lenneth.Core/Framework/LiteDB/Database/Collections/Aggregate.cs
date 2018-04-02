using System;
using System.Linq.Expressions;

namespace Lenneth.Core.Framework.LiteDB
{
    public partial class LiteCollection<T>
    {
        #region Count

        /// <summary>
        /// Get document count using property on collection.
        /// </summary>
        public int Count()
        {
            // do not use indexes - collections has DocumentCount property
            return (int)_engine.Value.Count(_name);
        }

        /// <summary>
        /// Count documents matching a query. This method does not deserialize any document. Needs indexes on query expression
        /// </summary>
        public int Count(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return (int)_engine.Value.Count(_name, query);
        }

        /// <summary>
        /// Count documents matching a query. This method does not deserialize any documents. Needs indexes on query expression
        /// </summary>
        public int Count(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return Count(_visitor.Visit(predicate));
        }

        #endregion Count

        #region LongCount

        /// <summary>
        /// Get document count using property on collection.
        /// </summary>
        public long LongCount()
        {
            // do not use indexes - collections has DocumentCount property
            return _engine.Value.Count(_name);
        }

        /// <summary>
        /// Count documents matching a query. This method does not deserialize any documents. Needs indexes on query expression
        /// </summary>
        public long LongCount(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return _engine.Value.Count(_name, query);
        }

        /// <summary>
        /// Count documents matching a query. This method does not deserialize any documents. Needs indexes on query expression
        /// </summary>
        public long LongCount(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return LongCount(_visitor.Visit(predicate));
        }

        #endregion LongCount

        #region Exists

        /// <summary>
        /// Returns true if query returns any document. This method does not deserialize any document. Needs indexes on query expression
        /// </summary>
        public bool Exists(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return _engine.Value.Exists(_name, query);
        }

        /// <summary>
        /// Returns true if query returns any document. This method does not deserialize any document. Needs indexes on query expression
        /// </summary>
        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return Exists(_visitor.Visit(predicate));
        }

        #endregion Exists

        #region Min/Max

        /// <summary>
        /// Returns the first/min value from a index field
        /// </summary>
        public BsonValue Min(string field)
        {
            if (string.IsNullOrEmpty(field)) throw new ArgumentNullException(nameof(field));

            return _engine.Value.Min(_name, field);
        }

        /// <summary>
        /// Returns the first/min _id field
        /// </summary>
        public BsonValue Min()
        {
            return Min("_id");
        }

        /// <summary>
        /// Returns the first/min field using a linq expression
        /// </summary>
        public BsonValue Min<TK>(Expression<Func<T, TK>> property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var field = _visitor.GetField(property);

            return Min(field);
        }

        /// <summary>
        /// Returns the last/max value from a index field
        /// </summary>
        public BsonValue Max(string field)
        {
            if (string.IsNullOrEmpty(field)) throw new ArgumentNullException(nameof(field));

            return _engine.Value.Max(_name, field);
        }

        /// <summary>
        /// Returns the last/max _id field
        /// </summary>
        public BsonValue Max()
        {
            return Max("_id");
        }

        /// <summary>
        /// Returns the last/max field using a linq expression
        /// </summary>
        public BsonValue Max<TK>(Expression<Func<T, TK>> property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var field = _visitor.GetField(property);

            return Max(field);
        }

        #endregion Min/Max
    }
}