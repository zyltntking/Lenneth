using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Lenneth.Core.Framework.Http.Client.Abstractions;
using Lenneth.Core.Framework.Http.Codecs;
using Lenneth.Core.Framework.Http.Infrastructure;

namespace Lenneth.Core.Framework.Http.Client
{
    // TODO: This class needs cleaning up and abstracting the encoder one more level
    public sealed class HttpRequest
    {
        private readonly IEncoder _encoder;
        private HttpRequestCachePolicy _cachePolicy;
        private bool _forceBasicAuth;
        private string _password;
        private string _username;
        private IHttpWebRequest _httpWebRequest;
        private CookieContainer _cookieContainer;

        public HttpRequest(IEncoder encoder)
        {
            RawHeaders = new Dictionary<string, object>();

            ClientCertificates = new X509CertificateCollection();

            UserAgent = $"EasyHttp HttpClient v{Assembly.GetAssembly(typeof(HttpClient)).GetName().Version}";

            Accept = string.Join(";", HttpContentTypes.TextHtml, HttpContentTypes.ApplicationXml,
                                 HttpContentTypes.ApplicationJson);
            _encoder = encoder;

            Timeout = 100000; //http://msdn.microsoft.com/en-us/library/system.net.httpwebrequest.timeout.aspx

            AllowAutoRedirect = true;
        }

        public bool DisableAutomaticCompression { get; set; }
        public string Accept { get; set; }
        public string AcceptCharSet { get; set; }
        public string AcceptEncoding { get; set; }
        public string AcceptLanguage { get; set; }
        public bool KeepAlive { get; set; }
        public X509CertificateCollection ClientCertificates { get; set; }
        public string ContentLength { get; private set; }
        public string ContentType { get; set; }
        public string ContentEncoding { get; set; }
        public CookieCollection Cookies { get; set; }
        public DateTime Date { get; set; }
        public bool Expect { get; set; }
        public string From { get; set; }
        public string Host { get; set; }
        public string IfMatch { get; set; }
        public DateTime IfModifiedSince { get; set; }
        public string IfRange { get; set; }
        public int MaxForwards { get; set; }
        public string Referer { get; set; }
        public int Range { get; set; }
        public string UserAgent { get; set; }
        public IDictionary<string, object> RawHeaders { get; private set; }
        public HttpMethod Method { get; set; }
        public object Data { get; set; }
        public string Uri { get; set; }
        public string PutFilename { get; set; }
        public IDictionary<string, object> MultiPartFormData { get; set; }
        public IList<FileData> MultiPartFileData { get; set; }
        public int Timeout { get; set; }
        public Boolean ParametersAsSegments { get; set; }

        public bool ForceBasicAuth
        {
            get => _forceBasicAuth;
            set => _forceBasicAuth = value;
        }

        public bool PersistCookies { get; set; }
        public bool AllowAutoRedirect { get; set; }

        public void SetBasicAuthentication(string username, string password)
        {
            _username = username;
            _password = password;
        }

        private void SetupHeader()
        {
            if (!PersistCookies || _cookieContainer == null)
                _cookieContainer = new CookieContainer();

            _httpWebRequest.CookieContainer = _cookieContainer;
            _httpWebRequest.ContentType = ContentType;
            _httpWebRequest.Accept = Accept;
            _httpWebRequest.Method = Method.ToString();
            _httpWebRequest.UserAgent = UserAgent;
            _httpWebRequest.Referer = Referer;
            _httpWebRequest.CachePolicy = _cachePolicy;
            _httpWebRequest.KeepAlive = KeepAlive;
            _httpWebRequest.AutomaticDecompression = DisableAutomaticCompression
                                                    ? DecompressionMethods.None
                                                    : DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.None;

            ServicePointManager.Expect100Continue = Expect;
            ServicePointManager.ServerCertificateValidationCallback = AcceptAllCertifications;

            if (Timeout > 0)
            {
                _httpWebRequest.Timeout = Timeout;
            }

            if (Cookies != null)
            {
                _httpWebRequest.CookieContainer.Add(Cookies);
            }

            if (IfModifiedSince != DateTime.MinValue)
            {
                _httpWebRequest.IfModifiedSince = IfModifiedSince;
            }

            if (Date != DateTime.MinValue)
            {
                _httpWebRequest.Date = Date;
            }

            if (!String.IsNullOrEmpty(Host))
            {
                _httpWebRequest.Host = Host;
            }

            if (MaxForwards != 0)
            {
                _httpWebRequest.MaximumAutomaticRedirections = MaxForwards;
            }

            if (Range != 0)
            {
                _httpWebRequest.AddRange(Range);
            }

            SetupAuthentication();

            AddExtraHeader("From", From);
            AddExtraHeader("Accept-CharSet", AcceptCharSet);
            AddExtraHeader("Accept-Encoding", AcceptEncoding);
            AddExtraHeader("Accept-Language", AcceptLanguage);
            AddExtraHeader("If-Match", IfMatch);
            AddExtraHeader("Content-Encoding", ContentEncoding);

            foreach (var header in RawHeaders)
            {
                _httpWebRequest.Headers.Add($@"{header.Key}: {header.Value}");
            }
        }

        private bool AcceptAllCertifications(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors) => true;

        public void AddExtraHeader(string header, object value)
        {
            if (value != null && !RawHeaders.ContainsKey(header))
            {
                RawHeaders.Add(header, value);
            }
        }

        private void SetupBody()
        {
            if (Data != null)
            {
                SetupData();

                return;
            }

            if (!string.IsNullOrEmpty(PutFilename))
            {
                SetupPutFilename();
                return;
            }

            if (MultiPartFormData != null || MultiPartFileData != null)
            {
                SetupMultiPartBody();
            }
        }

        private void SetupData()
        {
            var bytes = _encoder.Encode(Data, ContentType);

            if (bytes.Length > 0)
            {
                _httpWebRequest.ContentLength = bytes.Length;
            }

            var requestStream = _httpWebRequest.GetRequestStream();

            requestStream.Write(bytes, 0, bytes.Length);

            requestStream.Close();
        }

        private void SetupPutFilename()
        {
            using (var fileStream = new FileStream(PutFilename, FileMode.Open))
            {
                _httpWebRequest.ContentLength = fileStream.Length;

                var requestStream = _httpWebRequest.GetRequestStream();

                var buffer = new byte[81982];

                var bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                while (bytesRead > 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                    bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                }
                requestStream.Close();
            }
        }

        private void SetupMultiPartBody()
        {
            var multiPartStreamer = new MultiPartStreamer(MultiPartFormData, MultiPartFileData);

            _httpWebRequest.ContentType = multiPartStreamer.GetContentType();
            var contentLength = multiPartStreamer.GetContentLength();

            if (contentLength > 0)
            {
                _httpWebRequest.ContentLength = contentLength;
            }

            multiPartStreamer.StreamMultiPart(_httpWebRequest.GetRequestStream());
        }

        public IHttpWebRequest PrepareRequest()
        {
            _httpWebRequest =
                new HttpWebRequestWrapper((HttpWebRequest)WebRequest.Create(Uri))
                {
                    AllowAutoRedirect = AllowAutoRedirect
                };
            SetupHeader();

            SetupBody();

            return _httpWebRequest;
        }

        private void SetupClientCertificates()
        {
            if (ClientCertificates == null || ClientCertificates.Count == 0)
                return;

            _httpWebRequest.ClientCertificates.AddRange(ClientCertificates);
        }

        private void SetupAuthentication()
        {
            SetupClientCertificates();

            if (_forceBasicAuth)
            {
                var authInfo = _username + ":" + _password;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                _httpWebRequest.Headers["Authorization"] = "Basic " + authInfo;
            }
            else
            {
                var networkCredential = new NetworkCredential(_username, _password);
                _httpWebRequest.Credentials = networkCredential;
            }
        }

        public void SetCacheControlToNoCache() => _cachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);

        public void SetCacheControlWithMaxAge(TimeSpan maxAge) => _cachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, maxAge);

        public void SetCacheControlWithMaxAgeAndMaxStale(TimeSpan maxAge, TimeSpan maxStale) => _cachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAgeAndMaxStale, maxAge, maxStale);

        public void SetCacheControlWithMaxAgeAndMinFresh(TimeSpan maxAge, TimeSpan minFresh) => _cachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAgeAndMinFresh, maxAge, minFresh);
    }
}