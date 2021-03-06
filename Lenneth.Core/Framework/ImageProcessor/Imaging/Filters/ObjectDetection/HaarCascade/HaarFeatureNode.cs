﻿using System;
using System.Xml.Serialization;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.ObjectDetection.HaarCascade
{
    /// <summary>
    ///   Haar Cascade Feature Tree Node.
    /// </summary>
    /// 
    /// <remarks>
    ///   The Feature Node is a node belonging to a feature tree,
    ///   containing information about child nodes and an associated 
    ///   <see cref="HaarFeature"/>.
    /// </remarks>
    /// 
    [Serializable]
    public class HaarFeatureNode : ICloneable
    {
        private int _rightNodeIndex = -1;
        private int _leftNodeIndex = -1;

        /// <summary>
        ///   Gets the threshold for this feature.
        /// </summary>
        /// 
        [XmlElement("threshold")]
        public double Threshold { get; set; }

        /// <summary>
        ///   Gets the left value for this feature.
        /// </summary>
        /// 
        [XmlElement("left_val")]
        public double LeftValue { get; set; }

        /// <summary>
        ///   Gets the right value for this feature.
        /// </summary>
        /// 
        [XmlElement("right_val")]
        public double RightValue { get; set; }

        /// <summary>
        ///   Gets the left node index for this feature.
        /// </summary>
        /// 
        [XmlElement("left_node")]
        public int LeftNodeIndex
        {
            get { return _leftNodeIndex; }
            set { _leftNodeIndex = value; }
        }

        /// <summary>
        ///   Gets the right node index for this feature.
        /// </summary>
        /// 
        [XmlElement("right_node")]
        public int RightNodeIndex
        {
            get { return _rightNodeIndex; }
            set { _rightNodeIndex = value; }
        }

        /// <summary>
        ///   Gets the feature associated with this node.
        /// </summary>
        /// 
        [XmlElement("feature", IsNullable = false)]
        public HaarFeature Feature { get; set; }

        /// <summary>
        ///   Constructs a new feature tree node.
        /// </summary>
        public HaarFeatureNode()
        {
        }

        /// <summary>
        ///   Constructs a new feature tree node.
        /// </summary>
        /// 
        public HaarFeatureNode(double threshold, double leftValue, double rightValue, params int[][] rectangles)
            : this(threshold, leftValue, rightValue, false, rectangles) { }

        /// <summary>
        ///   Constructs a new feature tree node.
        /// </summary>
        /// 
        public HaarFeatureNode(double threshold, double leftValue, double rightValue, bool tilted, params int[][] rectangles)
        {
            Feature = new HaarFeature(tilted, rectangles);
            Threshold = threshold;
            LeftValue = leftValue;
            RightValue = rightValue;
        }


        /// <summary>
        ///   Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        ///   A new object that is a copy of this instance.
        /// </returns>
        /// 
        public object Clone()
        {
            var r = new HaarFeatureNode();

            r.Feature = (HaarFeature)Feature.Clone();
            r.Threshold = Threshold;

            r.RightValue = RightValue;
            r.LeftValue = LeftValue;

            r.LeftNodeIndex = _leftNodeIndex;
            r.RightNodeIndex = _rightNodeIndex;

            return r;
        }
    }
}
