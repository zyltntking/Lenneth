﻿using System.IO;

namespace Lenneth.Core.Framework.Http.Http
{
    public static class StreamExtensions
    {
        public static Stream ToStream(this string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            if (s != null)
            {
                writer.Write(s);
                writer.Flush();
            }
            stream.Position = 0;
            return stream;
        }
    }
}