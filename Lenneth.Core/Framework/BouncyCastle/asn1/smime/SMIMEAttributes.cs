using Lenneth.Core.FrameWork.BouncyCastle.asn1.pkcs;

namespace Lenneth.Core.FrameWork.BouncyCastle.asn1.smime
{
    public abstract class SmimeAttributes
    {
        public static readonly DerObjectIdentifier SmimeCapabilities = PkcsObjectIdentifiers.Pkcs9AtSmimeCapabilities;
        public static readonly DerObjectIdentifier EncrypKeyPref = PkcsObjectIdentifiers.IdAAEncrypKeyPref;
    }
}
