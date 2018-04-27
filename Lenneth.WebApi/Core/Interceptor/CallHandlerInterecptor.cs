using System;
using Unity.Interception.PolicyInjection.Pipeline;

namespace Lenneth.WebApi.Core.Interceptor
{
    internal class CallHandlerInterecptor : ICallHandler
    {
        #region Implementation of ICallHandler

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            Console.WriteLine($"Function Name: {input.MethodBase.Name}");
            for (var i = 0; i < input.Arguments.Count; i++)
            {
                Console.WriteLine($"argName: {input.Arguments.ParameterName(i)}");
                Console.WriteLine($"argType: {input.Arguments[i].GetType()}");
                Console.WriteLine($"argValue: {input.Arguments[i]}");
            }
            var result = getNext()(input, getNext);//progress
            Console.WriteLine();
            if (result.Exception != null)
            {
                //exception
                Console.WriteLine($"Method {input.MethodBase} threw exception {result.Exception.Message} at {DateTime.Now.ToLongTimeString()}");
            }
            Console.WriteLine("Function finish");
            return result;
        }

        public int Order { get; set; }

        #endregion Implementation of ICallHandler
    }
}