using System.IO;

namespace Lenneth.Core.FrameWork.BouncyCastle.crypto.tls
{
	public interface TlsCompression
	{
		Stream Compress(Stream output);

		Stream Decompress(Stream output);
	}
}
