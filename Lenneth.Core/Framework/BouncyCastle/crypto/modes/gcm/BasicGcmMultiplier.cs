namespace Lenneth.Core.FrameWork.BouncyCastle.crypto.modes.gcm
{
    public class BasicGcmMultiplier
        : IGcmMultiplier
    {
        private uint[] H;

        public void Init(byte[] H)
        {
            this.H = GcmUtilities.AsUints(H);
        }

        public void MultiplyH(byte[] x)
        {
            uint[] t = GcmUtilities.AsUints(x);
            GcmUtilities.Multiply(t, H);
            GcmUtilities.AsBytes(t, x);
        }
    }
}
