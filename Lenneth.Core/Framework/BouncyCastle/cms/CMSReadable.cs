using System.IO;

namespace Lenneth.Core.FrameWork.BouncyCastle.cms
{
	public interface CmsReadable
	{
		Stream GetInputStream();
	}
}
