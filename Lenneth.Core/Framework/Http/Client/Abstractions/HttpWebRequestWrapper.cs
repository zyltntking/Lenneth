using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Runtime.Remoting;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace Lenneth.Core.Framework.Http.Client.Abstractions
{
    internal class HttpWebRequestWrapper : IHttpWebRequest
    {
        private readonly HttpWebRequest _innerRequest;

        public HttpWebRequestWrapper(HttpWebRequest innerRequest) => _innerRequest = innerRequest;

        #region Properties

        public HttpWebRequest InnerRequest => _innerRequest;

        public bool AllowAutoRedirect
        {
            get => _innerRequest.AllowAutoRedirect;
            set => _innerRequest.AllowAutoRedirect = value;
        }

        public bool AllowWriteStreamBuffering
        {
            get => _innerRequest.AllowWriteStreamBuffering;
            set => _innerRequest.AllowWriteStreamBuffering = value;
        }

        public bool HaveResponse => _innerRequest.HaveResponse;

        public bool KeepAlive
        {
            get => _innerRequest.KeepAlive;
            set => _innerRequest.KeepAlive = value;
        }

        public bool Pipelined
        {
            get => _innerRequest.Pipelined;
            set => _innerRequest.Pipelined = value;
        }

        public bool PreAuthenticate
        {
            get => _innerRequest.PreAuthenticate;
            set => _innerRequest.PreAuthenticate = value;
        }

        public bool UnsafeAuthenticatedConnectionSharing
        {
            get => _innerRequest.UnsafeAuthenticatedConnectionSharing;
            set => _innerRequest.UnsafeAuthenticatedConnectionSharing = value;
        }

        public bool SendChunked
        {
            get => _innerRequest.SendChunked;
            set => _innerRequest.SendChunked = value;
        }

        public DecompressionMethods AutomaticDecompression
        {
            get => _innerRequest.AutomaticDecompression;
            set => _innerRequest.AutomaticDecompression = value;
        }

        public int MaximumResponseHeadersLength
        {
            get => _innerRequest.MaximumResponseHeadersLength;
            set => _innerRequest.MaximumResponseHeadersLength = value;
        }

        public X509CertificateCollection ClientCertificates
        {
            get => _innerRequest.ClientCertificates;
            set => _innerRequest.ClientCertificates = value;
        }

        public CookieContainer CookieContainer
        {
            get => _innerRequest.CookieContainer;
            set => _innerRequest.CookieContainer = value;
        }

        public bool SupportsCookieContainer => true;
        public Uri RequestUri => _innerRequest.RequestUri;

        public long ContentLength
        {
            get => _innerRequest.ContentLength;
            set => _innerRequest.ContentLength = value;
        }

        public int Timeout
        {
            get => _innerRequest.Timeout;
            set => _innerRequest.Timeout = value;
        }

        public int ReadWriteTimeout
        {
            get => _innerRequest.ReadWriteTimeout;
            set => _innerRequest.ReadWriteTimeout = value;
        }

        public Uri Address => _innerRequest.Address;

        public HttpContinueDelegate ContinueDelegate
        {
            get => _innerRequest.ContinueDelegate;
            set => _innerRequest.ContinueDelegate = value;
        }

        public ServicePoint ServicePoint => _innerRequest.ServicePoint;

        public string Host
        {
            get => _innerRequest.Host;
            set => _innerRequest.Host = value;
        }

        public int MaximumAutomaticRedirections
        {
            get => _innerRequest.MaximumAutomaticRedirections;
            set => _innerRequest.MaximumAutomaticRedirections = value;
        }

        public string Method
        {
            get => _innerRequest.Method;
            set => _innerRequest.Method = value;
        }

        public ICredentials Credentials
        {
            get => _innerRequest.Credentials;
            set => _innerRequest.Credentials = value;
        }

        public bool UseDefaultCredentials
        {
            get => _innerRequest.UseDefaultCredentials;
            set => _innerRequest.UseDefaultCredentials = value;
        }

        public string ConnectionGroupName
        {
            get => _innerRequest.ConnectionGroupName;
            set => _innerRequest.ConnectionGroupName = value;
        }

        public WebHeaderCollection Headers
        {
            get => _innerRequest.Headers;
            set => _innerRequest.Headers = value;
        }

        public IWebProxy Proxy
        {
            get => _innerRequest.Proxy;
            set => _innerRequest.Proxy = value;
        }

        public Version ProtocolVersion
        {
            get => _innerRequest.ProtocolVersion;
            set => _innerRequest.ProtocolVersion = value;
        }

        public string ContentType
        {
            get => _innerRequest.ContentType;
            set => _innerRequest.ContentType = value;
        }

        public string MediaType
        {
            get => _innerRequest.MediaType;
            set => _innerRequest.MediaType = value;
        }

        public string TransferEncoding
        {
            get => _innerRequest.TransferEncoding;
            set => _innerRequest.TransferEncoding = value;
        }

        public string Connection
        {
            get => _innerRequest.Connection;
            set => _innerRequest.Connection = value;
        }

        public string Accept
        {
            get => _innerRequest.Accept;
            set => _innerRequest.Accept = value;
        }

        public string Referer
        {
            get => _innerRequest.Referer;
            set => _innerRequest.Referer = value;
        }

        public string UserAgent
        {
            get => _innerRequest.UserAgent;
            set => _innerRequest.UserAgent = value;
        }

        public string Expect
        {
            get => _innerRequest.Expect;
            set => _innerRequest.Expect = value;
        }

        public DateTime IfModifiedSince
        {
            get => _innerRequest.IfModifiedSince;
            set => _innerRequest.IfModifiedSince = value;
        }

        public DateTime Date
        {
            get => _innerRequest.Date;
            set => _innerRequest.Date = value;
        }

        public RequestCachePolicy CachePolicy
        {
            get => _innerRequest.CachePolicy;
            set => _innerRequest.CachePolicy = value;
        }

        public AuthenticationLevel AuthenticationLevel
        {
            get => _innerRequest.AuthenticationLevel;
            set => _innerRequest.AuthenticationLevel = value;
        }

        public TokenImpersonationLevel ImpersonationLevel
        {
            get => _innerRequest.ImpersonationLevel;
            set => _innerRequest.ImpersonationLevel = value;
        }

        #endregion Properties

        #region Methods

        public IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state) => _innerRequest.BeginGetRequestStream(callback, state);

        public Stream EndGetRequestStream(IAsyncResult asyncResult) => _innerRequest.EndGetRequestStream(asyncResult);

        public Stream EndGetRequestStream(IAsyncResult asyncResult, out TransportContext context) => _innerRequest.EndGetRequestStream(asyncResult, out context);

        public Stream GetRequestStream() => _innerRequest.GetRequestStream();

        public Stream GetRequestStream(out TransportContext context) => _innerRequest.GetRequestStream(out context);

        public IAsyncResult BeginGetResponse(AsyncCallback callback, object state) => _innerRequest.BeginGetResponse(callback, state);

        public IHttpWebResponse EndGetResponse(IAsyncResult asyncResult) => new HttpWebResponseWrapper((HttpWebResponse)_innerRequest.EndGetResponse(asyncResult));

        public IHttpWebResponse GetResponse() => new HttpWebResponseWrapper((HttpWebResponse)_innerRequest.GetResponse());

        public void Abort() => _innerRequest.Abort();

        public void AddRange(int from, int to) => _innerRequest.AddRange(@from, to);

        public void AddRange(long from, long to) => _innerRequest.AddRange(@from, to);

        public void AddRange(int range) => _innerRequest.AddRange(range);

        public void AddRange(long range) => _innerRequest.AddRange(range);

        public void AddRange(string rangeSpecifier, int from, int to) => _innerRequest.AddRange(rangeSpecifier, @from, to);

        public void AddRange(string rangeSpecifier, long from, long to) => _innerRequest.AddRange(rangeSpecifier, @from, to);

        public void AddRange(string rangeSpecifier, int range) => _innerRequest.AddRange(rangeSpecifier, range);

        public void AddRange(string rangeSpecifier, long range) => _innerRequest.AddRange(rangeSpecifier, range);

        public object GetLifetimeService() => _innerRequest.GetLifetimeService();

        public object InitializeLifetimeService() => _innerRequest.InitializeLifetimeService();

        public ObjRef CreateObjRef(Type requestedType) => _innerRequest.CreateObjRef(requestedType);

        #endregion Methods
    }
}