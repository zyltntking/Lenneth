using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Lenneth.Core.Framework.Http.Client.Abstractions;
using Lenneth.Core.Framework.Http.Client.Injection;
using Lenneth.Core.Framework.Http.Codecs;
using Lenneth.Core.Framework.Http.Configuration;
using Lenneth.Core.Framework.Http.Infrastructure;

namespace Lenneth.Core.Framework.Http.Client
{
    public sealed class HttpClient
    {
        private readonly string _baseUri;
        private readonly IEncoder _encoder;
        private readonly IDecoder _decoder;
        private readonly UriComposer _uriComposer;
        private bool _shouldRemoveAtSign = true;

        public bool LoggingEnabled { get; set; }
        private bool ThrowExceptionOnHttpError { get; set; }
        private bool StreamResponse { get; set; }

        public bool ShouldRemoveAtSign
        {
            get => _shouldRemoveAtSign;
            set
            {
                _shouldRemoveAtSign = value;
                _decoder.ShouldRemoveAtSign = value;
            }
        }

        private IList<HttpRequestInterception> RegisteredInterceptions { get; }

        public HttpClient() : this(new DefaultEncoderDecoderConfiguration())
        {
        }

        private HttpClient(IEncoderDecoderConfiguration encoderDecoderConfiguration)
        {
            _encoder = encoderDecoderConfiguration.GetEncoder();
            _decoder = encoderDecoderConfiguration.GetDecoder();
            _decoder.ShouldRemoveAtSign = _shouldRemoveAtSign;
            _uriComposer = new UriComposer();

            Request = new HttpRequest(_encoder);

            RegisteredInterceptions = new List<HttpRequestInterception>();
        }

        public HttpClient(string baseUri, Func<string, HttpResponse> getResponse = null) : this(new DefaultEncoderDecoderConfiguration())
        {
            _baseUri = baseUri;
        }

        private HttpResponse Response { get; set; }
        public HttpRequest Request { get; }

        private void InitRequest(string uri, HttpMethod method, object query)
        {
            Request.Uri = _uriComposer.Compose(_baseUri, uri, query, Request.ParametersAsSegments);
            Request.Data = null;
            Request.PutFilename = string.Empty;
            Request.Expect = false;
            Request.KeepAlive = true;
            Request.MultiPartFormData = null;
            Request.MultiPartFileData = null;
            Request.ContentEncoding = null;
            Request.Method = method;
        }

        public HttpResponse GetAsFile(string uri, string filename)
        {
            InitRequest(uri, HttpMethod.GET, null);
            return ProcessRequest(filename);
        }

        public HttpResponse Get(string uri, object query = null)
        {
            InitRequest(uri, HttpMethod.GET, query);
            return ProcessRequest();
        }

        public HttpResponse Options(string uri)
        {
            InitRequest(uri, HttpMethod.OPTIONS, null);
            return ProcessRequest();
        }

        public HttpResponse Post(string uri, object data, string contentType, object query = null)
        {
            InitRequest(uri, HttpMethod.POST, query);
            InitData(data, contentType);
            return ProcessRequest();
        }

        public HttpResponse Patch(string uri, object data, string contentType, object query = null)
        {
            InitRequest(uri, HttpMethod.PATCH, query);
            InitData(data, contentType);
            return ProcessRequest();
        }

        public HttpResponse Post(string uri, IDictionary<string, object> formData, IList<FileData> files, object query = null)
        {
            InitRequest(uri, HttpMethod.POST, query);
            Request.MultiPartFormData = formData;
            Request.MultiPartFileData = files;
            Request.KeepAlive = true;
            return ProcessRequest();
        }

        public HttpResponse Put(string uri, object data, string contentType, object query = null)
        {
            InitRequest(uri, HttpMethod.PUT, query);
            InitData(data, contentType);
            return ProcessRequest();
        }

        private void InitData(object data, string contentType)
        {
            if (data == null) return;
            Request.ContentType = contentType;
            Request.Data = data;
        }

        public HttpResponse Delete(string uri, object query = null)
        {
            InitRequest(uri, HttpMethod.DELETE, query);
            return ProcessRequest();
        }

        public HttpResponse Head(string uri, object query = null)
        {
            InitRequest(uri, HttpMethod.HEAD, query);
            return ProcessRequest();
        }

        public HttpResponse PutFile(string uri, string filename, string contentType)
        {
            InitRequest(uri, HttpMethod.PUT, null);
            Request.ContentType = contentType;
            Request.PutFilename = filename;
            Request.Expect = true;
            Request.KeepAlive = true;
            return ProcessRequest();
        }

        private HttpResponse ProcessRequest(string filename = "")
        {
            var matchingInterceptor = RegisteredInterceptions.FirstOrDefault(i => i.Matches(Request));

            var httpWebRequest = matchingInterceptor != null
                ? new StubbedHttpWebRequest(matchingInterceptor)
                : Request.PrepareRequest();

            var response = new HttpResponse(_decoder);

            response.GetResponse(httpWebRequest, filename, StreamResponse);

            Response = response;

            if (ThrowExceptionOnHttpError && IsHttpError())
            {
                throw new HttpException(Response.StatusCode, Response.StatusDescription);
            }
            return Response;
        }

        public void AddClientCertificates(X509CertificateCollection certificates)
        {
            if (certificates == null || certificates.Count == 0)
                return;

            Request.ClientCertificates.AddRange(certificates);
        }

        private bool IsHttpError()
        {
            var num = (int)Response.StatusCode / 100;

            return num == 4 || num == 5;
        }

        public IHttpRequestInterceptionBuilder OnRequest(Func<HttpRequest, bool> requestPredicate = null)
        {
            var interceptor = new HttpRequestInterception(requestPredicate);

            RegisteredInterceptions.Add(interceptor);

            return interceptor; // so the caller can customize it
        }

        public IHttpRequestInterceptionBuilder OnRequest(HttpMethod method, string url = null)
        {
            var interceptor = new HttpRequestInterception(method, url);

            RegisteredInterceptions.Add(interceptor);

            return interceptor; // so the caller can customize it
        }
    }
}