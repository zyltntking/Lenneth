namespace Lenneth.Core.FrameWork.BouncyCastle.asn1
{
	public interface Asn1SetParser
		: IAsn1Convertible
	{
		IAsn1Convertible ReadObject();
	}
}
