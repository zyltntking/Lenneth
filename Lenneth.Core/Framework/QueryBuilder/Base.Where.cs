using System;
using System.Collections.Generic;
using System.Linq;
using Lenneth.Core.Framework.QueryBuilder.Clauses;

namespace Lenneth.Core.Framework.QueryBuilder
{
    public abstract partial class BaseQuery<TQ>
    {
        public TQ Where<T>(string column, string op, T value)
        {

            // If the value is "null", we will just assume the developer wants to add a
            // where null clause to the query. So, we will allow a short-cut here to
            // that method for convenience so the developer doesn't have to check.
            if (value == null)
            {
                return Not(op != "=").WhereNull(column);
            }

            if (value is Query)
            {
                return Where(column, op, value as Query);
            }

            return AddComponent("where", new BasicCondition<T>
            {
                Column = column,
                Operator = op,
                Value = value,
                IsOr = getOr(),
                IsNot = getNot(),
            });
        }

        public TQ Where<T>(string column, T value)
        {
            return Where(column, "=", value);
        }

        public TQ Where<T>(IReadOnlyDictionary<string, T> values)
        {
            var query = (TQ)this;
            var orFlag = getOr();
            var notFlag = getNot();

            foreach (var tuple in values)
            {
                if (orFlag)
                {
                    query = query.Or();
                }
                else
                {
                    query.And();
                }

                query = this.Not(notFlag).Where(tuple.Key, tuple.Value);
            }

            return query;
        }

        public TQ Where<T>(IEnumerable<string> columns, IEnumerable<T> values)
        {
            if (columns.Count() == 0 || columns.Count() != values.Count())
            {
                throw new ArgumentException("Columns and Values count must match");
            }

            var query = (TQ)this;

            var orFlag = getOr();
            var notFlag = getNot();

            for (var i = 0; i < columns.Count(); i++)
            {
                if (orFlag)
                {
                    query = query.Or();
                }
                else
                {
                    query.And();
                }

                query = this.Not(notFlag).Where(columns.ElementAt(i), values.ElementAt(i));
            }

            return query;
        }

        /// <summary>
        /// Apply the Where clause changes if the given "condition" is true.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public TQ WhereIf<T>(bool condition, string column, string op, T value)
        {
            if (condition)
            {
                return Where(column, op, value);
            }

            return (TQ)this;
        }

        public TQ WhereIf<T>(bool condition, string column, T value)
        {
            return WhereIf(condition, column, "=", value);
        }

        /// <summary>
        /// Apply the Or Where clause changes if the given "condition" is true.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public TQ OrWhereIf<T>(bool condition, string column, string op, T value)
        {
            if (condition)
            {
                return Or().Where(column, op, value);
            }

            return (TQ)this;
        }

        public TQ OrWhereIf<T>(bool condition, string column, T value)
        {
            return OrWhereIf(condition, column, "=", value);
        }

        public TQ WhereRaw(string sql, params object[] bindings)
        {
            return AddComponent("where", new RawCondition
            {
                Expression = sql,
                Bindings = Helper.Flatten(bindings).ToArray(),
                IsOr = getOr(),
                IsNot = getNot(),
            });
        }

        public TQ OrWhereRaw(string sql, params object[] bindings)
        {
            return Or().WhereRaw(sql, bindings);
        }

        public TQ Where(Func<TQ, TQ> callback)
        {
            var query = callback.Invoke(NewChild());

            return AddComponent("where", new NestedCondition<TQ>
            {
                Query = query,
                IsNot = getNot(),
                IsOr = getOr(),
            });
        }

        public TQ WhereNot(string column, string op, object value)
        {
            return Not(true).Where(column, op, value);
        }

        public TQ WhereNot(string column, object value)
        {
            return WhereNot(column, "=", value);
        }

        public TQ WhereNot(Func<TQ, TQ> callback)
        {
            return Not(true).Where(callback);
        }

        public TQ OrWhere(string column, string op, object value)
        {
            return Or().Where(column, op, value);
        }
        public TQ OrWhere(string column, object value)
        {
            return OrWhere(column, "=", value);
        }

        public TQ OrWhere(Func<TQ, TQ> callback)
        {
            return Or().Where(callback);
        }
        public TQ OrWhereNot(string column, string op, object value)
        {
            return this.Or().Not(true).Where(column, op, value);
        }
        public TQ OrWhereNot(string column, object value)
        {
            return OrWhereNot(column, "=", value);
        }

        public TQ OrWhereNot(Func<TQ, TQ> callback)
        {
            return Not(true).Or().Where(callback);
        }

        public TQ WhereColumns(string first, string op, string second)
        {
            return AddComponent("where", new TwoColumnsCondition
            {
                First = first,
                Second = second,
                Operator = op,
                IsOr = getOr(),
                IsNot = getNot(),
            });
        }

        public TQ OrWhereColumns(string first, string op, string second)
        {
            return Or().WhereColumns(first, op, second);
        }

        public TQ WhereNull(string column)
        {
            return AddComponent("where", new NullCondition
            {
                Column = column,
                IsOr = getOr(),
                IsNot = getNot(),
            });
        }

        public TQ WhereNotNull(string column)
        {
            return Not(true).WhereNull(column);
        }

        public TQ OrWhereNull(string column)
        {
            return this.Or().WhereNull(column);
        }

        public TQ OrWhereNotNull(string column)
        {
            return Or().Not(true).WhereNull(column);
        }

        public TQ WhereLike(string column, string value, bool caseSensitive = false)
        {
            return AddComponent("where", new BasicStringCondition
            {
                Operator = "like",
                Column = column,
                Value = value,
                CaseSensitive = caseSensitive,
                IsOr = getOr(),
                IsNot = getNot(),
            });
        }

        public TQ OrWhereLike(string column, string value, bool caseSensitive = false)
        {
            return Or().WhereLike(column, value, caseSensitive);
        }

        public TQ OrWhereNotLike(string column, string value, bool caseSensitive = false)
        {
            return Or().Not(true).WhereLike(column, value, caseSensitive);
        }
        public TQ WhereStarts(string column, string value, bool caseSensitive = false)
        {
            return AddComponent("where", new BasicStringCondition
            {
                Operator = "starts",
                Column = column,
                Value = value,
                CaseSensitive = caseSensitive,
                IsOr = getOr(),
                IsNot = getNot(),
            });
        }

        public TQ OrWhereStarts(string column, string value, bool caseSensitive = false)
        {
            return Or().WhereStarts(column, value, caseSensitive);
        }

        public TQ OrWhereNotStarts(string column, string value, bool caseSensitive = false)
        {
            return Or().Not(true).WhereStarts(column, value, caseSensitive);
        }

        public TQ WhereEnds(string column, string value, bool caseSensitive = false)
        {
            return AddComponent("where", new BasicStringCondition
            {
                Operator = "ends",
                Column = column,
                Value = value,
                CaseSensitive = caseSensitive,
                IsOr = getOr(),
                IsNot = getNot(),
            });
        }

        public TQ OrWhereEnds(string column, string value, bool caseSensitive = false)
        {
            return Or().WhereEnds(column, value, caseSensitive);
        }

        public TQ OrWhereNotEnds(string column, string value, bool caseSensitive = false)
        {
            return Or().Not(true).WhereEnds(column, value, caseSensitive);
        }

        public TQ WhereContains(string column, string value, bool caseSensitive = false)
        {
            return AddComponent("where", new BasicStringCondition
            {
                Operator = "contains",
                Column = column,
                Value = value,
                CaseSensitive = caseSensitive,
                IsOr = getOr(),
                IsNot = getNot(),
            });
        }

        public TQ OrWhereContains(string column, string value, bool caseSensitive = false)
        {
            return Or().WhereContains(column, value, caseSensitive);
        }

        public TQ OrWhereNotContains(string column, string value, bool caseSensitive = false)
        {
            return Or().Not(true).WhereContains(column, value, caseSensitive);
        }

        public TQ WhereBetween<T>(string column, T lower, T higher)
        {
            return AddComponent("where", new BetweenCondition<T>
            {
                Column = column,
                IsOr = getOr(),
                IsNot = getNot(),
                Lower = lower,
                Higher = higher
            });
        }

        public TQ OrWhereBetween<T>(string column, T lower, T higher)
        {
            return Or().WhereBetween(column, lower, higher);
        }
        public TQ WhereNotBetween<T>(string column, T lower, T higher)
        {
            return Not(true).WhereBetween(column, lower, higher);
        }
        public TQ OrWhereNotBetween<T>(string column, T lower, T higher)
        {
            return Or().Not(true).WhereBetween(column, lower, higher);
        }

        public TQ WhereIn<T>(string column, IEnumerable<T> values)
        {
            // If the developer has passed a string most probably he wants List<string>
            // since string is considered as List<char>
            if (values is string)
            {
                string val = values as string;

                return AddComponent("where", new InCondition<string>
                {
                    Column = column,
                    IsOr = getOr(),
                    IsNot = getNot(),
                    Values = new List<string> { val }
                });
            }

            return AddComponent("where", new InCondition<T>
            {
                Column = column,
                IsOr = getOr(),
                IsNot = getNot(),
                Values = values.Distinct().ToList()
            });


        }

        public TQ OrWhereIn<T>(string column, IEnumerable<T> values)
        {
            return Or().WhereIn(column, values);
        }

        public TQ WhereNotIn<T>(string column, IEnumerable<T> values)
        {
            return Not(true).WhereIn(column, values);
        }

        public TQ OrWhereNotIn<T>(string column, IEnumerable<T> values)
        {
            return Or().Not(true).WhereIn(column, values);
        }


        public TQ WhereIn(string column, Query query)
        {
            return AddComponent("where", new InQueryCondition
            {
                Column = column,
                IsOr = getOr(),
                IsNot = getNot(),
                Query = query.SetEngineScope(EngineScope)
            });
        }
        public TQ WhereIn(string column, Func<Query, Query> callback)
        {
            var query = callback.Invoke(new Query());

            return WhereIn(column, query);
        }

        public TQ OrWhereIn(string column, Query query)
        {
            return Or().WhereIn(column, query);
        }

        public TQ OrWhereIn(string column, Func<Query, Query> callback)
        {
            return Or().WhereIn(column, callback);
        }
        public TQ WhereNotIn(string column, Query query)
        {
            return Not(true).WhereIn(column, query);
        }

        public TQ WhereNotIn(string column, Func<Query, Query> callback)
        {
            return Not(true).WhereIn(column, callback);
        }

        public TQ OrWhereNotIn(string column, Query query)
        {
            return Or().Not(true).WhereIn(column, query);
        }

        public TQ OrWhereNotIn(string column, Func<Query, Query> callback)
        {
            return Or().Not(true).WhereIn(column, callback);
        }


        /// <summary>
        /// Perform a sub query where clause
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public TQ Where(string column, string op, Func<TQ, TQ> callback)
        {
            var query = callback.Invoke(NewChild());

            return Where(column, op, query);
        }

        public TQ Where(string column, string op, Query query)
        {
            return AddComponent("where", new QueryCondition<Query>
            {
                Column = column,
                Operator = op,
                Query = query.SetEngineScope(EngineScope),
                IsNot = getNot(),
                IsOr = getOr(),
            });
        }

        public TQ OrWhere(string column, string op, Query query)
        {
            return Or().Where(column, op, query);
        }
        public TQ OrWhere(string column, string op, Func<Query, Query> callback)
        {
            return Or().Where(column, op, callback);
        }

        public TQ WhereExists(Query query)
        {
            if (!query.HasComponent("from"))
            {
                throw new ArgumentException(@"""FromClause"" cannot be empty if used inside of a ""WhereExists"" condition");
            }

            return AddComponent("where", new ExistsCondition<Query>
            {
                Query = query.ClearComponent("select").SelectRaw("1").Limit(1).SetEngineScope(EngineScope),
                IsNot = getNot(),
                IsOr = getOr(),
            });
        }
        public TQ WhereExists(Func<Query, Query> callback)
        {
            var childQuery = new Query().SetParent(this);
            return WhereExists(callback.Invoke(childQuery));
        }

        public TQ WhereNotExists(Query query)
        {
            return Not(true).WhereExists(query);
        }

        public TQ WhereNotExists(Func<Query, Query> callback)
        {
            return Not(true).WhereExists(callback);
        }

        public TQ OrWhereExists(Query query)
        {
            return Or().WhereExists(query);
        }
        public TQ OrWhereExists(Func<Query, Query> callback)
        {
            return Or().WhereExists(callback);
        }
        public TQ OrWhereNotExists(Query query)
        {
            return Or().Not(true).WhereExists(query);
        }
        public TQ OrWhereNotExists(Func<Query, Query> callback)
        {
            return Or().Not(true).WhereExists(callback);
        }

    }
}