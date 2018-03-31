using System.Collections.Generic;
using Unity.Interception.Utilities;

namespace Lenneth.Core.Framework.Mail
{
    /// <inheritdoc />
    /// <summary>
    /// 邮件发送类
    /// </summary>
    internal abstract class Mail:IMail
    {
        /// <summary>
        /// 发送人列表
        /// </summary>
        protected IList<string> MailToList { get; private set; }
        /// <summary>
        /// 密件抄送列表
        /// </summary>
        protected IList<string> MailBccList { get; private set; }
        /// <summary>
        /// 抄送人列表
        /// </summary>
        protected IList<string> MailCcList { get; private set; }
        /// <summary>
        /// 邮件服务器地址
        /// </summary>
        protected string MailHost { get; private set; }
        /// <summary>
        /// 邮件服务器端口号
        /// </summary>
        protected int MailPort { get; private set; }
        /// <summary>
        /// 邮箱用户名
        /// </summary>
        protected string MailAddress { get; private set; }
        /// <summary>
        /// 邮箱密码
        /// </summary>
        protected string MailPassword { get; private set; }
        /// <summary>
        /// 邮件签名
        /// </summary>
        protected string MailSign { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">邮件配置信息</param>
        protected Mail(MailConfig config)
        {
            InitMail(
                mailHost: config.MailHost,
                mailPort:config.MailPort,
                mailAddress:config.MailAddress,
                mailPassword:config.MailPassword,
                mailSign:config.MailSign);
        }

        /// <summary>
        /// 初始化邮件引擎
        /// </summary>
        /// <param name="mailHost">邮件服务器地址</param>
        /// <param name="mailPort">邮件服务器端口</param>
        /// <param name="mailAddress">邮箱账号</param>
        /// <param name="mailPassword">邮箱密码</param>
        /// <param name="mailSign">邮件标记</param>
        private void InitMail(string mailHost, int mailPort, string mailAddress, string mailPassword,string mailSign)
        {
            MailHost = mailHost;
            MailPort = mailPort;
            MailAddress = mailAddress;
            MailPassword = mailPassword;
            MailSign = mailSign;
        }

        /// <inheritdoc />
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailSubject">邮件标题</param>
        /// <param name="mailBody">邮件内容</param>
        public virtual void Send(string mailSubject, string mailBody)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// 设置邮件收件人列表
        /// </summary>
        /// <param name="toMailList">收件人列表</param>
        /// <returns></returns>
        public IMail To(IList<string> toMailList)
        {
            if (MailToList == null)
            {
                MailToList = toMailList;
            }
            else
            {
                toMailList.ForEach(t => MailToList.Add(t));
            }
            
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// 设置邮件抄送人列表
        /// </summary>
        /// <param name="ccMailList">抄送人列表</param>
        /// <param name="isCrypt">是否保密</param>
        /// <returns></returns>
        public IMail Cc(IList<string> ccMailList, bool isCrypt = false)
        {
            if (isCrypt)
            {
                if (MailToList == null)
                {
                    MailBccList = ccMailList;
                }
                else
                {
                    ccMailList.ForEach(t => MailBccList.Add(t));
                }
            }
            else
            {
                if (MailToList == null)
                {
                    MailCcList = ccMailList;
                }
                else
                {
                    ccMailList.ForEach(t => MailCcList.Add(t));
                }

            }
            return this;
        }
    }
}