using System;

namespace Lenneth.Core.FrameWork.BouncyCastle.x509.store
{
	public interface IX509Selector
#if !(SILVERLIGHT || PORTABLE)
		: ICloneable
#endif
	{
#if SILVERLIGHT || PORTABLE
        object Clone();
#endif
        bool Match(object obj);
	}
}
