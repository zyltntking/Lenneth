using System;

namespace Lenneth.Core.FrameWork.BouncyCastle.security.cert
{
#if !(NETCF_1_0 || NETCF_2_0 || SILVERLIGHT || PORTABLE)
    [Serializable]
#endif
    public class CertificateException : GeneralSecurityException
	{
		public CertificateException() : base() { }
		public CertificateException(string message) : base(message) { }
		public CertificateException(string message, Exception exception) : base(message, exception) { }
	}
}
