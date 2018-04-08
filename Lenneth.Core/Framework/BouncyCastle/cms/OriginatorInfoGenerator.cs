using System.Collections;
using Lenneth.Core.FrameWork.BouncyCastle.asn1;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.cms;
using Lenneth.Core.FrameWork.BouncyCastle.util;
using Lenneth.Core.FrameWork.BouncyCastle.x509;
using Lenneth.Core.FrameWork.BouncyCastle.x509.store;

namespace Lenneth.Core.FrameWork.BouncyCastle.cms
{
    public class OriginatorInfoGenerator
    {
        private readonly IList origCerts;
        private readonly IList origCrls;

        public OriginatorInfoGenerator(X509Certificate origCert)
        {
            this.origCerts = Platform.CreateArrayList(1);
            this.origCrls = null;
            origCerts.Add(origCert.CertificateStructure);
        }

        public OriginatorInfoGenerator(IX509Store origCerts)
            : this(origCerts, null)
        {
        }

        public OriginatorInfoGenerator(IX509Store origCerts, IX509Store origCrls)
        {
            this.origCerts = CmsUtilities.GetCertificatesFromStore(origCerts);
            this.origCrls = origCrls == null ? null : CmsUtilities.GetCrlsFromStore(origCrls);
        }

        public virtual OriginatorInfo Generate()
        {
            Asn1Set certSet = CmsUtilities.CreateDerSetFromList(origCerts);
            Asn1Set crlSet = origCrls == null ? null : CmsUtilities.CreateDerSetFromList(origCrls);
            return new OriginatorInfo(certSet, crlSet);
        }
    }
}
