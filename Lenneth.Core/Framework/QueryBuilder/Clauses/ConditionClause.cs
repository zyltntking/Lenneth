using System.Collections.Generic;
using System.Linq;

namespace Lenneth.Core.Framework.QueryBuilder.Clauses
{
    public abstract class AbstractCondition : AbstractClause
    {
        public bool IsOr { get; set; } = false;
        public bool IsNot { get; set; } = false;
    }

    /// <summary>
    /// Represents a comparison between a column and a value.
    /// </summary>
    public class BasicCondition<T> : AbstractCondition
    {
        public string Column { get; set; }
        public string Operator { get; set; }
        public virtual T Value { get; set; }
        public override object[] GetBindings(string engine)
        {
            return new object[] { Value };
        }

        public override AbstractClause Clone()
        {
            return new BasicCondition<T>
            {
                Engine = Engine,
                Column = Column,
                Operator = Operator,
                Value = Value,
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
            };
        }
    }

    public class BasicStringCondition : BasicCondition<string>
    {
        public bool CaseSensitive { get; set; } = false;
        public override AbstractClause Clone()
        {
            return new BasicStringCondition
            {
                Engine = Engine,
                Column = Column,
                Operator = Operator,
                Value = Value,
                IsOr = IsOr,
                IsNot = IsNot,
                CaseSensitive = CaseSensitive,
                Component = Component,
            };
        }
    }

    /// <summary>
    /// Represents a comparison between two columns.
    /// </summary>
    public class TwoColumnsCondition : AbstractCondition
    {
        public string First { get; set; }
        public string Operator { get; set; }
        public string Second { get; set; }

        public override AbstractClause Clone()
        {
            return new TwoColumnsCondition
            {
                Engine = Engine,
                First = First,
                Operator = Operator,
                Second = Second,
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
            };
        }
    }

    /// <summary>
    /// Represents a comparison between a column and a full "subquery". 
    /// </summary>
    public class QueryCondition<T> : AbstractCondition where T : BaseQuery<T>
    {
        public string Column { get; set; }
        public string Operator { get; set; }
        public Query Query { get; set; }
        public override object[] GetBindings(string engine)
        {
            return Query.GetBindings(engine).ToArray();
        }

        public override AbstractClause Clone()
        {
            return new QueryCondition<T>
            {
                Engine = Engine,
                Column = Column,
                Operator = Operator,
                Query = Query.Clone(),
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
            };
        }
    }

    /// <summary>
    /// Represents a "is in" condition.
    /// </summary>
    public class InCondition<T> : AbstractCondition
    {
        public string Column { get; set; }
        public IEnumerable<T> Values { get; set; }
        public override object[] GetBindings(string engine)
        {
            return Values.Select(x => x).Cast<object>().ToArray();
        }

        public override AbstractClause Clone()
        {
            return new InCondition<T>
            {
                Engine = Engine,
                Column = Column,
                Values = new List<T>(Values),
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
            };
        }

    }

    /// <summary>
    /// Represents a "is in subquery" condition.
    /// </summary>
    public class InQueryCondition : AbstractCondition
    {
        public Query Query { get; set; }
        public string Column { get; set; }
        public override object[] GetBindings(string engine)
        {
            return Query.GetBindings(engine).ToArray();
        }
        public override AbstractClause Clone()
        {
            return new InQueryCondition
            {
                Engine = Engine,
                Column = Column,
                Query = Query.Clone(),
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
            };
        }
    }

    /// <summary>
    /// Represents a "is between" condition.
    /// </summary>
    public class BetweenCondition<T> : AbstractCondition
    {
        public string Column { get; set; }
        public T Higher { get; set; }
        public T Lower { get; set; }
        public override object[] GetBindings(string engine)
        {
            return new object[] { Lower, Higher };
        }

        public override AbstractClause Clone()
        {
            return new BetweenCondition<T>
            {
                Engine = Engine,
                Column = Column,
                Higher = Higher,
                Lower = Lower,
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
            };
        }
    }

    /// <summary>
    /// Represents an "is null" condition.
    /// </summary>
    public class NullCondition : AbstractCondition
    {
        public string Column { get; set; }

        public override AbstractClause Clone()
        {
            return new NullCondition
            {
                Engine = Engine,
                Column = Column,
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
            };
        }
    }

    /// <summary>
    /// Represents a "nested" clause condition.
    /// i.e OR (myColumn = "A")
    /// </summary>
    public class NestedCondition<T> : AbstractCondition where T : BaseQuery<T>
    {
        public T Query { get; set; }
        public override object[] GetBindings(string engine)
        {
            return Query.GetBindings(engine).ToArray();
        }

        public override AbstractClause Clone()
        {
            return new NestedCondition<T>
            {
                Engine = Engine,
                Query = Query.Clone(),
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
            };
        }
    }

    /// <summary>
    /// Represents an "exists sub query" clause condition.
    /// </summary>
    public class ExistsCondition<T> : AbstractCondition where T : BaseQuery<T>
    {
        public T Query { get; set; }
        public override object[] GetBindings(string engine)
        {
            return Query.GetBindings(engine).ToArray();
        }

        public override AbstractClause Clone()
        {
            return new ExistsCondition<T>
            {
                Engine = Engine,
                Query = Query.Clone(),
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
            };
        }
    }

    public class RawCondition : AbstractCondition, RawInterface
    {
        public string Expression { get; set; }
        protected object[] _bindings;
        public object[] Bindings { set => _bindings = value; }
        public override object[] GetBindings(string engine)
        {
            return _bindings;
        }

        public override AbstractClause Clone()
        {
            return new RawCondition
            {
                Engine = Engine,
                Expression = Expression,
                _bindings = _bindings,
                IsOr = IsOr,
                IsNot = IsNot,
                Component = Component,
            };
        }
    }

}