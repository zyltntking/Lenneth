// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OctreeQuantizer.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Encapsulates methods to calculate the colour palette if an image using an Octree pattern.
//   <see href="http://msdn.microsoft.com/en-us/library/aa479306.aspx" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Lenneth.Core.Framework.ImageProcessor.Common.Extensions;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Colors;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Quantizers
{
    /// <summary>
    /// Encapsulates methods to calculate the colour palette if an image using an Octree pattern.
    /// <see href="http://msdn.microsoft.com/en-us/library/aa479306.aspx"/>
    /// </summary>
    public unsafe class OctreeQuantizer : Quantizer
    {
        /// <summary>
        /// Maximum allowed color depth
        /// </summary>
        private readonly int maxColors;

        /// <summary>
        /// Maximum allowed color bit depth
        /// </summary>
        private readonly int maxColorBits;

        /// <summary>
        /// Stores the tree
        /// </summary>
        private Octree octree;

        /// <summary>
        /// The transparency threshold.
        /// </summary>
        private byte threshold = 64;

        /// <summary>
        /// Initializes a new instance of the <see cref="OctreeQuantizer"/> class.
        /// </summary>
        /// <remarks>
        /// The Octree quantizer is a two pass algorithm. The initial pass sets up the Octree,
        /// the second pass quantizes a color based on the nodes in the tree.
        /// <para>
        /// Defaults to return a maximum of 255 colors plus transparency with 8 significant bits.
        /// </para>
        /// </remarks>
        public OctreeQuantizer()
            : this(255, 8)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OctreeQuantizer"/> class.
        /// </summary>
        /// <remarks>
        /// The Octree quantizer is a two pass algorithm. The initial pass sets up the Octree,
        /// the second pass quantizes a color based on the nodes in the tree
        /// </remarks>
        /// <param name="maxColors">
        /// The maximum number of colors to return
        /// </param>
        /// <param name="maxColorBits">
        /// The number of significant bits
        /// </param>
        public OctreeQuantizer(int maxColors, int maxColorBits)
            : base(false)
        {
            if (maxColors > 255)
            {
                throw new ArgumentOutOfRangeException(nameof(maxColors), maxColors, "The number of colors should be less than 256");
            }

            if ((maxColorBits < 1) | (maxColorBits > 8))
            {
                throw new ArgumentOutOfRangeException(nameof(maxColorBits), maxColorBits, "This should be between 1 and 8");
            }

            this.maxColors = maxColors;
            this.maxColorBits = maxColorBits;
        }

        /// <summary>
        /// Gets or sets the transparency threshold (0 - 255)
        /// </summary>
        public byte Threshold
        {
            get
            {
                return threshold;
            }

            set
            {
                threshold = value;
            }
        }

        /// <summary>
        /// Execute the first pass through the pixels in the image
        /// </summary>
        /// <param name="sourceData">The source data</param>
        /// <param name="width">The width in pixels of the image</param>
        /// <param name="height">The height in pixels of the image</param>
        protected override void FirstPass(BitmapData sourceData, int width, int height)
        {
            // Construct the Octree
            octree = new Octree(maxColorBits);

            base.FirstPass(sourceData, width, height);
        }

        /// <summary>
        /// Process the pixel in the first pass of the algorithm
        /// </summary>
        /// <param name="pixel">
        /// The pixel to quantize
        /// </param>
        /// <remarks>
        /// This function need only be overridden if your quantize algorithm needs two passes,
        /// such as an Octree quantizer.
        /// </remarks>
        protected override void InitialQuantizePixel(Color32* pixel)
        {
            // Add the color to the Octree
            octree.AddColor(pixel);
        }

        /// <summary>
        /// Override this to process the pixel in the second pass of the algorithm
        /// </summary>
        /// <param name="pixel">
        /// The pixel to quantize
        /// </param>
        /// <returns>
        /// The quantized value
        /// </returns>
        protected override byte QuantizePixel(Color32* pixel)
        {
            // The color at [maxColors] is set to transparent
            var paletteIndex = (byte)maxColors;

            // Get the palette index if this non-transparent
            if (pixel->A > threshold)
            {
                paletteIndex = (byte)octree.GetPaletteIndex(pixel);
            }

            return paletteIndex;
        }

        /// <summary>
        /// Retrieve the palette for the quantized image
        /// </summary>
        /// <param name="original">
        /// Any old palette, this is overwritten
        /// </param>
        /// <returns>
        /// The new color palette
        /// </returns>
        protected override ColorPalette GetPalette(ColorPalette original)
        {
            // Clear out the original pallete
            for (var i = 0; i < original.Entries.Length; i++)
            {
                original.Entries[i] = Color.FromArgb(0, 0, 0, 0);
            }

            // First off convert the Octree to maxColors colors
            var palette = octree.Palletize(Math.Max(maxColors - 1, 1));

            // Then convert the palette based on those colors
            for (var index = 0; index < palette.Count; index++)
            {
                original.Entries[index] = palette[index];
            }

            // Add the transparent color
            original.Entries[maxColors] = Color.FromArgb(0, 0, 0, 0);

            return original;
        }

        /// <summary>
        /// Class which does the actual quantization
        /// </summary>
        private class Octree
        {
            /// <summary>
            /// Mask used when getting the appropriate pixels for a given node
            /// </summary>
            private static readonly int[] Mask = { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };

            /// <summary>
            /// The root of the Octree
            /// </summary>
            private readonly OctreeNode root;

            /// <summary>
            /// Array of reducible nodes
            /// </summary>
            private readonly OctreeNode[] reducibleNodes;

            /// <summary>
            /// Maximum number of significant bits in the image
            /// </summary>
            private readonly int maxColorBits;

            /// <summary>
            /// Number of leaves in the tree
            /// </summary>
            private int leafCount;

            /// <summary>
            /// Store the last node quantized
            /// </summary>
            private OctreeNode previousNode;

            /// <summary>
            /// Cache the previous color quantized
            /// </summary>
            private int previousColor;

            /// <summary>
            /// Initializes a new instance of the <see cref="Octree"/> class.
            /// </summary>
            /// <param name="maxColorBits">
            /// The maximum number of significant bits in the image
            /// </param>
            public Octree(int maxColorBits)
            {
                this.maxColorBits = maxColorBits;
                leafCount = 0;
                reducibleNodes = new OctreeNode[9];
                root = new OctreeNode(0, this.maxColorBits, this);
                previousColor = 0;
                previousNode = null;
            }

            /// <summary>
            /// Gets or sets the number of leaves in the tree
            /// </summary>
            private int Leaves
            {
                get { return leafCount; }
                set { leafCount = value; }
            }

            /// <summary>
            /// Gets the array of reducible nodes
            /// </summary>
            private OctreeNode[] ReducibleNodes => reducibleNodes;

            /// <summary>
            /// Add a given color value to the Octree
            /// </summary>
            /// <param name="pixel">
            /// The <see cref="Color32"/>containing color information to add.
            /// </param>
            public void AddColor(Color32* pixel)
            {
                // Check if this request is for the same color as the last
                if (previousColor == pixel->Argb)
                {
                    // If so, check if I have a previous node setup. This will only occur if the first color in the image
                    // happens to be black, with an alpha component of zero.
                    if (previousNode == null)
                    {
                        previousColor = pixel->Argb;
                        root.AddColor(pixel, maxColorBits, 0, this);
                    }
                    else
                    {
                        // Just update the previous node
                        previousNode.Increment(pixel);
                    }
                }
                else
                {
                    previousColor = pixel->Argb;
                    root.AddColor(pixel, maxColorBits, 0, this);
                }
            }

            /// <summary>
            /// Convert the nodes in the Octree to a palette with a maximum of colorCount colors
            /// </summary>
            /// <param name="colorCount">
            /// The maximum number of colors
            /// </param>
            /// <returns>
            /// An <see cref="List{Color}"/> with the palletized colors
            /// </returns>
            public List<Color> Palletize(int colorCount)
            {
                while (Leaves > colorCount)
                {
                    Reduce();
                }

                // Now palletize the nodes
                var palette = new List<Color>(Leaves);
                var paletteIndex = 0;
                root.ConstructPalette(palette, ref paletteIndex);

                // And return the palette
                return palette;
            }

            /// <summary>
            /// Get the palette index for the passed color
            /// </summary>
            /// <param name="pixel">
            /// The <see cref="Color32"/> containing the pixel data.
            /// </param>
            /// <returns>
            /// The index of the given structure.
            /// </returns>
            public int GetPaletteIndex(Color32* pixel)
            {
                return root.GetPaletteIndex(pixel, 0);
            }

            /// <summary>
            /// Keep track of the previous node that was quantized
            /// </summary>
            /// <param name="node">
            /// The node last quantized
            /// </param>
            protected void TrackPrevious(OctreeNode node)
            {
                previousNode = node;
            }

            /// <summary>
            /// Reduce the depth of the tree
            /// </summary>
            private void Reduce()
            {
                // Find the deepest level containing at least one reducible node
                var index = maxColorBits - 1;
                while ((index > 0) && (reducibleNodes[index] == null))
                {
                    index--;
                }

                // Reduce the node most recently added to the list at level 'index'
                var node = reducibleNodes[index];
                reducibleNodes[index] = node.NextReducible;

                // Decrement the leaf count after reducing the node
                leafCount -= node.Reduce();

                // And just in case I've reduced the last color to be added, and the next color to
                // be added is the same, invalidate the previousNode...
                previousNode = null;
            }

            /// <summary>
            /// Class which encapsulates each node in the tree
            /// </summary>
            protected class OctreeNode
            {
                /// <summary>
                /// Pointers to any child nodes
                /// </summary>
                private readonly OctreeNode[] children;

                /// <summary>
                /// Flag indicating that this is a leaf node
                /// </summary>
                private bool leaf;

                /// <summary>
                /// Number of pixels in this node
                /// </summary>
                private int pixelCount;

                /// <summary>
                /// Red component
                /// </summary>
                private int red;

                /// <summary>
                /// Green Component
                /// </summary>
                private int green;

                /// <summary>
                /// Blue component
                /// </summary>
                private int blue;

                /// <summary>
                /// The index of this node in the palette
                /// </summary>
                private int paletteIndex;

                /// <summary>
                /// Initializes a new instance of the <see cref="OctreeNode"/> class.
                /// </summary>
                /// <param name="level">
                /// The level in the tree = 0 - 7
                /// </param>
                /// <param name="colorBits">
                /// The number of significant color bits in the image
                /// </param>
                /// <param name="octree">
                /// The tree to which this node belongs
                /// </param>
                public OctreeNode(int level, int colorBits, Octree octree)
                {
                    // Construct the new node
                    leaf = level == colorBits;

                    red = green = blue = 0;
                    pixelCount = 0;

                    // If a leaf, increment the leaf count
                    if (leaf)
                    {
                        octree.Leaves++;
                        NextReducible = null;
                        children = null;
                    }
                    else
                    {
                        // Otherwise add this to the reducible nodes
                        NextReducible = octree.ReducibleNodes[level];
                        octree.ReducibleNodes[level] = this;
                        children = new OctreeNode[8];
                    }
                }

                /// <summary>
                /// Gets the next reducible node
                /// </summary>
                public OctreeNode NextReducible { get; }

                /// <summary>
                /// Add a color into the tree
                /// </summary>
                /// <param name="pixel">
                /// The color
                /// </param>
                /// <param name="colorBits">
                /// The number of significant color bits
                /// </param>
                /// <param name="level">
                /// The level in the tree
                /// </param>
                /// <param name="octree">
                /// The tree to which this node belongs
                /// </param>
                public void AddColor(Color32* pixel, int colorBits, int level, Octree octree)
                {
                    // Update the color information if this is a leaf
                    if (leaf)
                    {
                        Increment(pixel);

                        // Setup the previous node
                        octree.TrackPrevious(this);
                    }
                    else
                    {
                        // Go to the next level down in the tree
                        var shift = 7 - level;
                        var index = ((pixel->R & Mask[level]) >> (shift - 2)) |
                                    ((pixel->G & Mask[level]) >> (shift - 1)) |
                                    ((pixel->B & Mask[level]) >> shift);

                        var child = children[index];

                        if (child == null)
                        {
                            // Create a new child node & store in the array
                            child = new OctreeNode(level + 1, colorBits, octree);
                            children[index] = child;
                        }

                        // Add the color to the child node
                        child.AddColor(pixel, colorBits, level + 1, octree);
                    }
                }

                /// <summary>
                /// Reduce this node by removing all of its children
                /// </summary>
                /// <returns>The number of leaves removed</returns>
                public int Reduce()
                {
                    red = green = blue = 0;
                    var childNodes = 0;

                    // Loop through all children and add their information to this node
                    for (var index = 0; index < 8; index++)
                    {
                        if (children[index] != null)
                        {
                            red += children[index].red;
                            green += children[index].green;
                            blue += children[index].blue;
                            pixelCount += children[index].pixelCount;
                            ++childNodes;
                            children[index] = null;
                        }
                    }

                    // Now change this to a leaf node
                    leaf = true;

                    // Return the number of nodes to decrement the leaf count by
                    return childNodes - 1;
                }

                /// <summary>
                /// Traverse the tree, building up the color palette
                /// </summary>
                /// <param name="palette">
                /// The palette
                /// </param>
                /// <param name="index">
                /// The current palette index
                /// </param>
                public void ConstructPalette(List<Color> palette, ref int index)
                {
                    if (leaf)
                    {
                        // Consume the next palette index
                        paletteIndex = index++;

                        var r = (red / pixelCount).ToByte();
                        var g = (green / pixelCount).ToByte();
                        var b = (blue / pixelCount).ToByte();

                        // And set the color of the palette entry
                        palette.Add(Color.FromArgb(r, g, b));
                    }
                    else
                    {
                        // Loop through children looking for leaves
                        for (var i = 0; i < 8; i++)
                        {
                            if (children[i] != null)
                            {
                                children[i].ConstructPalette(palette, ref index);
                            }
                        }
                    }
                }

                /// <summary>
                /// Return the palette index for the passed color
                /// </summary>
                /// <param name="pixel">
                /// The <see cref="Color32"/> representing the pixel.
                /// </param>
                /// <param name="level">
                /// The level.
                /// </param>
                /// <returns>
                /// The <see cref="int"/> representing the index of the pixel in the palette.
                /// </returns>
                public int GetPaletteIndex(Color32* pixel, int level)
                {
                    var index = paletteIndex;

                    if (!leaf)
                    {
                        var shift = 7 - level;
                        var pixelIndex = ((pixel->R & Mask[level]) >> (shift - 2)) |
                                    ((pixel->G & Mask[level]) >> (shift - 1)) |
                                    ((pixel->B & Mask[level]) >> shift);

                        if (children[pixelIndex] != null)
                        {
                            index = children[pixelIndex].GetPaletteIndex(pixel, level + 1);
                        }
                        else
                        {
                            throw new Exception("Didn't expect this!");
                        }
                    }

                    return index;
                }

                /// <summary>
                /// Increment the pixel count and add to the color information
                /// </summary>
                /// <param name="pixel">
                /// The pixel to add.
                /// </param>
                public void Increment(Color32* pixel)
                {
                    pixelCount++;
                    red += pixel->R;
                    green += pixel->G;
                    blue += pixel->B;
                }
            }
        }
    }
}
