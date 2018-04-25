using NLog;
using NLog.Config;
using NLog.Targets;
using System;

namespace Lenneth.WebApi
{
    /// <summary>
    /// 全局配置
    /// </summary>
    internal class AppConfig
    {
        #region Nlog配置

        /// <summary>
        /// 基本Log配置
        /// </summary>
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

        public static LoggingConfiguration WebApiRequestLogConfig
        {
            get
            {
                var config = new LoggingConfiguration();

                var fileTarget = new FileTarget();
                config.AddTarget("file", fileTarget);
                fileTarget.FileName = @"${basedir}/log/request/" + $"{DateTime.Now:yyyy-M-d}.txt";
                fileTarget.Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss.fff}  |${logger}  |${level:format=Name}  |${aspnet-Request-Method}    |${message}";
                config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, fileTarget));

                return config;
            }
        }

        #endregion Nlog配置
    }
}