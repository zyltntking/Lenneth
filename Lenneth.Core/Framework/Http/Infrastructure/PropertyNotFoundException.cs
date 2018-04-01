using System;
using System.Runtime.Serialization;

namespace Lenneth.Core.Framework.Http.Infrastructure
{
    [Serializable]
    public class PropertyNotFoundException : Exception
    {
        private string PropertyName { get; }

        public PropertyNotFoundException()
        {
        }

        public PropertyNotFoundException(string propertyName) : base(propertyName)
        {
            PropertyName = propertyName;
        }

        public PropertyNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PropertyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}