﻿using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.ObjectDetection.HaarCascade
{
    /// <summary>
    ///   Cascade of Haar-like features' weak classification stages.
    /// </summary>
    /// 
    /// <remarks>
    /// <para>
    ///   The Viola-Jones object detection framework is the first object detection framework
    ///   to provide competitive object detection rates in real-time proposed in 2001 by Paul
    ///   Viola and Michael Jones. Although it can be trained to detect a variety of object
    ///   classes, it was motivated primarily by the problem of face detection.</para>
    ///   
    /// <para>
    ///   The implementation of this code has used Viola and Jones' original publication, the
    ///   OpenCV Library and the Marilena Project as reference. OpenCV is released under a BSD
    ///   license, it is free for both academic and commercial use. Please be aware that some
    ///   particular versions of the Haar object detection framework are patented by Viola and
    ///   Jones and may be subject to restrictions for use in commercial applications. </para>
    ///   
    ///  <para>
    ///     References:
    ///     <list type="bullet">
    ///       <item><description>
    ///         <a href="http://www.cs.utexas.edu/~grauman/courses/spring2007/395T/papers/viola_cvpr2001.pdf">
    ///         Viola, P. and Jones, M. (2001). Rapid Object Detection using a Boosted Cascade
    ///         of Simple Features.</a></description></item>
    ///       <item><description>
    ///         <a href="http://en.wikipedia.org/wiki/Viola-Jones_object_detection_framework">
    ///         Wikipedia, The Free Encyclopedia. Viola-Jones object detection framework </a>
    ///       </description></item>
    ///     </list></para>
    /// </remarks>
    /// 
    /// <example>
    /// <para>
    ///   To load an OpenCV-compatible XML definition for a Haar cascade, you can use HaarCascade's
    ///   <see cref="HaarCascade.FromXml(Stream)">FromXml</see> static method. An example would be:</para>
    ///   <code>
    ///   String path = @"C:\Users\haarcascade-frontalface_alt2.xml";
    ///   HaarCascade cascade1 = HaarCascade.FromXml(path);
    ///   </code>
    ///   
    /// <para>
    ///   After the cascade has been loaded, it is possible to create a new <see cref="HaarObjectDetector"/>
    ///   using the cascade. Please see <see cref="HaarObjectDetector"/> for more details. It is also
    ///   possible to generate embeddable C# definitions from a cascade, avoiding the need to load
    ///   XML files on program startup. Please see <see cref="ToCode(string, string)"/> method or 
    ///   <see cref="HaarCascadeWriter"/> class for details.</para> 
    /// </example>
    /// 
    [Serializable]
    public class HaarCascade : ICloneable
    {
        /// <summary>
        ///   Gets the stages' base width.
        /// </summary>
        /// 
        public int Width { get; protected set; }

        /// <summary>
        ///   Gets the stages' base height.
        /// </summary>
        /// 
        public int Height { get; protected set; }

        /// <summary>
        ///   Gets the classification stages.
        /// </summary>
        /// 
        public HaarCascadeStage[] Stages { get; protected set; }

        /// <summary>
        ///   Gets a value indicating whether this cascade has tilted features.
        /// </summary>
        /// 
        /// <value>
        /// 	<c>true</c> if this cascade has tilted features; otherwise, <c>false</c>.
        /// </value>
        /// 
        public bool HasTiltedFeatures { get; protected set; }

        /// <summary>
        ///   Constructs a new Haar Cascade.
        /// </summary>
        /// 
        /// <param name="baseWidth">Base feature width.</param>
        /// <param name="baseHeight">Base feature height.</param>
        /// <param name="stages">Haar-like features classification stages.</param>
        /// 
        public HaarCascade(int baseWidth, int baseHeight, HaarCascadeStage[] stages)
        {
            Width = baseWidth;
            Height = baseHeight;
            Stages = stages;

            // check if the classifier has tilted features
            HasTiltedFeatures = CheckTiltedFeatures(stages);
        }

        /// <summary>
        ///   Constructs a new Haar Cascade.
        /// </summary>
        /// 
        /// <param name="baseWidth">Base feature width.</param>
        /// <param name="baseHeight">Base feature height.</param>
        /// 
        protected HaarCascade(int baseWidth, int baseHeight)
        {
            Width = baseWidth;
            Height = baseHeight;
        }


        /// <summary>
        ///   Checks if the classifier contains tilted (rotated) features
        /// </summary>
        /// 
        private static bool CheckTiltedFeatures(HaarCascadeStage[] stages)
        {
            return stages.Any(stage => stage.Trees.Any(tree => tree.Any(node => node.Feature.Tilted)));
        }

        /// <summary>
        ///   Creates a new object that is a copy of the current instance.
        /// </summary>
        /// 
        /// <returns>
        ///   A new object that is a copy of this instance.
        /// </returns>
        /// 
        public object Clone()
        {
            var newStages = new HaarCascadeStage[Stages.Length];
            for (var i = 0; i < newStages.Length; i++)
                newStages[i] = (HaarCascadeStage)Stages[i].Clone();

            var r = new HaarCascade(Width, Height);
            r.HasTiltedFeatures = HasTiltedFeatures;
            r.Stages = newStages;

            return r;
        }


        /// <summary>
        ///   Loads a HaarCascade from a OpenCV-compatible XML file.
        /// </summary>
        /// 
        /// <param name="stream">
        ///    A <see cref="Stream"/> containing the file stream
        ///    for the xml definition of the classifier to be loaded.</param>
        ///    
        /// <returns>The HaarCascadeClassifier loaded from the file.</returns>
        /// 
        public static HaarCascade FromXml(Stream stream)
        {
            return FromXml(new StreamReader(stream));
        }

        /// <summary>
        ///   Loads a HaarCascade from a OpenCV-compatible XML file.
        /// </summary>
        /// 
        /// <param name="path">
        ///    The file path for the xml definition of the classifier to be loaded.</param>
        ///    
        /// <returns>The HaarCascadeClassifier loaded from the file.</returns>
        /// 
        public static HaarCascade FromXml(string path)
        {
            return FromXml(new StreamReader(path));
        }

        /// <summary>
        ///   Loads a HaarCascade from a OpenCV-compatible XML file.
        /// </summary>
        /// 
        /// <param name="stringReader">
        ///    A <see cref="StringReader"/> containing the file stream
        ///    for the xml definition of the classifier to be loaded.</param>
        ///    
        /// <returns>The HaarCascadeClassifier loaded from the file.</returns>
        /// 
        public static HaarCascade FromXml(TextReader stringReader)
        {
            var xmlReader = new XmlTextReader(stringReader);

            // Gathers the base window size
            xmlReader.ReadToFollowing("size");
            var size = xmlReader.ReadElementContentAsString();
            //xmlReader.ReadToFollowing("height");
            //int baseHeight = int.Parse(xmlReader.ReadElementContentAsString().Trim(), CultureInfo.InvariantCulture);

            //xmlReader.ReadToFollowing("width");
            //int baseWidth = int.Parse(xmlReader.ReadElementContentAsString().Trim(), CultureInfo.InvariantCulture);


            // Proceeds to load the cascade stages
            xmlReader.ReadToFollowing("stages");
            var serializer = new XmlSerializer(typeof(HaarCascadeSerializationObject));
            var stages = (HaarCascadeSerializationObject)serializer.Deserialize(xmlReader);

            // Process base window size
            var s = size.Trim().Split(' ');
            var baseWidth = int.Parse(s[0], CultureInfo.InvariantCulture);
            var baseHeight = int.Parse(s[1], CultureInfo.InvariantCulture);

            // Create and return the new cascade
            return new HaarCascade(baseWidth, baseHeight, stages.Stages);
        }

        /// <summary>
        ///   Saves a HaarCascade to C# code.
        /// </summary>
        /// 
        public void ToCode(string path, string className)
        {
            ToCode(new StreamWriter(path), className);
        }

        /// <summary>
        ///   Saves a HaarCascade to C# code.
        /// </summary>
        /// 
        public void ToCode(TextWriter textWriter, string className)
        {
            new HaarCascadeWriter(textWriter).Write(this, className);
        }

    }
}
