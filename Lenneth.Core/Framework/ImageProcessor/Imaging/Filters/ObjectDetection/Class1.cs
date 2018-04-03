namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.ObjectDetection
{

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class opencv_storage
    {

        private opencv_storageCascade cascadeField;

        /// <remarks/>
        public opencv_storageCascade cascade
        {
            get
            {
                return cascadeField;
            }
            set
            {
                cascadeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class opencv_storageCascade
    {

        private string stageTypeField;

        private string featureTypeField;

        private byte heightField;

        private byte widthField;

        private opencv_storageCascadeStageParams stageParamsField;

        private opencv_storageCascadeFeatureParams featureParamsField;

        private byte stageNumField;

        private opencv_storageCascade_[] stagesField;

        private opencv_storageCascade_2[] featuresField;

        private string type_idField;

        /// <remarks/>
        public string stageType
        {
            get
            {
                return stageTypeField;
            }
            set
            {
                stageTypeField = value;
            }
        }

        /// <remarks/>
        public string featureType
        {
            get
            {
                return featureTypeField;
            }
            set
            {
                featureTypeField = value;
            }
        }

        /// <remarks/>
        public byte height
        {
            get
            {
                return heightField;
            }
            set
            {
                heightField = value;
            }
        }

        /// <remarks/>
        public byte width
        {
            get
            {
                return widthField;
            }
            set
            {
                widthField = value;
            }
        }

        /// <remarks/>
        public opencv_storageCascadeStageParams stageParams
        {
            get
            {
                return stageParamsField;
            }
            set
            {
                stageParamsField = value;
            }
        }

        /// <remarks/>
        public opencv_storageCascadeFeatureParams featureParams
        {
            get
            {
                return featureParamsField;
            }
            set
            {
                featureParamsField = value;
            }
        }

        /// <remarks/>
        public byte stageNum
        {
            get
            {
                return stageNumField;
            }
            set
            {
                stageNumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("_", IsNullable = false)]
        public opencv_storageCascade_[] stages
        {
            get
            {
                return stagesField;
            }
            set
            {
                stagesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("_", IsNullable = false)]
        public opencv_storageCascade_2[] features
        {
            get
            {
                return featuresField;
            }
            set
            {
                featuresField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type_id
        {
            get
            {
                return type_idField;
            }
            set
            {
                type_idField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class opencv_storageCascadeStageParams
    {

        private byte maxWeakCountField;

        /// <remarks/>
        public byte maxWeakCount
        {
            get
            {
                return maxWeakCountField;
            }
            set
            {
                maxWeakCountField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class opencv_storageCascadeFeatureParams
    {

        private byte maxCatCountField;

        /// <remarks/>
        public byte maxCatCount
        {
            get
            {
                return maxCatCountField;
            }
            set
            {
                maxCatCountField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class opencv_storageCascade_
    {

        private byte maxWeakCountField;

        private double stageThresholdField;

        private opencv_storageCascade__[] weakClassifiersField;

        /// <remarks/>
        public byte maxWeakCount
        {
            get
            {
                return maxWeakCountField;
            }
            set
            {
                maxWeakCountField = value;
            }
        }

        /// <remarks/>
        public double stageThreshold
        {
            get
            {
                return stageThresholdField;
            }
            set
            {
                stageThresholdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("_", IsNullable = false)]
        public opencv_storageCascade__[] weakClassifiers
        {
            get
            {
                return weakClassifiersField;
            }
            set
            {
                weakClassifiersField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class opencv_storageCascade__
    {

        private string internalNodesField;

        private string leafValuesField;

        /// <remarks/>
        public string internalNodes
        {
            get
            {
                return internalNodesField;
            }
            set
            {
                internalNodesField = value;
            }
        }

        /// <remarks/>
        public string leafValues
        {
            get
            {
                return leafValuesField;
            }
            set
            {
                leafValuesField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class opencv_storageCascade_2
    {

        private string[] rectsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("_", IsNullable = false)]
        public string[] rects
        {
            get
            {
                return rectsField;
            }
            set
            {
                rectsField = value;
            }
        }
    }


}
