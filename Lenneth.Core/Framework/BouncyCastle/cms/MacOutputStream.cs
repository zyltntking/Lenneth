using Lenneth.Core.FrameWork.BouncyCastle.crypto;
using Lenneth.Core.FrameWork.BouncyCastle.util.io;

namespace Lenneth.Core.FrameWork.BouncyCastle.cms
{
	internal class MacOutputStream
		: BaseOutputStream
	{
		private readonly IMac mac;

		internal MacOutputStream(IMac mac)
		{
			this.mac = mac;
		}

		public override void Write(byte[] b, int off, int len)
		{
			mac.BlockUpdate(b, off, len);
		}

		public override void WriteByte(byte b)
		{
			mac.Update(b);
		}
	}
}
