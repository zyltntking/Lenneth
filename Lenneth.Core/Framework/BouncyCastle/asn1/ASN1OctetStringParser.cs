using System.IO;

namespace Lenneth.Core.FrameWork.BouncyCastle.asn1
{
	public interface Asn1OctetStringParser
		: IAsn1Convertible
	{
		Stream GetOctetStream();
	}
}
