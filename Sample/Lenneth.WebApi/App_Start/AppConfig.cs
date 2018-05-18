using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using Lenneth.WebApi.Core.Redis;

namespace Lenneth.WebApi
{
    /// <summary>
    /// 全局配置
    /// </summary>
    internal static class AppConfig
    {

        #region Redis配置

        /// <summary>
        /// Redis配置实例
        /// </summary>
        private static IRedisConnection _redisConfig;

        /// <summary>
        /// Redis配置缓存
        /// </summary>
        //public static IRedisConnection RedisConfig => _redisConfig ?? (_redisConfig = new RedisConnection("127.0.0.1", 6379));
        public static IRedisConnection RedisConfig => _redisConfig ?? (_redisConfig = new RedisConnection("127.0.0.1", 6379, "Other001", "1"));

        #endregion

        #region Nlog配置

        /// <summary>
        /// Api访问日志配置
        /// </summary>
        public static LoggingConfiguration WebApiRequestLogConfig
        {
            get
            {
                var config = new LoggingConfiguration();

                var fileTarget = new FileTarget();
                config.AddTarget(Resources.AppResource.WebApiRequestLogName, fileTarget);
                fileTarget.FileName = @"${basedir}/log/request/" + $"{DateTime.Now:yyyy-MM-dd}.txt";
                fileTarget.Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss.fff}  |${logger}  |${level:format=Name}  |${aspnet-Request-Method}    |${message}";
                config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, fileTarget));

                return config;
            }
        }

        /// <summary>
        /// Api异常日志配置
        /// </summary>
        public static LoggingConfiguration WebApiExceptionLogConfig
        {
            get
            {
                var config = new LoggingConfiguration();

                var fileTarget = new FileTarget();
                config.AddTarget(Resources.AppResource.WebApiExceptionLogName, fileTarget);
                fileTarget.FileName = @"${basedir}/log/exception/" + $"{ DateTime.Now:yyyy-MM-dd}/" + "${exception:format=ShortType}/" + $"{DateTime.Now:HH}.txt";
                fileTarget.Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss.fff}  |${logger}  |${level:format=Name}  |${message}  |${exception:format=toString,Data:maxInnerExceptionLevel=10}";
                config.LoggingRules.Add(new LoggingRule("*", LogLevel.Error, fileTarget));

                return config;
            }
        }

        /// <summary>
        /// Redis日志配置
        /// </summary>
        public static LoggingConfiguration RedisLogConfig
        {
            get
            {
                var config = new LoggingConfiguration();

                var connectionInfoTarget = new FileTarget();
                config.AddTarget("RedisConnectionInfo", connectionInfoTarget);
                connectionInfoTarget.FileName = @"${basedir}/log/redis/connectioninfo/" + $"{ DateTime.Now:yyyy-MM-dd}.txt";
                connectionInfoTarget.Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss.fff}  |${logger}  |${level:format=Name}  |${message}";
                config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, connectionInfoTarget));

                var connectionExceptionTarget = new FileTarget();
                config.AddTarget("RedisConnectionException", connectionExceptionTarget);
                connectionExceptionTarget.FileName = @"${basedir}/log/redis/exception/" + $"{ DateTime.Now:yyyy-MM-dd}.txt";
                connectionExceptionTarget.Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss.fff}  |${logger}  |${level:format=Name}  |${message}   |${exception:format=toString,Data:maxInnerExceptionLevel=10}";
                config.LoggingRules.Add(new LoggingRule("*", LogLevel.Error, connectionExceptionTarget));

                return config;
            }
        }

        #endregion Nlog配置
    }
}