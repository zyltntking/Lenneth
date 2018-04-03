using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Lenneth.Core.Framework.LiteDB
{
    /// <summary>
    /// Represents a file inside storage collection
    /// </summary>
    public class LiteFileInfo
    {
        /// <summary>
        /// File id have a specific format - it's like file path.
        /// </summary>
        public const string ID_PATTERN = @"^[\w-$@!+%;\.]+(\/[\w-$@!+%;\.]+)*$";

        private static Regex IdPattern = new Regex(ID_PATTERN, RegexOptions.Compiled);

        public string Id { get; private set; }
        public string Filename { get; private set; }
        public string MimeType { get; private set; }
        public long Length { get; internal set; }
        public int Chunks { get; internal set; }
        public DateTime UploadDate { get; internal set; }
        public BsonDocument Metadata { get; set; }

        private LiteEngine _engine;

        internal LiteFileInfo(LiteEngine engine, string id, string filename)
        {
            if (!IdPattern.IsMatch(id)) throw LiteException.InvalidFormat(id);

            _engine = engine;

            Id = id;
            Filename = Path.GetFileName(filename);
            MimeType = MimeTypeConverter.GetMimeType(Filename);
            Length = 0;
            Chunks = 0;
            UploadDate = DateTime.Now;
            Metadata = new BsonDocument();
        }

        internal LiteFileInfo(LiteEngine engine, BsonDocument doc)
        {
            _engine = engine;

            Id = doc["_id"].AsString;
            Filename = doc["filename"].AsString;
            MimeType = doc["mimeType"].AsString;
            Length = doc["length"].AsInt64;
            Chunks = doc["chunks"].AsInt32;
            UploadDate = doc["uploadDate"].AsDateTime;
            Metadata = doc["metadata"].AsDocument;
        }

        public BsonDocument AsDocument
        {
            get
            {
                return new BsonDocument
                {
                    { "_id", Id },
                    { "filename", Filename },
                    { "mimeType", MimeType },
                    { "length", Length },
                    { "chunks", Chunks },
                    { "uploadDate", UploadDate },
                    { "metadata", Metadata ?? new BsonDocument() }
                };
            }
        }

        /// <summary>
        /// Open file stream to read from database
        /// </summary>
        public LiteFileStream OpenRead()
        {
            return new LiteFileStream(_engine, this, FileAccess.Read);
        }

        /// <summary>
        /// Open file stream to write to database
        /// </summary>
        public LiteFileStream OpenWrite()
        {
            return new LiteFileStream(_engine, this, FileAccess.Write);
        }

        /// <summary>
        /// Copy file content to another stream
        /// </summary>
        public void CopyTo(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            using (var reader = OpenRead())
            {
                reader.CopyTo(stream);
            }
        }

        /// <summary>
        /// Save file content to a external file
        /// </summary>
        public void SaveAs(string filename, bool overwritten = true)
        {
            if (filename.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(filename));

            using (var file = File.Open(filename, overwritten ? System.IO.FileMode.Create : System.IO.FileMode.CreateNew))
            {
                OpenRead().CopyTo(file);
            }
        }
    }
}