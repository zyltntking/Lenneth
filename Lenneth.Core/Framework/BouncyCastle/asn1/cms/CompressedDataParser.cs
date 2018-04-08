using Lenneth.Core.FrameWork.BouncyCastle.asn1.x509;

namespace Lenneth.Core.FrameWork.BouncyCastle.asn1.cms
{
	/**
	* RFC 3274 - CMS Compressed Data.
	* <pre>
	* CompressedData ::= SEQUENCE {
	*  version CMSVersion,
	*  compressionAlgorithm CompressionAlgorithmIdentifier,
	*  encapContentInfo EncapsulatedContentInfo
	* }
	* </pre>
	*/
	public class CompressedDataParser
	{
		private DerInteger			_version;
		private AlgorithmIdentifier	_compressionAlgorithm;
		private ContentInfoParser	_encapContentInfo;

		public CompressedDataParser(
			Asn1SequenceParser seq)
		{
			this._version = (DerInteger)seq.ReadObject();
			this._compressionAlgorithm = AlgorithmIdentifier.GetInstance(seq.ReadObject().ToAsn1Object());
			this._encapContentInfo = new ContentInfoParser((Asn1SequenceParser)seq.ReadObject());
		}

		public DerInteger Version
		{
			get { return _version; }
		}

		public AlgorithmIdentifier CompressionAlgorithmIdentifier
		{
			get { return _compressionAlgorithm; }
		}

		public ContentInfoParser GetEncapContentInfo()
		{
			return _encapContentInfo;
		}
	}
}
