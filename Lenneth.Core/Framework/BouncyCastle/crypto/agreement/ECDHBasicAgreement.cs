using System;
using Lenneth.Core.FrameWork.BouncyCastle.crypto.parameters;
using Lenneth.Core.FrameWork.BouncyCastle.math;
using Lenneth.Core.FrameWork.BouncyCastle.math.ec;

namespace Lenneth.Core.FrameWork.BouncyCastle.crypto.agreement
{
    /**
     * P1363 7.2.1 ECSVDP-DH
     *
     * ECSVDP-DH is Elliptic Curve Secret Value Derivation Primitive,
     * Diffie-Hellman version. It is based on the work of [DH76], [Mil86],
     * and [Kob87]. This primitive derives a shared secret value from one
     * party's private key and another party's public key, where both have
     * the same set of EC domain parameters. If two parties correctly
     * execute this primitive, they will produce the same output. This
     * primitive can be invoked by a scheme to derive a shared secret key;
     * specifically, it may be used with the schemes ECKAS-DH1 and
     * DL/ECKAS-DH2. It assumes that the input keys are valid (see also
     * Section 7.2.2).
     */
    public class ECDHBasicAgreement
        : IBasicAgreement
    {
        protected internal ECPrivateKeyParameters privKey;

        public virtual void Init(
            ICipherParameters parameters)
        {
            if (parameters is ParametersWithRandom)
            {
                parameters = ((ParametersWithRandom)parameters).Parameters;
            }

            this.privKey = (ECPrivateKeyParameters)parameters;
        }

        public virtual int GetFieldSize()
        {
            return (privKey.Parameters.Curve.FieldSize + 7) / 8;
        }

        public virtual BigInteger CalculateAgreement(
            ICipherParameters pubKey)
        {
            ECPublicKeyParameters pub = (ECPublicKeyParameters) pubKey;
            ECPoint P = pub.Q.Multiply(privKey.D).Normalize();

            if (P.IsInfinity)
                throw new InvalidOperationException("Infinity is not a valid agreement value for ECDH");

            return P.AffineXCoord.ToBigInteger();
        }
    }
}