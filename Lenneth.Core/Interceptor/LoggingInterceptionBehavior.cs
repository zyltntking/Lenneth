using System;
using System.Collections.Generic;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

namespace Lenneth.Core.Interceptor
{
    internal abstract class LoggingInterceptionBehavior : IInterceptionBehavior
    {
        #region Implementation of IInterceptionBehavior

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            Console.WriteLine($"===进入方法，方法名: {input.MethodBase.Name}===");
            Console.WriteLine();
            Console.WriteLine("====参数区域====:");
            for (var i = 0; i < input.Arguments.Count; i++)
            {
                Console.WriteLine($"参数名: {input.Arguments.ParameterName(i)}");
                Console.WriteLine($"参数类型: {input.Arguments[i].GetType()}");
                Console.WriteLine($"参数值: {input.Arguments[i]}");
            }
            Console.WriteLine("====参数区域====:");
            Console.WriteLine();
            Console.WriteLine("执行前,可在这里记录执行前的日志");
            Console.WriteLine();
            var result = getNext()(input, getNext);//progress
            Console.WriteLine();
            if (result.Exception != null)
            {
                //exception
                Console.WriteLine("异常发生时,可在这里记录异常发生时的日志");
                Console.WriteLine( $"Method {input.MethodBase} threw exception {result.Exception.Message} at {DateTime.Now.ToLongTimeString()}");
                Console.WriteLine();
            }
            Console.WriteLine("执行后,可在这里记录执行后的日志");
            return result;
        }

        public IEnumerable<Type> GetRequiredInterfaces() => Type.EmptyTypes;

        public bool WillExecute => true;

        #endregion
    }
}