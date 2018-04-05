using System;
using System.IO;
using System.Net;
using System.Runtime.Remoting;
using System.Runtime.Serialization;

namespace Lenneth.Core.Framework.Http.Client.Abstractions
{
    internal class HttpWebResponseWrapper : IWebResponse, IHttpWebResponse
    {
        private readonly HttpWebResponse _innerResponse;

        public HttpWebResponseWrapper(HttpWebResponse innerResponse)
        {
            _innerResponse = innerResponse;
        }

        public HttpWebResponse InnerResponse => _innerResponse;
        public bool IsMutuallyAuthenticated => _innerResponse.IsMutuallyAuthenticated;
        long IWebResponse.ContentLength { get; set; }
        string IWebResponse.ContentType { get; set; }

        public CookieCollection Cookies
        {
            get => _innerResponse.Cookies;
            set => _innerResponse.Cookies = value;
        }

        public WebHeaderCollection Headers => _innerResponse.Headers;
        public bool SupportsHeaders => true;
        public long ContentLength => _innerResponse.ContentLength;
        public string ContentEncoding => _innerResponse.ContentEncoding;
        public string ContentType => _innerResponse.ContentType;
        public string CharacterSet => _innerResponse.CharacterSet;
        public string Server => _innerResponse.Server;
        public DateTime LastModified => _innerResponse.LastModified;
        public HttpStatusCode StatusCode => _innerResponse.StatusCode;
        public string StatusDescription => _innerResponse.StatusDescription;
        public Version ProtocolVersion => _innerResponse.ProtocolVersion;
        public Uri ResponseUri => _innerResponse.ResponseUri;
        public string Method => _innerResponse.Method;
        public bool IsFromCache => _innerResponse.IsFromCache;

        public Stream GetResponseStream() => _innerResponse.GetResponseStream();

        public void Close() => _innerResponse.Close();

        public string GetResponseHeader(string headerName) => _innerResponse.GetResponseHeader(headerName);

        public object GetLifetimeService() => _innerResponse.GetLifetimeService();

        public object InitializeLifetimeService() => _innerResponse.InitializeLifetimeService();

        public ObjRef CreateObjRef(Type requestedType) => _innerResponse.CreateObjRef(requestedType);

        public void GetObjectData(SerializationInfo info, StreamingContext context) => (_innerResponse as ISerializable).GetObjectData(info, context);

        public void Dispose() => (_innerResponse as IDisposable).Dispose();
    }
}