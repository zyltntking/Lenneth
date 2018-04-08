using System;
using System.IO;

namespace Lenneth.Core.FrameWork.BouncyCastle.openssl
{
#if !(NETCF_1_0 || NETCF_2_0 || SILVERLIGHT || PORTABLE)
    [Serializable]
#endif
    public class PemException
		: IOException
	{
		public PemException(
			string message)
			: base(message)
		{
		}

		public PemException(
			string		message,
			Exception	exception)
			: base(message, exception)
		{
		}
	}
}
