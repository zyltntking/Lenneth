using System;

namespace Lenneth.Core.Framework.LiteDB
{
    /// <summary>
    /// Indicate that property will not be persist in Bson serialization
    /// </summary>
    public class BsonIgnoreAttribute : Attribute
    {
    }
}