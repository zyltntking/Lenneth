using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lenneth.Core.Framework.LiteDB
{
    public sealed class BsonArray : BsonValue, IList<BsonValue>
    {
        public BsonArray()
            : base(new List<BsonValue>())
        {
        }

        public BsonArray(List<BsonValue> array)
            : base(array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
        }

        public BsonArray(BsonValue[] array)
            : base(new List<BsonValue>(array))
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
        }

        public BsonArray(IEnumerable<BsonValue> items)
            : this()
        {
            AddRange(items);
        }

        public BsonArray(IEnumerable<BsonArray> items)
            : this()
        {
            AddRange(items);
        }

        public BsonArray(IEnumerable<BsonDocument> items)
            : this()
        {
            AddRange(items);
        }

        public new List<BsonValue> RawValue
        {
            get
            {
                return (List<BsonValue>)base.RawValue;
            }
        }

        public BsonValue this[int index]
        {
            get
            {
                return RawValue.ElementAt(index);
            }
            set
            {
                RawValue[index] = value ?? Null;
            }
        }

        public int Count
        {
            get
            {
                return RawValue.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(BsonValue item)
        {
            RawValue.Add(item ?? Null);
        }

        public void AddRange<T>(IEnumerable<T> array)
            where T : BsonValue
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            foreach (var item in array)
            {
                Add(item ?? Null);
            }
        }

        public void Clear()
        {
            RawValue.Clear();
        }

        public bool Contains(BsonValue item)
        {
            return RawValue.Contains(item);
        }

        public void CopyTo(BsonValue[] array, int arrayIndex)
        {
            RawValue.CopyTo(array, arrayIndex);
        }

        public IEnumerator<BsonValue> GetEnumerator()
        {
            return RawValue.GetEnumerator();
        }

        public int IndexOf(BsonValue item)
        {
            return RawValue.IndexOf(item);
        }

        public void Insert(int index, BsonValue item)
        {
            RawValue.Insert(index, item);
        }

        public bool Remove(BsonValue item)
        {
            return RawValue.Remove(item);
        }

        public void RemoveAt(int index)
        {
            RawValue.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var value in RawValue)
            {
                yield return new BsonValue(value);
            }
        }

        public override int CompareTo(BsonValue other)
        {
            // if types are different, returns sort type order
            if (other.Type != BsonType.Document) return Type.CompareTo(other.Type);

            var otherArray = other.AsArray;

            var result = 0;
            var i = 0;
            var stop = Math.Min(Count, otherArray.Count);

            // compare each element
            for (; 0 == result && i < stop; i++)
                result = this[i].CompareTo(otherArray[i]);

            if (result != 0) return result;
            if (i == Count) return i == otherArray.Count ? 0 : -1;
            return 1;
        }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}