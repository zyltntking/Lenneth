using Lenneth.Core.FrameWork.BouncyCastle.asn1;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.cms;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.x509;
using Lenneth.Core.FrameWork.BouncyCastle.crypto;
using Lenneth.Core.FrameWork.BouncyCastle.crypto.parameters;
using Lenneth.Core.FrameWork.BouncyCastle.security;

namespace Lenneth.Core.FrameWork.BouncyCastle.cms
{
	/**
	 * the RecipientInfo class for a recipient who has been sent a message
	 * encrypted using a password.
	 */
	public class PasswordRecipientInformation
		: RecipientInformation
	{
		private readonly PasswordRecipientInfo	info;

		internal PasswordRecipientInformation(
			PasswordRecipientInfo	info,
			CmsSecureReadable		secureReadable)
			: base(info.KeyEncryptionAlgorithm, secureReadable)
		{
			this.info = info;
			this.rid = new RecipientID();
		}

		/**
		 * return the object identifier for the key derivation algorithm, or null
		 * if there is none present.
		 *
		 * @return OID for key derivation algorithm, if present.
		 */
		public virtual AlgorithmIdentifier KeyDerivationAlgorithm
		{
			get { return info.KeyDerivationAlgorithm; }
		}

		/**
		 * decrypt the content and return an input stream.
		 */
		public override CmsTypedStream GetContentStream(
			ICipherParameters key)
		{
			try
			{
				AlgorithmIdentifier kekAlg = AlgorithmIdentifier.GetInstance(info.KeyEncryptionAlgorithm);
				Asn1Sequence        kekAlgParams = (Asn1Sequence)kekAlg.Parameters;
				byte[]              encryptedKey = info.EncryptedKey.GetOctets();
				string              kekAlgName = DerObjectIdentifier.GetInstance(kekAlgParams[0]).Id;
				string				cName = CmsEnvelopedHelper.Instance.GetRfc3211WrapperName(kekAlgName);
				IWrapper			keyWrapper = WrapperUtilities.GetWrapper(cName);

				byte[] iv = Asn1OctetString.GetInstance(kekAlgParams[1]).GetOctets();

				ICipherParameters parameters = ((CmsPbeKey)key).GetEncoded(kekAlgName);
				parameters = new ParametersWithIV(parameters, iv);

				keyWrapper.Init(false, parameters);

				KeyParameter sKey = ParameterUtilities.CreateKeyParameter(
					GetContentAlgorithmName(), keyWrapper.Unwrap(encryptedKey, 0, encryptedKey.Length));

				return GetContentFromSessionKey(sKey);
			}
			catch (SecurityUtilityException e)
			{
				throw new CmsException("couldn't create cipher.", e);
			}
			catch (InvalidKeyException e)
			{
				throw new CmsException("key invalid in message.", e);
			}
		}
	}
}
