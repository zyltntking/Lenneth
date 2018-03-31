using System;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Lenneth.Core
{
    internal static class Config
    {
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

        #endregion
    }
}