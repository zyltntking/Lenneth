namespace Lenneth.Core.Framework.Mail
{
    internal class MailConfig
    {
        /// <summary>
        /// 邮件服务器地址
        /// </summary>
        public string MailHost { get; set; }

        /// <summary>
        /// 邮件服务器端口号
        /// </summary>
        public int MailPort { get; set; }

        /// <summary>
        /// 邮箱用户名
        /// </summary>
        public string MailAddress { get; set; }

        /// <summary>
        /// 邮箱密码
        /// </summary>
        public string MailPassword { get; set; }

        /// <summary>
        /// 邮件签名
        /// </summary>
        public string MailSign { get; set; }
    }
}