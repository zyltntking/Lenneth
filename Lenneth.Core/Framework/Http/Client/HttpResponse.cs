using Lenneth.Core.Framework.Http.Client.Abstractions;
using Lenneth.Core.Framework.Http.Codecs;
using Lenneth.Core.Framework.Http.Configuration;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Lenneth.Core.Framework.Http.Client
{
    public class HttpResponse
    {
        private readonly IDecoder _decoder;
        private IHttpWebResponse _response;

        public virtual string CharacterSet { get; private set; }
        public virtual string ContentType { get; private set; }
        public virtual HttpStatusCode StatusCode { get; private set; }
        public virtual string StatusDescription { get; private set; }
        public virtual CookieCollection Cookies { get; private set; }
        public virtual int Age { get; private set; }
        public virtual HttpMethod[] Allow { get; private set; }
        public virtual CacheControl CacheControl { get; private set; }
        public virtual string ContentEncoding { get; private set; }
        public virtual string ContentLanguage { get; private set; }
        public virtual long ContentLength { get; private set; }
        public virtual string ContentLocation { get; private set; }

        // TODO :This should be files
        public virtual string ContentDisposition { get; private set; }

        public virtual DateTime Date { get; private set; }
        public virtual string ETag { get; private set; }
        public virtual DateTime Expires { get; private set; }
        public virtual DateTime LastModified { get; private set; }
        public virtual string Location { get; private set; }
        public virtual CacheControl Pragma { get; private set; }
        public virtual string Server { get; private set; }
        public virtual WebHeaderCollection RawHeaders { get; private set; }

        public virtual Stream ResponseStream
        {
            get { return _response.GetResponseStream(); }
        }

        public virtual dynamic DynamicBody => _decoder.DecodeToDynamic(RawText, ContentType);

        public virtual string RawText { get; private set; }

        public virtual T StaticBody<T>(string overrideContentType = null)
        {
            return _decoder.DecodeToStatic<T>(RawText, overrideContentType ?? ContentType);
        }

        public HttpResponse() : this(null)
        {
        }

        public HttpResponse(IDecoder decoder)
        {
            _decoder = decoder ?? new DefaultEncoderDecoderConfiguration().GetDecoder();
        }

        public virtual void GetResponse(IHttpWebRequest request, string filename, bool streamResponse)
        {
            try
            {
                _response = request.GetResponse();
            }
            catch (WebException webException)
            {
                if (webException.Response == null)
                {
                    throw;
                }
                _response = new HttpWebResponseWrapper((HttpWebResponse)webException.Response);
            }

            GetHeaders();

            if (streamResponse) return;

            var stream = _response.GetResponseStream();
            if (stream == null) return;

            if (!string.IsNullOrEmpty(filename))
            {
                using (var filestream = new FileStream(filename, FileMode.CreateNew))
                {
                    int count;
                    var buffer = new byte[8192];

                    while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        filestream.Write(buffer, 0, count);
                    }
                }
            }
            else
            {
                var encoding = string.IsNullOrEmpty(CharacterSet) ? Encoding.UTF8 : Encoding.GetEncoding(CharacterSet);
                using (var reader = new StreamReader(stream, encoding))
                {
                    RawText = reader.ReadToEnd();
                }
            }
        }

        private void GetHeaders()
        {
            CharacterSet = _response.CharacterSet;
            ContentType = _response.ContentType;
            StatusCode = _response.StatusCode;
            StatusDescription = _response.StatusDescription;
            Cookies = _response.Cookies;
            ContentEncoding = _response.ContentEncoding;
            ContentLength = _response.ContentLength;
            Date = DateTime.Now;
            LastModified = _response.LastModified;
            Server = _response.Server;

            if (!string.IsNullOrEmpty(GetHeader("Age")))
            {
                Age = Convert.ToInt32(GetHeader("Age"));
            }

            ContentLanguage = GetHeader("Content-Language");
            ContentLocation = GetHeader("Content-Location");
            ContentDisposition = GetHeader("Content-Disposition");
            ETag = GetHeader("ETag");
            Location = GetHeader("Location");

            if (!string.IsNullOrEmpty(GetHeader("Expires")))
            {
                if (DateTime.TryParse(GetHeader("Expires"), out var expires))
                {
                    Expires = expires;
                }
            }

            // TODO: Finish this.
            //   public HttpMethod Allow { get; private set; }
            //   public CacheControl CacheControl { get; private set; }
            //   public CacheControl Pragma { get; private set; }

            RawHeaders = _response.Headers;
        }

        private string GetHeader(string header)
        {
            var headerValue = _response.GetResponseHeader(header);

            return headerValue.Replace("\"", "");
        }
    }
}