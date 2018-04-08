using System;
using System.Collections;
using System.IO;
using Lenneth.Core.FrameWork.BouncyCastle.asn1;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.ess;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.pkcs;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.tsp;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.x509;
using Lenneth.Core.FrameWork.BouncyCastle.cms;
using Lenneth.Core.FrameWork.BouncyCastle.crypto;
using Lenneth.Core.FrameWork.BouncyCastle.math;
using Lenneth.Core.FrameWork.BouncyCastle.security;
using Lenneth.Core.FrameWork.BouncyCastle.security.cert;
using Lenneth.Core.FrameWork.BouncyCastle.util;
using Lenneth.Core.FrameWork.BouncyCastle.x509;
using Lenneth.Core.FrameWork.BouncyCastle.x509.store;
using Attribute = Lenneth.Core.FrameWork.BouncyCastle.asn1.cms.Attribute;
using AttributeTable = Lenneth.Core.FrameWork.BouncyCastle.asn1.cms.AttributeTable;

namespace Lenneth.Core.FrameWork.BouncyCastle.tsp
{
	public class TimeStampTokenGenerator
	{
		private int accuracySeconds = -1;
		private int accuracyMillis = -1;
		private int accuracyMicros = -1;
		private bool ordering = false;
		private GeneralName tsa = null;
		private string tsaPolicyOID;

		private AsymmetricKeyParameter	key;
		private X509Certificate			cert;
		private string					digestOID;
		private AttributeTable	signedAttr;
		private AttributeTable	unsignedAttr;
		private IX509Store				x509Certs;
		private IX509Store				x509Crls;

		/**
		 * basic creation - only the default attributes will be included here.
		 */
		public TimeStampTokenGenerator(
			AsymmetricKeyParameter	key,
			X509Certificate			cert,
			string					digestOID,
			string					tsaPolicyOID)
			: this(key, cert, digestOID, tsaPolicyOID, null, null)
		{
		}

		/**
		 * create with a signer with extra signed/unsigned attributes.
		 */
		public TimeStampTokenGenerator(
			AsymmetricKeyParameter	key,
			X509Certificate			cert,
			string					digestOID,
			string					tsaPolicyOID,
			AttributeTable	signedAttr,
			AttributeTable	unsignedAttr)
		{
			this.key = key;
			this.cert = cert;
			this.digestOID = digestOID;
			this.tsaPolicyOID = tsaPolicyOID;
			this.unsignedAttr = unsignedAttr;

			TspUtil.ValidateCertificate(cert);

			//
			// Add the ESSCertID attribute
			//
			IDictionary signedAttrs;
			if (signedAttr != null)
			{
				signedAttrs = signedAttr.ToDictionary();
			}
			else
			{
				signedAttrs = Platform.CreateHashtable();
			}

			try
			{
				byte[] hash = DigestUtilities.CalculateDigest("SHA-1", cert.GetEncoded());

				EssCertID essCertid = new EssCertID(hash);

				Attribute attr = new Attribute(
					PkcsObjectIdentifiers.IdAASigningCertificate,
					new DerSet(new SigningCertificate(essCertid)));

				signedAttrs[attr.AttrType] = attr;
			}
			catch (CertificateEncodingException e)
			{
				throw new TspException("Exception processing certificate.", e);
			}
			catch (SecurityUtilityException e)
			{
				throw new TspException("Can't find a SHA-1 implementation.", e);
			}

			this.signedAttr = new AttributeTable(signedAttrs);
		}

		public void SetCertificates(
			IX509Store certificates)
		{
			this.x509Certs = certificates;
		}

		public void SetCrls(
			IX509Store crls)
		{
			this.x509Crls = crls;
		}

		public void SetAccuracySeconds(
			int accuracySeconds)
		{
			this.accuracySeconds = accuracySeconds;
		}

		public void SetAccuracyMillis(
			int accuracyMillis)
		{
			this.accuracyMillis = accuracyMillis;
		}

		public void SetAccuracyMicros(
			int accuracyMicros)
		{
			this.accuracyMicros = accuracyMicros;
		}

		public void SetOrdering(
			bool ordering)
		{
			this.ordering = ordering;
		}

		public void SetTsa(
			GeneralName tsa)
		{
			this.tsa = tsa;
		}

		//------------------------------------------------------------------------------

		public TimeStampToken Generate(
			TimeStampRequest	request,
			BigInteger			serialNumber,
			DateTime			genTime)
		{
			DerObjectIdentifier digestAlgOID = new DerObjectIdentifier(request.MessageImprintAlgOid);

			AlgorithmIdentifier algID = new AlgorithmIdentifier(digestAlgOID, DerNull.Instance);
			MessageImprint messageImprint = new MessageImprint(algID, request.GetMessageImprintDigest());

			Accuracy accuracy = null;
			if (accuracySeconds > 0 || accuracyMillis > 0 || accuracyMicros > 0)
			{
				DerInteger seconds = null;
				if (accuracySeconds > 0)
				{
					seconds = new DerInteger(accuracySeconds);
				}

				DerInteger millis = null;
				if (accuracyMillis > 0)
				{
					millis = new DerInteger(accuracyMillis);
				}

				DerInteger micros = null;
				if (accuracyMicros > 0)
				{
					micros = new DerInteger(accuracyMicros);
				}

				accuracy = new Accuracy(seconds, millis, micros);
			}

			DerBoolean derOrdering = null;
			if (ordering)
			{
				derOrdering = DerBoolean.GetInstance(ordering);
			}

			DerInteger nonce = null;
			if (request.Nonce != null)
			{
				nonce = new DerInteger(request.Nonce);
			}

			DerObjectIdentifier tsaPolicy = new DerObjectIdentifier(tsaPolicyOID);
			if (request.ReqPolicy != null)
			{
				tsaPolicy = new DerObjectIdentifier(request.ReqPolicy);
			}

			TstInfo tstInfo = new TstInfo(tsaPolicy, messageImprint,
				new DerInteger(serialNumber), new DerGeneralizedTime(genTime), accuracy,
				derOrdering, nonce, tsa, request.Extensions);

			try
			{
				CmsSignedDataGenerator signedDataGenerator = new CmsSignedDataGenerator();

				byte[] derEncodedTstInfo = tstInfo.GetDerEncoded();

				if (request.CertReq)
				{
					signedDataGenerator.AddCertificates(x509Certs);
				}

				signedDataGenerator.AddCrls(x509Crls);
				signedDataGenerator.AddSigner(key, cert, digestOID, signedAttr, unsignedAttr);

				CmsSignedData signedData = signedDataGenerator.Generate(
					PkcsObjectIdentifiers.IdCTTstInfo.Id,
					new CmsProcessableByteArray(derEncodedTstInfo),
					true);

				return new TimeStampToken(signedData);
			}
			catch (CmsException cmsEx)
			{
				throw new TspException("Error generating time-stamp token", cmsEx);
			}
			catch (IOException e)
			{
				throw new TspException("Exception encoding info", e);
			}
			catch (X509StoreException e)
			{
				throw new TspException("Exception handling CertStore", e);
			}
//			catch (InvalidAlgorithmParameterException e)
//			{
//				throw new TspException("Exception handling CertStore CRLs", e);
//			}
		}
	}
}
