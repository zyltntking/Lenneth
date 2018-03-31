using System.Net;
using System.Net.Mail;
using System.Text;

namespace Lenneth.Core.Framework.Mail
{
    /// <inheritdoc />
    /// <summary>
    /// SMTP邮件引擎
    /// </summary>
    internal sealed class SmtpMail : Mail
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">邮件配置信息</param>
        public SmtpMail(MailConfig config) : base(config)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailSubject">邮件标题</param>
        /// <param name="mailBody">邮件内容</param>
        public override void Send(string mailSubject, string mailBody)
        {
            try
            {
                using (var client = new SmtpClient(MailHost, MailPort))
                {
                    client.Credentials = new NetworkCredential(MailAddress, MailPassword);
                    client.EnableSsl = true;

                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(MailAddress, MailSign, Encoding.UTF8);
                        if (MailToList != null)
                        {
                            foreach (var address in MailToList)
                            {
                                message.To.Add(new MailAddress(address));
                            }
                        }
                        if (MailCcList != null)
                        {
                            foreach (var address in MailCcList)
                            {
                                message.CC.Add(new MailAddress(address));
                            }
                        }
                        if (MailBccList != null)
                        {
                            foreach (var address in MailBccList)
                            {
                                message.Bcc.Add(new MailAddress(address));
                            }
                        }
                        message.IsBodyHtml = true;
                        message.Subject = mailSubject;
                        message.SubjectEncoding = Encoding.UTF8;
                        message.Body = mailBody;
                        message.BodyEncoding = Encoding.UTF8;
                        client.Send(message);
                    }
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}