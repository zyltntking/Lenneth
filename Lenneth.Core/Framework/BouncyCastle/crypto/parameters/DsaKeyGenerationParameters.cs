using Lenneth.Core.FrameWork.BouncyCastle.security;

namespace Lenneth.Core.FrameWork.BouncyCastle.crypto.parameters
{
    public class DsaKeyGenerationParameters
		: KeyGenerationParameters
    {
        private readonly DsaParameters parameters;

        public DsaKeyGenerationParameters(
            SecureRandom	random,
            DsaParameters	parameters)
			: base(random, parameters.P.BitLength - 1)
        {
            this.parameters = parameters;
        }

		public DsaParameters Parameters
        {
            get { return parameters; }
        }
    }

}
