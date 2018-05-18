namespace Lenneth.WebApi.Models
{
    internal static class MessageCode
    {
        #region simple

        public static readonly MessageStruct Success = new MessageStruct { Code = 0, Message = "成功" };
        public static readonly MessageStruct Fail = new MessageStruct { Code = 1, Message = "失败" };

        #endregion simple

        #region auth

        public static readonly MessageStruct ApiKeyValiFail = new MessageStruct { Code = 20001, Message = "apiKey验证失败" };
        public static readonly MessageStruct TokenAuthFail = new MessageStruct { Code = 20002, Message = "token验证失败" };
        public static readonly MessageStruct UserNameOrPassWordError = new MessageStruct{ Code = 21001, Message = "用户名或密码错误" };
        public static readonly MessageStruct SilentAuthFail = new MessageStruct { Code = 21002, Message = "静默授权失败，可用凭证已过期或不存在" };

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