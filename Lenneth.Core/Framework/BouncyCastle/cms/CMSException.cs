using System;

namespace Lenneth.Core.FrameWork.BouncyCastle.cms
{
#if !(NETCF_1_0 || NETCF_2_0 || SILVERLIGHT || PORTABLE)
    [Serializable]
#endif
    public class CmsException
		: Exception
	{
		public CmsException()
		{
		}

		public CmsException(
			string msg)
			: base(msg)
		{
		}

		public CmsException(
			string		msg,
			Exception	e)
			: base(msg, e)
		{
		}
	}
}
