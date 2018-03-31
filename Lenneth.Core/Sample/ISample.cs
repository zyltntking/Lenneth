using Lenneth.Core.Interceptor;

namespace Lenneth.Core.Sample
{
    [CallHandler]
    public interface ISample
    {
        
        void SampleMethod();
    }
}