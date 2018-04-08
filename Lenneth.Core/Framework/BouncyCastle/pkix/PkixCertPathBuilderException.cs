using System;
using Lenneth.Core.FrameWork.BouncyCastle.security;

namespace Lenneth.Core.FrameWork.BouncyCastle.pkix
{
	/// <summary>
	/// Summary description for PkixCertPathBuilderException.
	/// </summary>
#if !(NETCF_1_0 || NETCF_2_0 || SILVERLIGHT || PORTABLE)
    [Serializable]
#endif
    public class PkixCertPathBuilderException : GeneralSecurityException
	{
		public PkixCertPathBuilderException() : base() { }
		
		public PkixCertPathBuilderException(string message) : base(message)	{ }  

		public PkixCertPathBuilderException(string message, Exception exception) : base(message, exception) { }
		
	}
}
