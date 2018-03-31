using System;
using NLog;
using NLog.Config;

namespace Lenneth.Core.Framework.Log
{
    /// <summary>
    /// Nlog简单封装
    /// </summary>
    internal class NLogWrapper : ILogging
    {
        /// <summary>
        /// 当前日志组
        /// </summary>
        private string _currentName;

        /// <summary>
        /// Nlog实例
        /// </summary>
        private Logger _nlogger;

        /// <summary>
        /// 日志实例
        /// </summary>
        private Logger Nlogger
        {
            get
            {
                if (_nlogger != null && LogName.Equals(_currentName)) return _nlogger;
                _currentName = LogName;
                LogManager.Configuration = Config;
                var name = string.IsNullOrWhiteSpace(_currentName) ? "log" : _currentName;
                _nlogger = LogManager.GetLogger(name);
                return _nlogger;
            }
        }

        /// <summary>
        /// 日志配置
        /// </summary>
        private LoggingConfiguration Config { get; }

        /// <summary>
        /// 日志名
        /// </summary>
        public string LogName { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">Log配置</param>
        /// <param name="logname">日志名</param>
        public NLogWrapper(LoggingConfiguration config, string logname = null)
        {
            Config = config;
            _currentName = logname;
            LogName = _currentName;
        }

        #region Method

        /// <summary>
        /// 记录Debug级日志
        /// </summary>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        public void Debug(string msg, params object[] args) => Nlogger.Debug(msg, args);

        /// <summary>
        /// 记录Debug级日志
        /// </summary>
        /// <param name="err">程序异常</param>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        public void Debug(Exception err, string msg, params object[] args) => Nlogger.Debug(err, msg, args);

        /// <summary>
        /// 记录Info级日志
        /// </summary>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        public void Info(string msg, params object[] args) => Nlogger.Info(msg, args);

        /// <summary>
        /// 记录Info级日志
        /// </summary>
        /// <param name="err">程序异常</param>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        public void Info(Exception err, string msg, params object[] args) => Nlogger.Info(err, msg, args);

        /// <summary>
        /// 记录Trace级日志
        /// </summary>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        public void Trace(string msg, params object[] args) => Nlogger.Trace(msg, args);

        /// <summary>
        /// 记录Trace级日志
        /// </summary>
        /// <param name="err">程序异常</param>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        public void Trace(Exception err, string msg, params object[] args) => Nlogger.Trace(err, msg, args);

        /// <summary>
        /// 记录Fatal级日志
        /// </summary>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        public void Fatal(string msg, params object[] args) => Nlogger.Fatal(msg, args);

        /// <summary>
        /// 记录Fatal级日志
        /// </summary>
        /// <param name="err">程序异常</param>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        public void Fatal(Exception err, string msg, params object[] args) => Nlogger.Fatal(err, msg, args);

        /// <summary>
        /// 记录Error级日志
        /// </summary>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        public void Error(string msg, params object[] args) => Nlogger.Error(msg, args);

        /// <summary>
        /// 记录Error级日志
        /// </summary>
        /// <param name="err">程序异常</param>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        public void Error(Exception err, string msg, params object[] args) => Nlogger.Error(err, msg, args);

        /// <summary>
        /// 记录Warn级日志
        /// </summary>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        public void Warn(string msg, params object[] args) => Nlogger.Warn(msg, args);

        /// <summary>
        /// 记录Warn级日志
        /// </summary>
        /// <param name="err">程序异常</param>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        public void Warn(Exception err, string msg, params object[] args) => Nlogger.Warn(err, msg, args);

        #endregion Method
    }
}