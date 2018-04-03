using System;
using System.Xml.Serialization;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.ObjectDetection.HaarCascade
{
    /// <summary>
    ///   Haar Cascade Serialization Root. This class is used
    ///   only for XML serialization/deserialization.
    /// </summary>
    /// 
    [Serializable]
    [XmlRoot(Namespace = "", IsNullable = false, ElementName = "stages")]
    public class HaarCascadeSerializationObject
    {
        /// <summary>
        ///   The stages retrieved after deserialization.
        /// </summary>
        [XmlElement("_")]
        public HaarCascadeStage[] Stages { get; set; }
    }
}