using System;
using System.Globalization;
using System.IO;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.ObjectDetection.HaarCascade
{
    /// <summary>
    ///   Automatic transcriber for Haar cascades.
    /// </summary>
    /// 
    /// <remarks>
    ///   This class can be used to generate code-only definitions for Haar cascades,
    ///   avoiding the need for loading and parsing XML files during application startup.
    ///   This class generates C# code for a class inheriting from <see cref="HaarCascade"/>
    ///   which may be used to create a <see cref="HaarObjectDetector"/>.
    /// </remarks>
    /// 
    public class HaarCascadeWriter
    {
        private TextWriter _writer;

        /// <summary>
        ///   Constructs a new <see cref="HaarCascadeWriter"/> class.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// 
        public HaarCascadeWriter(TextWriter stream)
        {
            _writer = stream;
        }

        /// <summary>
        ///   Writes the specified cascade.
        /// </summary>
        /// <param name="cascade">The cascade to write.</param>
        /// <param name="className">The name for the generated class.</param>
        /// 
        public void Write(HaarCascade cascade, string className)
        {
            for (var i = 0; i < cascade.Stages.Length; i++)
                for (var j = 0; j < cascade.Stages[i].Trees.Length; j++)
                    if (cascade.Stages[i].Trees[j].Length != 1)
                        throw new ArgumentException("Only cascades with single node trees are currently supported.");


            _writer.WriteLine("// This file has been automatically transcribed by the");
            _writer.WriteLine("//");
            _writer.WriteLine("// Accord Vision Library");
            _writer.WriteLine("// The Accord.NET Framework");
            _writer.WriteLine("// http://accord-framework.net");
            _writer.WriteLine("//");
            _writer.WriteLine();
            _writer.WriteLine("namespace HaarCascades");
            _writer.WriteLine("{");
            _writer.WriteLine("    using System.CodeDom.Compiler;");
            _writer.WriteLine("    using System.Collections.Generic;");
            _writer.WriteLine();
            _writer.WriteLine("    /// <summary>");
            _writer.WriteLine("    ///   Automatically generated haar-cascade definition");
            _writer.WriteLine("    ///   to use with the Accord.NET Framework object detectors.");
            _writer.WriteLine("    /// </summary>");
            _writer.WriteLine("    /// ");
            _writer.WriteLine("    [GeneratedCode(\"Accord.NET HaarCascadeWriter\", \"2.7\")]");
            _writer.WriteLine("    public class {0} : Accord.Vision.Detection.HaarCascade", className);
            _writer.WriteLine("    {");
            _writer.WriteLine();
            _writer.WriteLine("        /// <summary>");
            _writer.WriteLine("        ///   Automatically generated transcription");
            _writer.WriteLine("        /// </summary>");
            _writer.WriteLine("        public {0}()", className);
            _writer.WriteLine("            : base({0}, {1})", cascade.Width, cascade.Height);
            _writer.WriteLine("        {");
            _writer.WriteLine("            List<HaarCascadeStage> stages = new List<HaarCascadeStage>();");
            _writer.WriteLine("            List<HaarFeatureNode[]> nodes;");
            _writer.WriteLine("            HaarCascadeStage stage;");
            _writer.WriteLine();

            if (cascade.HasTiltedFeatures)
            {
                _writer.WriteLine("            HasTiltedFeatures = true;");
                _writer.WriteLine();
            }

            // Write cascade stages
            for (var i = 0; i < cascade.Stages.Length; i++)
                WriteStage(i, cascade.Stages[i]);

            _writer.WriteLine();
            _writer.WriteLine("            Stages = stages.ToArray();");
            _writer.WriteLine("         }");
            _writer.WriteLine("    }");
            _writer.WriteLine("}");
        }

        private void WriteStage(int i, HaarCascadeStage stage)
        {
            _writer.WriteLine("            #region Stage {0}", i);
            _writer.WriteLine("            stage = new HaarCascadeStage({0}, {1}, {2}); nodes = new List<HaarFeatureNode[]>();",
                stage.Threshold.ToString("R", NumberFormatInfo.InvariantInfo),
                stage.ParentIndex, stage.NextIndex);

            // Write stage trees
            for (var j = 0; j < stage.Trees.Length; j++)
                WriteTrees(stage, j);

            _writer.WriteLine("            stage.Trees = nodes.ToArray(); stages.Add(stage);");
            _writer.WriteLine("            #endregion");
            _writer.WriteLine();
        }

        private void WriteTrees(HaarCascadeStage stage, int j)
        {
            _writer.Write("            nodes.Add(new[] { ");

            // Assume trees have single node
            WriteFeature(stage.Trees[j][0]);

            _writer.WriteLine(" });");
        }

        private void WriteFeature(HaarFeatureNode node)
        {

            _writer.Write("new HaarFeatureNode({0}, {1}, {2}, ",
                node.Threshold.ToString("R", NumberFormatInfo.InvariantInfo),
                node.LeftValue.ToString("R", NumberFormatInfo.InvariantInfo),
                node.RightValue.ToString("R", NumberFormatInfo.InvariantInfo));

            if (node.Feature.Tilted)
                _writer.Write("true, ");

            // Write Haar-like rectangular features
            for (var k = 0; k < node.Feature.Rectangles.Length; k++)
            {
                WriteRectangle(node.Feature.Rectangles[k]);

                if (k < node.Feature.Rectangles.Length - 1)
                    _writer.Write(", ");
            }

            _writer.Write(" )");
        }

        private void WriteRectangle(HaarRectangle rectangle)
        {
            _writer.Write("new int[] {{ {0}, {1}, {2}, {3}, {4} }}",
                rectangle.X.ToString(NumberFormatInfo.InvariantInfo),
                rectangle.Y.ToString(NumberFormatInfo.InvariantInfo),
                rectangle.Width.ToString(NumberFormatInfo.InvariantInfo),
                rectangle.Height.ToString(NumberFormatInfo.InvariantInfo),
                rectangle.Weight.ToString("R", NumberFormatInfo.InvariantInfo));
        }
    }
}
