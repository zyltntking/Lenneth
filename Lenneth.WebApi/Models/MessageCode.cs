namespace Lenneth.WebApi.Models
{
    internal static class MessageCode
    {
        #region simple

        public static readonly MessageStruct Success = new MessageStruct { Code = 0, Message = "成功" };
        public static readonly MessageStruct Fail = new MessageStruct { Code = 1, Message = "失败" };

        #endregion simple
    }

    /// <summary>
    /// 消息结构
    /// </summary>
    internal struct MessageStruct
    {
        /// <summary>
        /// 消息码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }
    }
}