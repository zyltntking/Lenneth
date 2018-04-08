using Lenneth.Core.FrameWork.BouncyCastle.crypto;
using Lenneth.Core.FrameWork.BouncyCastle.security;
using Lenneth.Core.FrameWork.BouncyCastle.util.io;

namespace Lenneth.Core.FrameWork.BouncyCastle.cms
{
	internal class SigOutputStream
		: BaseOutputStream
	{
		private readonly ISigner sig;

		internal SigOutputStream(ISigner sig)
		{
			this.sig = sig;
		}

		public override void WriteByte(byte b)
		{
			try
			{
				sig.Update(b);
			}
			catch (SignatureException e)
			{
				throw new CmsStreamException("signature problem: " + e);
			}
		}

		public override void Write(byte[] b, int off, int len)
		{
			try
			{
				sig.BlockUpdate(b, off, len);
			}
			catch (SignatureException e)
			{
				throw new CmsStreamException("signature problem: " + e);
			}
		}
	}
}