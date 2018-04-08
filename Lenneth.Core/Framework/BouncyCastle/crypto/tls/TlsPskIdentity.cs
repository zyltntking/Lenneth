namespace Lenneth.Core.FrameWork.BouncyCastle.crypto.tls
{
	public interface TlsPskIdentity
	{
		void SkipIdentityHint();

		void NotifyIdentityHint(byte[] psk_identity_hint);

		byte[] GetPskIdentity();

		byte[] GetPsk();
	}
}
