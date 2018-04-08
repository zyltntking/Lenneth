using System.IO;
using Lenneth.Core.FrameWork.BouncyCastle.asn1;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.cms;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.x509;
using Lenneth.Core.FrameWork.BouncyCastle.util;
using AttributeTable = Lenneth.Core.FrameWork.BouncyCastle.asn1.cms.AttributeTable;

namespace Lenneth.Core.FrameWork.BouncyCastle.cms
{
	/**
	* containing class for an CMS Authenticated Data object
	*/
	public class CmsAuthenticatedData
	{
		internal RecipientInformationStore recipientInfoStore;
		internal ContentInfo contentInfo;

		private AlgorithmIdentifier macAlg;
		private Asn1Set authAttrs;
		private Asn1Set unauthAttrs;
		private byte[] mac;

		public CmsAuthenticatedData(
			byte[] authData)
			: this(CmsUtilities.ReadContentInfo(authData))
		{
		}

		public CmsAuthenticatedData(
			Stream authData)
			: this(CmsUtilities.ReadContentInfo(authData))
		{
		}

		public CmsAuthenticatedData(
			ContentInfo contentInfo)
		{
			this.contentInfo = contentInfo;

			AuthenticatedData authData = AuthenticatedData.GetInstance(contentInfo.Content);

			//
			// read the recipients
			//
			Asn1Set recipientInfos = authData.RecipientInfos;

			this.macAlg = authData.MacAlgorithm;

			//
			// read the authenticated content info
			//
			ContentInfo encInfo = authData.EncapsulatedContentInfo;
			CmsReadable readable = new CmsProcessableByteArray(
				Asn1OctetString.GetInstance(encInfo.Content).GetOctets());
			CmsSecureReadable secureReadable = new CmsEnvelopedHelper.CmsAuthenticatedSecureReadable(
				this.macAlg, readable);

			//
			// build the RecipientInformationStore
			//
			this.recipientInfoStore = CmsEnvelopedHelper.BuildRecipientInformationStore(
				recipientInfos, secureReadable);

			this.authAttrs = authData.AuthAttrs;
			this.mac = authData.Mac.GetOctets();
			this.unauthAttrs = authData.UnauthAttrs;
		}

		public byte[] GetMac()
		{
			return Arrays.Clone(mac);
		}

		public AlgorithmIdentifier MacAlgorithmID
		{
			get { return macAlg; }
		}

		/**
		* return the object identifier for the content MAC algorithm.
		*/
		public string MacAlgOid
		{
            get { return macAlg.Algorithm.Id; }
		}

		/**
		* return a store of the intended recipients for this message
		*/
		public RecipientInformationStore GetRecipientInfos()
		{
			return recipientInfoStore;
		}

		/**
		 * return the ContentInfo 
		 */
		public ContentInfo ContentInfo
		{
			get { return contentInfo; }
		}

		/**
		* return a table of the digested attributes indexed by
		* the OID of the attribute.
		*/
		public AttributeTable GetAuthAttrs()
		{
			if (authAttrs == null)
				return null;

			return new AttributeTable(authAttrs);
		}

		/**
		* return a table of the undigested attributes indexed by
		* the OID of the attribute.
		*/
		public AttributeTable GetUnauthAttrs()
		{
			if (unauthAttrs == null)
				return null;

			return new AttributeTable(unauthAttrs);
		}

		/**
		* return the ASN.1 encoded representation of this object.
		*/
		public byte[] GetEncoded()
		{
			return contentInfo.GetEncoded();
		}
	}
}
