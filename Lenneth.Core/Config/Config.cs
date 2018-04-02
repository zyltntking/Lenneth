using Lenneth.Core.Framework.Redis;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;

namespace Lenneth.Core
{
    using Framework.Mail;

    internal static class Config
    {
        #region Redis配置

        private static readonly object RedisLocker = new object();

        /// <summary>
        /// connection实例
        /// </summary>
        private static IRedisConnection _redisConnection;

        /// <summary>
        /// Redis缓存配置
        /// </summary>
        public static IRedisConnection RedisConnection
        {
            get
            {
                if (_redisConnection == null)
                {
                    lock (RedisLocker)
                    {
                        if (_redisConnection == null)
                        {
                            _redisConnection = new RedisConnection();
                            _redisConnection.InitConnection(host: "127.0.0.1", port: 6379, prefix: "lenneth", collection: "0");
                        }
                    }
                }
                return _redisConnection;
            }
        }

        #endregion Redis配置

        #region Mail配置

        private static readonly object MailLocker = new object();

        private static MailConfig _mailConfig;

        public static MailConfig MailConfig
        {
            get
            {
                if (_mailConfig == null)
                {
                    lock (MailLocker)
                    {
                        if (_mailConfig == null)
                        {
                            _mailConfig = new MailConfig
                            {
                                MailHost = "smtp.qq.com",
                                MailPort = 587,
                                MailAddress = "zyltntking@qq.com",
                                MailPassword = "nggxargxcoxgfhhh",
                                MailSign = "Lenneth"
                            };
                        }
                    }
                }
                return _mailConfig;
            }
        }

        #endregion Mail配置

        #region Nlog配置

        public static LoggingConfiguration LogConfig
        {
            get
            {
                var config = new LoggingConfiguration();

                var consoleTarget = new ColoredConsoleTarget();
                config.AddTarget("console", consoleTarget);
                consoleTarget.Layout = @"${date:format=HH\:mm\:ss} ${logger} ${message}";
                config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));

                var fileTarget = new FileTarget();
                config.AddTarget("file", fileTarget);
                fileTarget.FileName = @"${basedir}/log/" + $"{DateTime.Now:yyyy-M-d}.txt";
                fileTarget.Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss.fff}  |${logger}  |${level:format=Name}  |${message}  |${exception:format=toString,Data}";
                config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));

                return config;
            }
        }

        #endregion Nlog配置
    }
}