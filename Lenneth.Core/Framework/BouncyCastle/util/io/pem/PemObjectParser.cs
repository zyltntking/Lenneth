using System.IO;

namespace Lenneth.Core.FrameWork.BouncyCastle.util.io.pem
{
	public interface PemObjectParser
	{
		/// <param name="obj">
		/// A <see cref="PemObject"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Object"/>
		/// </returns>
		/// <exception cref="IOException"></exception>
		object ParseObject(PemObject obj);
	}
}
