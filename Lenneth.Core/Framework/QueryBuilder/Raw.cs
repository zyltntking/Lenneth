using System.Collections.Generic;
using System.Linq;

namespace Lenneth.Core.Framework.QueryBuilder
{
    public class Raw
    {
        private readonly string _value;
        
        public List<object> Bindings { get; set; } = new List<object>();
        
        public string Value
        {
            get
            {
                return _value;
            }
        }

        public Raw(string value, params object[] bindings)
        {
            Bindings = bindings.ToList();
            _value = value;
        }

        public override string ToString()
        {
            return _value;
        }

    }
}