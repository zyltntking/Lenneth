using System;
using System.IO;
using System.Net;
using System.Runtime.Remoting;
using System.Runtime.Serialization;

namespace Lenneth.Core.Framework.Http.Client.Abstractions
{
    internal class WebResponseWrapper : IWebResponse
    {
        private readonly WebResponse _innerRespose;

        public WebResponseWrapper(WebResponse innerRespose) => _innerRespose = innerRespose;

        public void Close() => _innerRespose.Close();

        public bool IsFromCache => _innerRespose.IsFromCache;
        public bool IsMutuallyAuthenticated => _innerRespose.IsMutuallyAuthenticated;

        public long ContentLength
        {
            get => _innerRespose.ContentLength;
            set => _innerRespose.ContentLength = value;
        }

        public string ContentType
        {
            get => _innerRespose.ContentType;
            set => _innerRespose.ContentType = value;
        }

        public Uri ResponseUri => _innerRespose.ResponseUri;
        public WebHeaderCollection Headers => _innerRespose.Headers;
        public bool SupportsHeaders => false;

        public Stream GetResponseStream()
        {
            return _innerRespose.GetResponseStream();
        }

        public object GetLifetimeService()
        {
            return _innerRespose.GetLifetimeService();
        }

        public object InitializeLifetimeService()
        {
            return _innerRespose.InitializeLifetimeService();
        }

        public ObjRef CreateObjRef(Type requestedType)
        {
            return _innerRespose.CreateObjRef(requestedType);
        }

        public void Dispose()
        {
            (_innerRespose as IDisposable).Dispose();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            (_innerRespose as ISerializable).GetObjectData(info, context);
        }
    }
}