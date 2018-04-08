using Lenneth.Core.FrameWork.BouncyCastle.crypto;
using Lenneth.Core.FrameWork.BouncyCastle.util.io;

namespace Lenneth.Core.FrameWork.BouncyCastle.cms
{
	internal class DigOutputStream
		: BaseOutputStream
	{
		private readonly IDigest dig;

		internal DigOutputStream(IDigest dig)
		{
			this.dig = dig;
		}

		public override void WriteByte(byte b)
		{
			dig.Update(b);
		}

		public override void Write(byte[] b, int off, int len)
		{
			dig.BlockUpdate(b, off, len);
		}
	}
}
