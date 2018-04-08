using System.Collections;

namespace Lenneth.Core.FrameWork.BouncyCastle.x509.store
{
	public interface IX509Store
	{
//		void Init(IX509StoreParameters parameters);
		ICollection GetMatches(IX509Selector selector);
	}
}
