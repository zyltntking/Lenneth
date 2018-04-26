namespace Lenneth.WebApi.Models
{
    internal static class MessageCode
    {
        #region simple

        public static readonly MessageStruct Success = new MessageStruct { Code = 0, Message = "成功" };
        public static readonly MessageStruct Fail = new MessageStruct { Code = 1, Message = "失败" };

        #endregion simple

        #region auth

        public static readonly MessageStruct TokenAuthFail = new MessageStruct { Code = 20001, Message = "token验证失败" };

        #endregion auth

        #region exception

        public static readonly MessageStruct ApiInterfaceException = new MessageStruct { Code = 300001, Message = "Api接口异常" };

        #endregion exception
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