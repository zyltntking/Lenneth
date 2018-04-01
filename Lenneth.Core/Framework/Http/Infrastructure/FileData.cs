namespace Lenneth.Core.Framework.Http.Infrastructure
{
    using Http;

    public abstract class FileData
    {
        public string FieldName { get; set; }
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public string ContentTransferEncoding { get; set; }

        protected FileData()
        {
            ContentTransferEncoding = HttpContentTransferEncoding.Binary;
        }
    }
}