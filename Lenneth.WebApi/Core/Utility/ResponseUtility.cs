using Lenneth.WebApi.Models;

namespace Lenneth.WebApi.Core.Utility
{
    internal class ResponseUtility
    {
        public static ResultContent<T> InitResult<T>(MessageStruct result)
        {
            return new ResultContent<T>
            {
                Code = result.Code,
                Message = result.Message
            };
        }
    }
}