namespace Lenneth.Core.FrameWork.BouncyCastle.asn1
{
	public interface IAsn1ApplicationSpecificParser
    	: IAsn1Convertible
	{
    	IAsn1Convertible ReadObject();
	}
}
