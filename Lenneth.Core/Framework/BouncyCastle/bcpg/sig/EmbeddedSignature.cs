namespace Lenneth.Core.FrameWork.BouncyCastle.bcpg.sig
{
	/**
	 * Packet embedded signature
	 */
	public class EmbeddedSignature
		: SignatureSubpacket
	{
		public EmbeddedSignature(
			bool	critical,
            bool    isLongLength,
			byte[]	data)
			: base(SignatureSubpacketTag.EmbeddedSignature, critical, isLongLength, data)
		{
		}
	}
}
