using System.IO;
using Lenneth.Core.FrameWork.BouncyCastle.asn1;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.cms;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.x509;
using Lenneth.Core.FrameWork.BouncyCastle.util;
using AttributeTable = Lenneth.Core.FrameWork.BouncyCastle.asn1.cms.AttributeTable;

namespace Lenneth.Core.FrameWork.BouncyCastle.cms
{
	/**
	* Parsing class for an CMS Authenticated Data object from an input stream.
	* <p>
	* Note: that because we are in a streaming mode only one recipient can be tried and it is important
	* that the methods on the parser are called in the appropriate order.
	* </p>
	* <p>
	* Example of use - assuming the first recipient matches the private key we have.
	* <pre>
	*      CMSAuthenticatedDataParser     ad = new CMSAuthenticatedDataParser(inputStream);
	*
	*      RecipientInformationStore  recipients = ad.getRecipientInfos();
	*
	*      Collection  c = recipients.getRecipients();
	*      Iterator    it = c.iterator();
	*
	*      if (it.hasNext())
	*      {
	*          RecipientInformation   recipient = (RecipientInformation)it.next();
	*
	*          CMSTypedStream recData = recipient.getContentStream(privateKey, "BC");
	*
	*          processDataStream(recData.getContentStream());
	*
	*          if (!Arrays.equals(ad.getMac(), recipient.getMac())
	*          {
	*              System.err.println("Data corrupted!!!!");
	*          }
	*      }
	*  </pre>
	*  Note: this class does not introduce buffering - if you are processing large files you should create
	*  the parser with:
	*  <pre>
	*          CMSAuthenticatedDataParser     ep = new CMSAuthenticatedDataParser(new BufferedInputStream(inputStream, bufSize));
	*  </pre>
	*  where bufSize is a suitably large buffer size.
	* </p>
	*/
	public class CmsAuthenticatedDataParser
		: CmsContentInfoParser
	{
		internal RecipientInformationStore	_recipientInfoStore;
		internal AuthenticatedDataParser	authData;

		private AlgorithmIdentifier			macAlg;
		private byte[]						mac;
		private AttributeTable		authAttrs;
		private AttributeTable		unauthAttrs;

		private bool authAttrNotRead;
		private bool unauthAttrNotRead;

		public CmsAuthenticatedDataParser(
			byte[] envelopedData)
			: this(new MemoryStream(envelopedData, false))
		{
		}

		public CmsAuthenticatedDataParser(
			Stream envelopedData)
			: base(envelopedData)
		{
			this.authAttrNotRead = true;
			this.authData = new AuthenticatedDataParser(
				(Asn1SequenceParser)contentInfo.GetContent(Asn1Tags.Sequence));

			// TODO Validate version?
			//DerInteger version = this.authData.getVersion();

			//
			// read the recipients
			//
			Asn1Set recipientInfos = Asn1Set.GetInstance(authData.GetRecipientInfos().ToAsn1Object());

			this.macAlg = authData.GetMacAlgorithm();

			//
			// read the authenticated content info
			//
			ContentInfoParser data = authData.GetEnapsulatedContentInfo();
			CmsReadable readable = new CmsProcessableInputStream(
				((Asn1OctetStringParser)data.GetContent(Asn1Tags.OctetString)).GetOctetStream());
			CmsSecureReadable secureReadable = new CmsEnvelopedHelper.CmsAuthenticatedSecureReadable(
				this.macAlg, readable);

			//
			// build the RecipientInformationStore
			//
			this._recipientInfoStore = CmsEnvelopedHelper.BuildRecipientInformationStore(
				recipientInfos, secureReadable);
		}

		public AlgorithmIdentifier MacAlgorithmID
		{
			get { return macAlg; }
		}

		/**
		* return the object identifier for the mac algorithm.
		*/
		public string MacAlgOid
		{
            get { return macAlg.Algorithm.Id; }
		}


		/**
		 * return the ASN.1 encoded encryption algorithm parameters, or null if
		 * there aren't any.
		 */
		public Asn1Object MacAlgParams
		{
			get
			{
				Asn1Encodable ae = macAlg.Parameters;

				return ae == null ? null : ae.ToAsn1Object();
			}
		}

		/**
		* return a store of the intended recipients for this message
		*/
		public RecipientInformationStore GetRecipientInfos()
		{
			return _recipientInfoStore;
		}

		public byte[] GetMac()
		{
			if (mac == null)
			{
				GetAuthAttrs();
				mac = authData.GetMac().GetOctets();
			}
			return Arrays.Clone(mac);
		}

		/**
		* return a table of the unauthenticated attributes indexed by
		* the OID of the attribute.
		* @exception java.io.IOException
		*/
		public AttributeTable GetAuthAttrs()
		{
			if (authAttrs == null && authAttrNotRead)
			{
				Asn1SetParser s = authData.GetAuthAttrs();

				authAttrNotRead = false;

				if (s != null)
				{
					Asn1EncodableVector v = new Asn1EncodableVector();

					IAsn1Convertible o;
					while ((o = s.ReadObject()) != null)
					{
						Asn1SequenceParser seq = (Asn1SequenceParser)o;

						v.Add(seq.ToAsn1Object());
					}

					authAttrs = new AttributeTable(new DerSet(v));
				}
			}

			return authAttrs;
		}

		/**
		* return a table of the unauthenticated attributes indexed by
		* the OID of the attribute.
		* @exception java.io.IOException
		*/
		public AttributeTable GetUnauthAttrs()
		{
			if (unauthAttrs == null && unauthAttrNotRead)
			{
				Asn1SetParser s = authData.GetUnauthAttrs();

				unauthAttrNotRead = false;

				if (s != null)
				{
					Asn1EncodableVector v = new Asn1EncodableVector();

					IAsn1Convertible o;
					while ((o = s.ReadObject()) != null)
					{
						Asn1SequenceParser seq = (Asn1SequenceParser)o;

						v.Add(seq.ToAsn1Object());
					}

					unauthAttrs = new AttributeTable(new DerSet(v));
				}
			}

			return unauthAttrs;
		}
	}
}
