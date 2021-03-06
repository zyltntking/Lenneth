﻿namespace Lenneth.WebApi.Models
{
    /// <summary>
    /// 结果内容
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultContent<T>
    {
        /// <summary>
        /// 消息码
        /// </summary>
        public int Code { get; set; } = MessageCode.Success.Code;

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; } = MessageCode.Success.Message;

        /// <summary>
        /// 内容数据
        /// </summary>
        public T Data { get; set; }
    }
}