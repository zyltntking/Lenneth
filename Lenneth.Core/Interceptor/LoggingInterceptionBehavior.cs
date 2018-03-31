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
            //Facade.Logger.Info($"Function Name: {input.MethodBase.Name}");
            //for (var i = 0; i < input.Arguments.Count; i++)
            //{
            //    Facade.Logger.Info($"argName: {input.Arguments.ParameterName(i)}, argType: {input.Arguments[i].GetType()}, argValue: {input.Arguments[i]}");
            //}
            var result = getNext()(input, getNext);
            if (result.Exception != null)
            {
                //exception
                Facade.Logger.Error(result.Exception, $"Method {input.MethodBase} threw exception {result.Exception.Message} at {DateTime.Now.ToLongTimeString()}");
            }
            //Console.WriteLine("Finish");
            return result;
        }

        public IEnumerable<Type> GetRequiredInterfaces() => Type.EmptyTypes;

        public bool WillExecute => true;

        #endregion Implementation of IInterceptionBehavior
    }
}