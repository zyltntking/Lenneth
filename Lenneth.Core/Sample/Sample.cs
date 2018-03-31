using System;

namespace Lenneth.Core.Sample
{
    /// <inheritdoc />
    internal class Sample : ISample
    {
        #region Implementation of ISample

        public virtual void SampleMethod()
        {
            Console.WriteLine("I'm in sample method");
        }

        #endregion Implementation of ISample
    }
}