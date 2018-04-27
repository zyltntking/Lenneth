using Lenneth.WebApi.Core.Crypt;
using Lenneth.WebApi.Models;

using Newtonsoft.Json;

using Unity;

namespace Lenneth.WebApi.Core.Utility
{
    internal static class ResponseUtility
    {
        /// <summary>
        /// 初始化结果文档
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="result">返回消息结果</param>
        /// <returns></returns>
        public static ResultContent<T> InitResult<T>(MessageStruct result)
        {
            return new ResultContent<T>
            {
                Code = result.Code,
                Message = result.Message
            };
        }

        /// <summary>
        /// 加密内容对象
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="content">内容对象</param>
        /// <returns></returns>
        public static ResultContent<string> EncryptContent<T>(ResultContent<T> content)
        {
            return new ResultContent<string>
            {
                Code = content.Code,
                Message = content.Message,
                Data = UnityConfig.Container.Resolve<ICrypt>().Encrypt(JsonConvert.SerializeObject(content.Data))
            };
        }
    }
}