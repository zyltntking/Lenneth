using System.Collections.Generic;

namespace Lenneth.Core.Framework.Mail
{
    /// <summary>
    /// 邮件发送接口
    /// </summary>
    public interface IMail
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailSubject">邮件标题</param>
        /// <param name="mailBody">邮件内容</param>
        void Send(string mailSubject, string mailBody);

        /// <summary>
        /// 设置邮件收件人列表
        /// </summary>
        /// <param name="toMailList">收件人列表</param>
        /// <returns></returns>
        IMail To(IList<string> toMailList);

        /// <summary>
        /// 设置邮件抄送人列表
        /// </summary>
        /// <param name="ccMailList">抄送人列表</param>
        /// <param name="isCrypt">是否保密</param>
        /// <returns></returns>
        IMail Cc(IList<string> ccMailList, bool isCrypt = false);
    }
}