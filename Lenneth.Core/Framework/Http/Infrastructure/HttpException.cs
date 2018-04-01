using System;
using System.Net;
using System.Runtime.Serialization;

namespace Lenneth.Core.Framework.Http.Infrastructure
{
    [Serializable]
    public class HttpException : Exception
    {
        private HttpStatusCode StatusCode { get; }
        private string StatusDescription { get; }

        public HttpException(HttpStatusCode statusCode, string statusDescription) : base(
            $"{statusCode} {statusDescription}")
        {
            StatusCode = statusCode;
            StatusDescription = statusDescription;
        }

        public HttpException()
        {
        }

        public HttpException(string message) : base(message)
        {
        }

        public HttpException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}