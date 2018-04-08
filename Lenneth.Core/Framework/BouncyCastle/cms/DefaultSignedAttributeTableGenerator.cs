using System;
using System.Collections;
using Lenneth.Core.FrameWork.BouncyCastle.asn1;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.cms;
using Lenneth.Core.FrameWork.BouncyCastle.util;
using Attribute = Lenneth.Core.FrameWork.BouncyCastle.asn1.cms.Attribute;

namespace Lenneth.Core.FrameWork.BouncyCastle.cms
{
	/**
	 * Default signed attributes generator.
	 */
	public class DefaultSignedAttributeTableGenerator
		: CmsAttributeTableGenerator
	{
		private readonly IDictionary table;

		/**
		 * Initialise to use all defaults
		 */
		public DefaultSignedAttributeTableGenerator()
		{
			table = Platform.CreateHashtable();
		}

		/**
		 * Initialise with some extra attributes or overrides.
		 *
		 * @param attributeTable initial attribute table to use.
		 */
		public DefaultSignedAttributeTableGenerator(
			AttributeTable attributeTable)
		{
			if (attributeTable != null)
			{
				table = attributeTable.ToDictionary();
			}
			else
			{
				table = Platform.CreateHashtable();
			}
		}

#if SILVERLIGHT || PORTABLE
		/**
		 * Create a standard attribute table from the passed in parameters - this will
		 * normally include contentType, signingTime, and messageDigest. If the constructor
		 * using an AttributeTable was used, entries in it for contentType, signingTime, and
		 * messageDigest will override the generated ones.
		 *
		 * @param parameters source parameters for table generation.
		 *
		 * @return a filled in Hashtable of attributes.
		 */
		protected virtual IDictionary createStandardAttributeTable(
			IDictionary parameters)
		{
            IDictionary std = Platform.CreateHashtable(table);
            DoCreateStandardAttributeTable(parameters, std);
            return std;
		}
#else
        /**
		 * Create a standard attribute table from the passed in parameters - this will
		 * normally include contentType, signingTime, and messageDigest. If the constructor
		 * using an AttributeTable was used, entries in it for contentType, signingTime, and
		 * messageDigest will override the generated ones.
		 *
		 * @param parameters source parameters for table generation.
		 *
		 * @return a filled in Hashtable of attributes.
		 */
		protected virtual Hashtable createStandardAttributeTable(
			IDictionary parameters)
		{
            Hashtable std = new Hashtable(table);
            DoCreateStandardAttributeTable(parameters, std);
			return std;
		}
#endif

        private void DoCreateStandardAttributeTable(IDictionary parameters, IDictionary std)
        {
            // contentType will be absent if we're trying to generate a counter signature.
            if (parameters.Contains(CmsAttributeTableParameter.ContentType))
            {
                if (!std.Contains(CmsAttributes.ContentType))
                {
                    DerObjectIdentifier contentType = (DerObjectIdentifier)
                        parameters[CmsAttributeTableParameter.ContentType];
                    Attribute attr = new Attribute(CmsAttributes.ContentType,
                        new DerSet(contentType));
                    std[attr.AttrType] = attr;
                }
            }

            if (!std.Contains(CmsAttributes.SigningTime))
            {
                Attribute attr = new Attribute(CmsAttributes.SigningTime,
                    new DerSet(new Time(DateTime.UtcNow)));
                std[attr.AttrType] = attr;
            }

            if (!std.Contains(CmsAttributes.MessageDigest))
            {
                byte[] messageDigest = (byte[])parameters[CmsAttributeTableParameter.Digest];
                Attribute attr = new Attribute(CmsAttributes.MessageDigest,
                    new DerSet(new DerOctetString(messageDigest)));
                std[attr.AttrType] = attr;
            }
        }

        /**
		 * @param parameters source parameters
		 * @return the populated attribute table
		 */
		public virtual AttributeTable GetAttributes(
			IDictionary parameters)
		{
            IDictionary table = createStandardAttributeTable(parameters);
			return new AttributeTable(table);
		}
	}
}
