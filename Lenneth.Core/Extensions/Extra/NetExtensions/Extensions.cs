using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Lenneth.Core.Extensions.Extra.NetExtensions
{
    public static class Extensions
    {
        #region MailMessage

        #region Send

        /// <summary>
        ///     A MailMessage extension method that send this message.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        public static void Send(this MailMessage @this)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Send(@this);
            }
        }

        #endregion Send

        #region SendAsync

        /// <summary>
        ///     A MailMessage extension method that sends this message asynchronous.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="userToken">The user token.</param>
        public static void SendAsync(this MailMessage @this, object userToken)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.SendAsync(@this, userToken);
            }
        }

        #endregion SendAsync

        #endregion MailMessage

        #region WebRequest

        #region GetResponseSafe

        /// <summary>
        ///     A WebRequest extension method that gets the WebRequest response or the WebException response.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The WebRequest response or WebException response.</returns>
        public static WebResponse GetResponseSafe(this WebRequest @this)
        {
            try
            {
                return @this.GetResponse();
            }
            catch (WebException e)
            {
                return e.Response;
            }
        }

        #endregion GetResponseSafe

        #endregion WebRequest

        #region WebResponse

        #region ReadToEnd

        /// <summary>
        ///     A WebResponse extension method that reads the response stream to the end.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The response stream as a string, from the current position to the end.</returns>
        public static string ReadToEnd(this WebResponse @this)
        {
            using (var stream = @this.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        #endregion ReadToEnd

        #region ReadToEndAndDispose

        /// <summary>
        ///     A WebRequest extension method that gets the WebRequest response and read the response stream.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The response stream as a string, from the current position to the end.</returns>
        public static string ReadToEndAndDispose(this WebResponse @this)
        {
            using (var response = @this)
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream, Encoding.Default))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        #endregion ReadToEndAndDispose

        #endregion WebResponse
    }
}