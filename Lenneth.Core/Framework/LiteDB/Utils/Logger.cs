using System;

namespace Lenneth.Core.Framework.LiteDB
{
    /// <summary>
    /// A logger class to log all information about database. Used with levels. Level = 0 - 255
    /// All log will be trigger before operation execute (better for debug)
    /// </summary>
    public class Logger
    {
        public const byte None = 0;
        public const byte Error = 1;
        public const byte Recovery = 2;
        public const byte Command = 4;
        public const byte Lock = 8;
        public const byte Query = 16;
        public const byte Journal = 32;
        public const byte Cache = 64;
        public const byte Disk = 128;
        public const byte Full = 255;

        /// <summary>
        /// Initialize logger class using a custom logging level (see Logger.NONE to Logger.FULL)
        /// </summary>
        public Logger(byte level = None, Action<string> logging = null)
        {
            Level = level;

            if (logging != null)
            {
                Logging += logging;
            }
        }

        /// <summary>
        /// Event when log writes a message. Fire on each log message
        /// </summary>
        public event Action<string> Logging;

        /// <summary>
        /// To full logger use Logger.FULL or any combination of Logger constants like Level = Logger.ERROR | Logger.COMMAND | Logger.DISK
        /// </summary>
        public byte Level { get; set; }

        public Logger()
        {
            Level = None;
        }

        /// <summary>
        /// Execute msg function only if level are enabled
        /// </summary>
        public void Write(byte level, Func<string> fn)
        {
            if ((level & Level) == 0) return;

            Write(level, fn());
        }

        /// <summary>
        /// Write log text to output using inside a component (statics const of Logger)
        /// </summary>
        public void Write(byte level, string message, params object[] args)
        {
            if ((level & Level) == 0 || string.IsNullOrEmpty(message)) return;

            if (Logging != null)
            {
                var text = string.Format(message, args);

                var str =
                    level == Error ? "ERROR" :
                    level == Recovery ? "RECOVERY" :
                    level == Command ? "COMMAND" :
                    level == Journal ? "JOURNAL" :
                    level == Lock ? "LOCK" :
                    level == Query ? "QUERY" :
                    level == Cache ? "CACHE" : 
                    level == Disk ? "DISK" : "";

                var msg = DateTime.Now.ToString("HH:mm:ss.ffff") + " [" + str + "] " + text;

                try
                {
                    Logging(msg);
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}