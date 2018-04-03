using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Lenneth.Core.Framework.Http.Infrastructure;

namespace Lenneth.Core.Framework.Http.Client
{
    public class MultiPartStreamer
    {
        private readonly string _boundary;
        private readonly string _boundaryCode;
        private readonly IList<FileData> _multipartFileData;
        private readonly IDictionary<string, object> _multipartFormData;

        public MultiPartStreamer(IDictionary<string, object> multipartFormData, IList<FileData> multipartFileData)
        {
            _boundaryCode = DateTime.Now.Ticks.GetHashCode() + "548130";
            _boundary = $"\r\n----------------{_boundaryCode}";

            _multipartFormData = multipartFormData;
            _multipartFileData = multipartFileData;
        }

        public void StreamMultiPart(Stream stream)
        {
            stream.WriteString(_boundary);

            if (_multipartFormData != null)
            {
                foreach (var entry in _multipartFormData)
                {
                    stream.WriteString(CreateFormBoundaryHeader(entry.Key, entry.Value));
                    stream.WriteString(_boundary);
                }
            }

            if (_multipartFileData != null)
            {
                foreach (var fileData in _multipartFileData)
                {
                    using (var file = new FileStream(fileData.Filename, FileMode.Open))
                    {
                        stream.WriteString(CreateFileBoundaryHeader(fileData));

                        StreamFileContents(file, fileData, stream);

                        stream.WriteString(_boundary);
                    }
                }
            }
            stream.WriteString("--");
        }

        private static void StreamFileContents(Stream file, FileData fileData, Stream requestStream)
        {
            var buffer = new byte[8192];

            int count;

            while ((count = file.Read(buffer, 0, buffer.Length)) > 0)
            {
                if (fileData.ContentTransferEncoding == HttpContentTransferEncoding.Base64)
                {
                    var str = Convert.ToBase64String(buffer, 0, count);

                    requestStream.WriteString(str);
                }
                else if (fileData.ContentTransferEncoding == HttpContentTransferEncoding.Binary)
                {
                    requestStream.Write(buffer, 0, count);
                }
            }
        }

        public string GetContentType()
        {
            return $"multipart/form-data; boundary=--------------{_boundaryCode}";
        }

        public long GetContentLength()
        {
            var ascii = new ASCIIEncoding();
            long contentLength = ascii.GetBytes(_boundary).Length;

            // Multipart Form
            if (_multipartFormData != null)
            {
                foreach (var entry in _multipartFormData)
                {
                    contentLength += ascii.GetBytes(CreateFormBoundaryHeader(entry.Key, entry.Value)).Length; // header
                    contentLength += ascii.GetBytes(_boundary).Length;
                }
            }

            if (_multipartFileData != null)
            {
                foreach (var fileData in _multipartFileData)
                {
                    contentLength += ascii.GetBytes(CreateFileBoundaryHeader(fileData)).Length;
                    contentLength += new FileInfo(fileData.Filename).Length;
                    contentLength += ascii.GetBytes(_boundary).Length;
                }
            }

            contentLength += ascii.GetBytes("--").Length; // ending -- to the boundary

            return contentLength;
        }

        private static string CreateFileBoundaryHeader(FileData fileData)
        {
            return $"\r\nContent-Disposition: form-data; name=\"{fileData.FieldName}\"; filename=\"{Path.GetFileName(fileData.Filename)}\"\r\n" +
                $"Content-Type: {fileData.ContentType}\r\n" +
                $"Content-Transfer-Encoding: {fileData.ContentTransferEncoding}\r\n\r\n";
        }

        private static string CreateFormBoundaryHeader(string name, object value)
        {
            return $"\r\nContent-Disposition: form-data; name=\"{name}\"\r\n\r\n{value}";
        }
    }
}