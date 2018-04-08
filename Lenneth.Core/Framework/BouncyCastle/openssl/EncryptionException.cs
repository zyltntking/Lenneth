using System;
using System.IO;

namespace Lenneth.Core.FrameWork.BouncyCastle.openssl
{
#if !(NETCF_1_0 || NETCF_2_0 || SILVERLIGHT || PORTABLE)
    [Serializable]
#endif
    public class EncryptionException
		: IOException
	{
		public EncryptionException(
			string message)
			: base(message)
		{
		}

		public EncryptionException(
			string		message,
			Exception	exception)
			: base(message, exception)
		{
		}
	}
}
