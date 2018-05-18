using System;
using System.Collections.Generic;
using Lenneth.WebApi.Core.Crypt;
using Lenneth.WebApi.Models;
using Lenneth.WebApi.Models.Response;
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

        /// <summary>
        /// 生成大规模数据响应
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">数据集</param>
        /// <param name="total">数据总量</param>
        /// <returns></returns>
        public static ResultContent<MassiveDataResponse<T>> MassiveResult<T>(this List<T> data, int total)
        {
            return GeneratMassiveDataResult(data, total);
        }

        /// <summary>
        /// 初始化大规模数据服务器响应
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">数据集</param>
        /// <param name="total">数据总量</param>
        /// <returns></returns>
        private static ResultContent<MassiveDataResponse<T>> GeneratMassiveDataResult<T>(List<T> data, int total)
        {
            var result = InitResult<MassiveDataResponse<T>>(MessageCode.Success);
            var content = new MassiveDataResponse<T>
            {
                data = data,
                total = total,
                timestamp = DateTime.Now
            };
            result.Data = content;

            return result;
        }
    }
}