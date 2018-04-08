using System;

namespace Lenneth.Core.FrameWork.BouncyCastle.security.cert
{
#if !(NETCF_1_0 || NETCF_2_0 || SILVERLIGHT || PORTABLE)
    [Serializable]
#endif
    public class CertificateExpiredException : CertificateException
	{
		public CertificateExpiredException() : base() { }
		public CertificateExpiredException(string message) : base(message) { }
		public CertificateExpiredException(string message, Exception exception) : base(message, exception) { }
	}
}
