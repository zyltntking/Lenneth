using Lenneth.Core.FrameWork.BouncyCastle.asn1.x509;

namespace Lenneth.Core.FrameWork.BouncyCastle.asn1.smime
{
    public class SmimeCapabilitiesAttribute
        : AttributeX509
    {
        public SmimeCapabilitiesAttribute(
            SmimeCapabilityVector capabilities)
            : base(SmimeAttributes.SmimeCapabilities,
                    new DerSet(new DerSequence(capabilities.ToAsn1EncodableVector())))
        {
        }
    }
}
