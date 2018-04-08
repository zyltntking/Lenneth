using Lenneth.Core.FrameWork.BouncyCastle.bcpg;

namespace Lenneth.Core.FrameWork.BouncyCastle.openpgp
{
	public class PgpExperimental
		: PgpObject
	{
		private readonly ExperimentalPacket p;

		public PgpExperimental(
			BcpgInputStream bcpgIn)
		{
			p = (ExperimentalPacket) bcpgIn.ReadPacket();
		}
	}
}
