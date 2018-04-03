using System;
using System.Collections.Generic;
using System.Text;

namespace Lenneth.Core.Framework.LiteDB
{
    /// <summary>
    /// Represent a Bson Value used in BsonDocument
    /// </summary>
    public class BsonValue : IComparable<BsonValue>, IEquatable<BsonValue>
    {
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Represent a Null bson type
        /// </summary>
        public static readonly BsonValue Null = new BsonValue();

        /// <summary>
        /// Represent a MinValue bson type
        /// </summary>
        public static readonly BsonValue MinValue = new BsonValue { Type = BsonType.MinValue, RawValue = "-oo" };

        /// <summary>
        /// Represent a MaxValue bson type
        /// </summary>
        public static readonly BsonValue MaxValue = new BsonValue { Type = BsonType.MaxValue, RawValue = "+oo" };

        /// <summary>
        /// Indicate BsonType of this BsonValue
        /// </summary>
        public BsonType Type { get; private set; }

        /// <summary>
        /// Get internal .NET value object
        /// </summary>
        public virtual object RawValue { get; private set; }

        /// <summary>
        /// Internal destroy method. Works only when used with BsonExpression
        /// </summary>
        internal Action Destroy = () => { };

        #region Constructor

        public BsonValue()
        {
            Type = BsonType.Null;
            RawValue = null;
        }

        public BsonValue(Int32 value)
        {
            Type = BsonType.Int32;
            RawValue = value;
        }

        public BsonValue(Int64 value)
        {
            Type = BsonType.Int64;
            RawValue = value;
        }

        public BsonValue(Double value)
        {
            Type = BsonType.Double;
            RawValue = value;
        }

        public BsonValue(Decimal value)
        {
            Type = BsonType.Decimal;
            RawValue = value;
        }

        public BsonValue(String value)
        {
            Type = value == null ? BsonType.Null : BsonType.String;
            RawValue = value;
        }

        public BsonValue(Dictionary<string, BsonValue> value)
        {
            Type = value == null ? BsonType.Null : BsonType.Document;
            RawValue = value;
        }

        public BsonValue(List<BsonValue> value)
        {
            Type = value == null ? BsonType.Null : BsonType.Array;
            RawValue = value;
        }

        public BsonValue(Byte[] value)
        {
            Type = value == null ? BsonType.Null : BsonType.Binary;
            RawValue = value;
        }

        public BsonValue(ObjectId value)
        {
            Type = value == null ? BsonType.Null : BsonType.ObjectId;
            RawValue = value;
        }

        public BsonValue(Guid value)
        {
            Type = BsonType.Guid;
            RawValue = value;
        }

        public BsonValue(Boolean value)
        {
            Type = BsonType.Boolean;
            RawValue = value;
        }

        public BsonValue(DateTime value)
        {
            Type = BsonType.DateTime;
            RawValue = value.Truncate();
        }

        public BsonValue(BsonValue value)
        {
            Type = value == null ? BsonType.Null : value.Type;
            RawValue = value.RawValue;
        }

        public BsonValue(object value)
        {
            RawValue = value;

            if (value == null) Type = BsonType.Null;
            else if (value is Int32) Type = BsonType.Int32;
            else if (value is Int64) Type = BsonType.Int64;
            else if (value is Double) Type = BsonType.Double;
            else if (value is Decimal) Type = BsonType.Decimal;
            else if (value is String) Type = BsonType.String;
            else if (value is Dictionary<string, BsonValue>) Type = BsonType.Document;
            else if (value is List<BsonValue>) Type = BsonType.Array;
            else if (value is Byte[]) Type = BsonType.Binary;
            else if (value is ObjectId) Type = BsonType.ObjectId;
            else if (value is Guid) Type = BsonType.Guid;
            else if (value is Boolean) Type = BsonType.Boolean;
            else if (value is DateTime)
            {
                Type = BsonType.DateTime;
                RawValue = ((DateTime)value).Truncate();
            }
            else if (value is BsonValue)
            {
                var v = (BsonValue)value;
                Type = v.Type;
                RawValue = v.RawValue;
            }
            else
            {
                // test for array or dictionary (document)
                var enumerable = value as System.Collections.IEnumerable;
                var dictionary = value as System.Collections.IDictionary;

                // test first for dictionary (because IDictionary implements IEnumerable)
                if (dictionary != null)
                {
                    var dict = new Dictionary<string, BsonValue>();

                    foreach (var key in dictionary.Keys)
                    {
                        dict.Add(key.ToString(), new BsonValue(dictionary[key]));
                    }

                    Type = BsonType.Document;
                    RawValue = dict;
                }
                else if (enumerable != null)
                {
                    var list = new List<BsonValue>();

                    foreach (var x in enumerable)
                    {
                        list.Add(new BsonValue(x));
                    }

                    Type = BsonType.Array;
                    RawValue = list;
                }
                else
                {
                    throw new InvalidCastException("Value is not a valid BSON data type - Use Mapper.ToDocument for more complex types converts");
                }
            }
        }

        #endregion

        #region Convert types

        public BsonArray AsArray
        {
            get
            {
                if (IsArray)
                {
                    var array = new BsonArray((List<BsonValue>)RawValue);
                    array.Length = Length;
                    array.Destroy = Destroy;

                    return array;
                }
                else
                {
                    return default(BsonArray);
                }
            }
        }

        public BsonDocument AsDocument
        {
            get
            {
                if (IsDocument)
                {
                    var doc = new BsonDocument((Dictionary<string, BsonValue>)RawValue);
                    doc.Length = Length;
                    doc.Destroy = Destroy;

                    return doc;
                }
                else
                {
                    return default(BsonDocument);
                }
            }
        }

        public Byte[] AsBinary
        {
            get { return Type == BsonType.Binary ? (Byte[])RawValue : default(Byte[]); }
        }

        public bool AsBoolean
        {
            get { return Type == BsonType.Boolean ? (Boolean)RawValue : default(Boolean); }
        }

        public string AsString
        {
            get { return Type != BsonType.Null ? RawValue.ToString() : default(String); }
        }

        public int AsInt32
        {
            get { return IsNumber ? Convert.ToInt32(RawValue) : default(Int32); }
        }

        public long AsInt64
        {
            get { return IsNumber ? Convert.ToInt64(RawValue) : default(Int64); }
        }

        public double AsDouble
        {
            get { return IsNumber ? Convert.ToDouble(RawValue) : default(Double); }
        }

        public decimal AsDecimal
        {
            get { return IsNumber ? Convert.ToDecimal(RawValue) : default(Decimal); }
        }

        public DateTime AsDateTime
        {
            get { return Type == BsonType.DateTime ? (DateTime)RawValue : default(DateTime); }
        }

        public ObjectId AsObjectId
        {
            get { return Type == BsonType.ObjectId ? (ObjectId)RawValue : default(ObjectId); }
        }

        public Guid AsGuid
        {
            get { return Type == BsonType.Guid ? (Guid)RawValue : default(Guid); }
        }

        #endregion

        #region IsTypes

        public bool IsNull
        {
            get { return Type == BsonType.Null; }
        }

        public bool IsArray
        {
            get { return Type == BsonType.Array; }
        }

        public bool IsDocument
        {
            get { return Type == BsonType.Document; }
        }

        public bool IsInt32
        {
            get { return Type == BsonType.Int32; }
        }

        public bool IsInt64
        {
            get { return Type == BsonType.Int64; }
        }

        public bool IsDouble
        {
            get { return Type == BsonType.Double; }
        }

        public bool IsDecimal
        {
            get { return Type == BsonType.Decimal; }
        }

        public bool IsNumber
        {
            get { return IsInt32 || IsInt64 || IsDouble || IsDecimal; }
        }

        public bool IsBinary
        {
            get { return Type == BsonType.Binary; }
        }

        public bool IsBoolean
        {
            get { return Type == BsonType.Boolean; }
        }

        public bool IsString
        {
            get { return Type == BsonType.String; }
        }

        public bool IsObjectId
        {
            get { return Type == BsonType.ObjectId; }
        }

        public bool IsGuid
        {
            get { return Type == BsonType.Guid; }
        }

        public bool IsDateTime
        {
            get { return Type == BsonType.DateTime; }
        }

        public bool IsMinValue
        {
            get { return Type == BsonType.MinValue; }
        }

        public bool IsMaxValue
        {
            get { return Type == BsonType.MaxValue; }
        }

        #endregion

        #region Implicit Ctor

        // Int32
        public static implicit operator Int32(BsonValue value)
        {
            return (Int32)value.RawValue;
        }

        // Int32
        public static implicit operator BsonValue(Int32 value)
        {
            return new BsonValue(value);
        }

        // Int64
        public static implicit operator Int64(BsonValue value)
        {
            return (Int64)value.RawValue;
        }

        // Int64
        public static implicit operator BsonValue(Int64 value)
        {
            return new BsonValue(value);
        }

        // Double
        public static implicit operator Double(BsonValue value)
        {
            return (Double)value.RawValue;
        }

        // Double
        public static implicit operator BsonValue(Double value)
        {
            return new BsonValue(value);
        }

        // Decimal
        public static implicit operator Decimal(BsonValue value)
        {
            return (Decimal)value.RawValue;
        }

        // Decimal
        public static implicit operator BsonValue(Decimal value)
        {
            return new BsonValue(value);
        }

        // UInt64 (to avoid ambigous between Double-Decimal)
        public static implicit operator UInt64(BsonValue value)
        {
            return (UInt64)value.RawValue;
        }

        // Decimal
        public static implicit operator BsonValue(UInt64 value)
        {
            return new BsonValue((Double)value);
        }

        // String
        public static implicit operator String(BsonValue value)
        {
            return (String)value.RawValue;
        }

        // String
        public static implicit operator BsonValue(String value)
        {
            return new BsonValue(value);
        }

        // Document
        public static implicit operator Dictionary<string, BsonValue>(BsonValue value)
        {
            return (Dictionary<string, BsonValue>)value.RawValue;
        }

        // Document
        public static implicit operator BsonValue(Dictionary<string, BsonValue> value)
        {
            return new BsonValue(value);
        }

        // Array
        public static implicit operator List<BsonValue>(BsonValue value)
        {
            return (List<BsonValue>)value.RawValue;
        }

        // Array
        public static implicit operator BsonValue(List<BsonValue> value)
        {
            return new BsonValue(value);
        }

        // Binary
        public static implicit operator Byte[] (BsonValue value)
        {
            return (Byte[])value.RawValue;
        }

        // Binary
        public static implicit operator BsonValue(Byte[] value)
        {
            return new BsonValue(value);
        }

        // ObjectId
        public static implicit operator ObjectId(BsonValue value)
        {
            return (ObjectId)value.RawValue;
        }

        // ObjectId
        public static implicit operator BsonValue(ObjectId value)
        {
            return new BsonValue(value);
        }

        // Guid
        public static implicit operator Guid(BsonValue value)
        {
            return (Guid)value.RawValue;
        }

        // Guid
        public static implicit operator BsonValue(Guid value)
        {
            return new BsonValue(value);
        }

        // Boolean
        public static implicit operator Boolean(BsonValue value)
        {
            return (Boolean)value.RawValue;
        }

        // Boolean
        public static implicit operator BsonValue(Boolean value)
        {
            return new BsonValue(value);
        }

        // DateTime
        public static implicit operator DateTime(BsonValue value)
        {
            return (DateTime)value.RawValue;
        }

        // DateTime
        public static implicit operator BsonValue(DateTime value)
        {
            return new BsonValue(value);
        }

        // +
        public static BsonValue operator +(BsonValue left, BsonValue right)
        {
            if (!left.IsNumber || !right.IsNumber) return Null;

            if (left.IsInt32 && right.IsInt32) return left.AsInt32 + right.AsInt32;
            if (left.IsInt64 && right.IsInt64) return left.AsInt64 + right.AsInt64;
            if (left.IsDouble && right.IsDouble) return left.AsDouble + right.AsDouble;
            if (left.IsDecimal && right.IsDecimal) return left.AsDecimal + right.AsDecimal;

            var result = left.AsDecimal + right.AsDecimal;
            var type = (BsonType)Math.Max((int)left.Type, (int)right.Type);

            return
                type == BsonType.Int64 ? new BsonValue((Int64)result) :
                type == BsonType.Double ? new BsonValue((Double)result) :
                new BsonValue(result);
        }

        // -
        public static BsonValue operator -(BsonValue left, BsonValue right)
        {
            if (!left.IsNumber || !right.IsNumber) return Null;

            if (left.IsInt32 && right.IsInt32) return left.AsInt32 - right.AsInt32;
            if (left.IsInt64 && right.IsInt64) return left.AsInt64 - right.AsInt64;
            if (left.IsDouble && right.IsDouble) return left.AsDouble - right.AsDouble;
            if (left.IsDecimal && right.IsDecimal) return left.AsDecimal - right.AsDecimal;

            var result = left.AsDecimal - right.AsDecimal;
            var type = (BsonType)Math.Max((int)left.Type, (int)right.Type);

            return
                type == BsonType.Int64 ? new BsonValue((Int64)result) :
                type == BsonType.Double ? new BsonValue((Double)result) :
                new BsonValue(result);
        }

        // *
        public static BsonValue operator *(BsonValue left, BsonValue right)
        {
            if (!left.IsNumber || !right.IsNumber) return Null;

            if (left.IsInt32 && right.IsInt32) return left.AsInt32 * right.AsInt32;
            if (left.IsInt64 && right.IsInt64) return left.AsInt64 * right.AsInt64;
            if (left.IsDouble && right.IsDouble) return left.AsDouble * right.AsDouble;
            if (left.IsDecimal && right.IsDecimal) return left.AsDecimal * right.AsDecimal;

            var result = left.AsDecimal * right.AsDecimal;
            var type = (BsonType)Math.Max((int)left.Type, (int)right.Type);

            return
                type == BsonType.Int64 ? new BsonValue((Int64)result) :
                type == BsonType.Double ? new BsonValue((Double)result) :
                new BsonValue(result);
        }

        // /
        public static BsonValue operator /(BsonValue left, BsonValue right)
        {
            if (!left.IsNumber || !right.IsNumber) return Null;
            if (left.IsDecimal || right.IsDecimal) return left.AsDecimal / right.AsDecimal;

            return left.AsDouble / right.AsDouble;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        #endregion

        #region IComparable<BsonValue>, IEquatable<BsonValue>

        public virtual int CompareTo(BsonValue other)
        {
            // first, test if types are different
            if (Type != other.Type)
            {
                // if both values are number, convert them to Decimal (128 bits) to compare
                // it's the slowest way, but more secure
                if (IsNumber && other.IsNumber)
                {
                    return Convert.ToDecimal(RawValue).CompareTo(Convert.ToDecimal(other.RawValue));
                }
                // if not, order by sort type order
                else
                {
                    return Type.CompareTo(other.Type);
                }
            }

            // for both values with same data type just compare
            switch (Type)
            {
                case BsonType.Null:
                case BsonType.MinValue:
                case BsonType.MaxValue:
                    return 0;

                case BsonType.Int32: return ((Int32)RawValue).CompareTo((Int32)other.RawValue);
                case BsonType.Int64: return ((Int64)RawValue).CompareTo((Int64)other.RawValue);
                case BsonType.Double: return ((Double)RawValue).CompareTo((Double)other.RawValue);
                case BsonType.Decimal: return ((Decimal)RawValue).CompareTo((Decimal)other.RawValue);

                case BsonType.String: return string.Compare((String)RawValue, (String)other.RawValue);

                case BsonType.Document: return AsDocument.CompareTo(other);
                case BsonType.Array: return AsArray.CompareTo(other);

                case BsonType.Binary: return ((Byte[])RawValue).BinaryCompareTo((Byte[])other.RawValue);
                case BsonType.ObjectId: return ((ObjectId)RawValue).CompareTo((ObjectId)other.RawValue);
                case BsonType.Guid: return ((Guid)RawValue).CompareTo((Guid)other.RawValue);

                case BsonType.Boolean: return ((Boolean)RawValue).CompareTo((Boolean)other.RawValue);
                case BsonType.DateTime:
                    var d0 = (DateTime)RawValue;
                    var d1 = (DateTime)other.RawValue;
                    if (d0.Kind != DateTimeKind.Utc) d0 = d0.ToUniversalTime();
                    if (d1.Kind != DateTimeKind.Utc) d1 = d1.ToUniversalTime();
                    return d0.CompareTo(d1);

                default: throw new NotImplementedException();
            }
        }

        public bool Equals(BsonValue other)
        {
            return CompareTo(other) == 0;
        }

        #endregion

        #region Operators

        public static bool operator ==(BsonValue lhs, BsonValue rhs)
        {
            if (ReferenceEquals(lhs, null)) return ReferenceEquals(rhs, null);
            if (ReferenceEquals(rhs, null)) return false; // don't check type because sometimes different types can be ==

            return lhs.Equals(rhs);
        }

        public static bool operator !=(BsonValue lhs, BsonValue rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator >=(BsonValue lhs, BsonValue rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }

        public static bool operator >(BsonValue lhs, BsonValue rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }

        public static bool operator <(BsonValue lhs, BsonValue rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }

        public static bool operator <=(BsonValue lhs, BsonValue rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }

        public override bool Equals(object obj)
        {
            return Equals(new BsonValue(obj));
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = 37 * hash + Type.GetHashCode();
            hash = 37 * hash + RawValue.GetHashCode();
            return hash;
        }

        #endregion

        #region GetBytesCount

        internal int? Length = null;

        /// <summary>
        /// Returns how many bytes this BsonValue will use to persist in index writes
        /// </summary>
        public int GetBytesCount(bool recalc)
        {
            if (recalc == false && Length.HasValue) return Length.Value;

            switch (Type)
            {
                case BsonType.Null:
                case BsonType.MinValue:
                case BsonType.MaxValue:
                    Length = 0; break;

                case BsonType.Int32: Length = 4; break;
                case BsonType.Int64: Length = 8; break;
                case BsonType.Double: Length = 8; break;
                case BsonType.Decimal: Length = 16; break;

                case BsonType.String: Length = Encoding.UTF8.GetByteCount((string)RawValue); break;

                case BsonType.Binary: Length = ((Byte[])RawValue).Length; break;
                case BsonType.ObjectId: Length = 12; break;
                case BsonType.Guid: Length = 16; break;

                case BsonType.Boolean: Length = 1; break;
                case BsonType.DateTime: Length = 8; break;

                // for Array/Document calculate from elements
                case BsonType.Array:
                    var array = (List<BsonValue>)RawValue;
                    Length = 5; // header + footer
                    for (var i = 0; i < array.Count; i++)
                    {
                        Length += GetBytesCountElement(i.ToString(), array[i] ?? Null, recalc);
                    }
                    break;

                case BsonType.Document:
                    var doc = (Dictionary<string, BsonValue>)RawValue;
                    Length = 5; // header + footer
                    foreach (var key in doc.Keys)
                    {
                        Length += GetBytesCountElement(key, doc[key] ?? Null, recalc);
                    }
                    break;
            }

            return Length.Value;
        }

        private int GetBytesCountElement(string key, BsonValue value, bool recalc)
        {
            return
                1 + // element type
                Encoding.UTF8.GetByteCount(key) + // CString
                1 + // CString 0x00
                value.GetBytesCount(recalc) +
                (value.Type == BsonType.String || value.Type == BsonType.Binary || value.Type == BsonType.Guid ? 5 : 0); // bytes.Length + 0x??
        }

        #endregion
    }
}
