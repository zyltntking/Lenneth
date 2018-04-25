using System;

namespace Lenneth.WebApi.Core.Log
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILogWapper
    {
        /// <summary>
        /// 日志名
        /// </summary>
        string LogName { get; set; }

        #region Method

        /// <summary>
        /// 记录Debug级日志
        /// </summary>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        void Debug(string msg, params object[] args);

        /// <summary>
        /// 记录Debug级日志
        /// </summary>
        /// <param name="err">程序异常</param>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        void Debug(Exception err,string msg,params object[] args);

        /// <summary>
        /// 记录Info级日志
        /// </summary>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        void Info(string msg, params object[] args);

        /// <summary>
        /// 记录Info级日志
        /// </summary>
        /// <param name="err">程序异常</param>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        void Info(Exception err, string msg, params object[] args);

        /// <summary>
        /// 记录Trace级日志
        /// </summary>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        void Trace(string msg, params object[] args);

        /// <summary>
        /// 记录Trace级日志
        /// </summary>
        /// <param name="err">程序异常</param>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        void Trace(Exception err, string msg, params object[] args);

        /// <summary>
        /// 记录Fatal级日志
        /// </summary>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        void Fatal(string msg, params object[] args);

        /// <summary>
        /// 记录Fatal级日志
        /// </summary>
        /// <param name="err">程序异常</param>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        void Fatal(Exception err, string msg, params object[] args);

        /// <summary>
        /// 记录Error级日志
        /// </summary>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        void Error(string msg, params object[] args);

        /// <summary>
        /// 记录Error级日志
        /// </summary>
        /// <param name="err">程序异常</param>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        void Error(Exception err, string msg, params object[] args);

        /// <summary>
        /// 记录Warn级日志
        /// </summary>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        void Warn(string msg, params object[] args);

        /// <summary>
        /// 记录Warn级日志
        /// </summary>
        /// <param name="err">程序异常</param>
        /// <param name="msg">格式化消息</param>
        /// <param name="args">格式参数</param>
        void Warn(Exception err, string msg, params object[] args);

        #endregion
    }
}