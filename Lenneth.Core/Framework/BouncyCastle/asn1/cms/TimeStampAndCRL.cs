using Lenneth.Core.FrameWork.BouncyCastle.asn1.x509;

namespace Lenneth.Core.FrameWork.BouncyCastle.asn1.cms
{
	public class TimeStampAndCrl
		: Asn1Encodable
	{
		private ContentInfo timeStamp;
		private CertificateList crl;

		public TimeStampAndCrl(ContentInfo timeStamp)
		{
			this.timeStamp = timeStamp;
		}

		private TimeStampAndCrl(Asn1Sequence seq)
		{
			this.timeStamp = ContentInfo.GetInstance(seq[0]);
			if (seq.Count == 2)
			{
				this.crl = CertificateList.GetInstance(seq[1]);
			}
		}

		public static TimeStampAndCrl GetInstance(object obj)
		{
			if (obj is TimeStampAndCrl)
				return (TimeStampAndCrl)obj;

			if (obj != null)
				return new TimeStampAndCrl(Asn1Sequence.GetInstance(obj));

			return null;
		}

		public virtual ContentInfo TimeStampToken
		{
			get { return this.timeStamp; }
		}

		public virtual CertificateList Crl
		{
			get { return this.crl; }
		}

		/**
		 * <pre>
		 * TimeStampAndCRL ::= SEQUENCE {
		 *     timeStamp   TimeStampToken,          -- according to RFC 3161
		 *     crl         CertificateList OPTIONAL -- according to RFC 5280
		 *  }
		 * </pre>
		 * @return
		 */
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector v = new Asn1EncodableVector(timeStamp);
			v.AddOptional(crl);
			return new DerSequence(v);
		}
	}
}
