// Accord Vision Library
// The Accord.NET Framework
// http://accord-framework.net
//
// Copyright © César Souza, 2009-2015
// cesarsouza at gmail.com
//
// This code has been submitted as an user contribution by darko.juric2
// GCode Issue #12 https://code.google.com/p/accord/issues/detail?id=12
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

using System;
using System.Collections.Generic;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.ObjectDetection
{
    /// <summary>
    ///   Group matching algorithm for detection region averaging.
    /// </summary>
    /// 
    /// <remarks>
    ///   This class can be seen as a post-processing filter. Its goal is to
    ///   group near or contained regions together in order to produce more
    ///   robust and smooth estimates of the detected regions.
    /// </remarks>
    /// 
    public abstract class GroupMatching<T>
    {

        private int _classCount;
        private int _minNeighbors;

        private int[] _labels;
        private int[] _equals;

        private List<T> _filter;

        /// <summary>
        ///   Creates a new <see cref="RectangleGroupMatching"/> object.
        /// </summary>
        /// 
        /// <param name="minimumNeighbors">
        ///   The minimum number of neighbors needed to keep a detection. If a rectangle
        ///   has less than this minimum number, it will be discarded as a false positive.</param>
        /// 
        protected GroupMatching(int minimumNeighbors = 2)
        {
            _minNeighbors = minimumNeighbors;
            _filter = new List<T>();
        }

        /// <summary>
        ///   Gets or sets the minimum number of neighbors necessary to keep a detection.
        ///   If a rectangle has less neighbors than this number, it will be discarded as
        ///   a false positive.
        /// </summary>
        /// 
        public int MinimumNeighbors
        {
            get { return _minNeighbors; }
            set
            {
                if (_minNeighbors < 0)
                    throw new ArgumentOutOfRangeException("value", "Value must be equal to or higher than zero.");
                _minNeighbors = value;
            }
        }

        /// <summary>
        ///   Gets how many classes were found in the
        ///   last call to <see cref="Group(T[])"/>.
        /// </summary>
        /// 
        public int Classes
        {
            get { return _classCount; }
        }

        /// <summary>
        ///   Groups possibly near rectangles into a smaller
        ///   set of distinct and averaged rectangles.
        /// </summary>
        /// 
        /// <param name="shapes">The rectangles to group.</param>
        /// 
        public T[] Group(T[] shapes)
        {
            // Start by classifying rectangles according to distance
            Classify(shapes); // assign label for near rectangles

            int[] neighborCount;

            // Average the rectangles contained in each labeled group
            var output = Average(_labels, shapes, out neighborCount);

            // Check suppression
            if (_minNeighbors > 0)
            {
                _filter.Clear();

                // Discard weak rectangles which don't have enough neighbors
                for (var i = 0; i < output.Length; i++)
                    if (neighborCount[i] >= _minNeighbors) _filter.Add(output[i]);

                return _filter.ToArray();
            }

            return output;
        }



        /// <summary>
        ///   Detects rectangles which are near and 
        ///   assigns similar class labels accordingly.
        /// </summary>
        /// 
        private void Classify(T[] shapes)
        {
            _equals = new int[shapes.Length];
            for (var i = 0; i < _equals.Length; i++)
                _equals[i] = -1;

            _labels = new int[shapes.Length];
            for (var i = 0; i < _labels.Length; i++)
                _labels[i] = i;

            _classCount = 0;

            // If two rectangles are near, or contained in
            // each other, merge then in a single rectangle
            for (var i = 0; i < shapes.Length - 1; i++)
            {
                for (var j = i + 1; j < shapes.Length; j++)
                {
                    if (Near(shapes[i], shapes[j]))
                        Merge(_labels[i], _labels[j]);
                }
            }

            // Count the number of classes and centroids
            var centroids = new int[shapes.Length];
            for (var i = 0; i < centroids.Length; i++)
                if (_equals[i] == -1) centroids[i] = _classCount++;

            // Classify all rectangles with their labels
            for (var i = 0; i < shapes.Length; i++)
            {
                var root = _labels[i];
                while (_equals[root] != -1)
                    root = _equals[root];

                _labels[i] = centroids[root];
            }
        }

        /// <summary>
        ///   Merges two labels.
        /// </summary>
        /// 
        private void Merge(int label1, int label2)
        {
            var root1 = label1;
            var root2 = label2;

            // Get the roots associated with the two labels
            while (_equals[root1] != -1) root1 = _equals[root1];
            while (_equals[root2] != -1) root2 = _equals[root2];

            if (root1 == root2) // labels are already connected
                return;

            int minRoot, maxRoot;
            int labelWithMaxRoot;

            if (root1 > root2)
            {
                maxRoot = root1;
                minRoot = root2;

                labelWithMaxRoot = label1;
            }
            else
            {
                maxRoot = root2;
                minRoot = root1;

                labelWithMaxRoot = label2;
            }

            _equals[maxRoot] = minRoot;

            for (var root = maxRoot + 1; root <= labelWithMaxRoot; root++)
            {
                if (_equals[root] == maxRoot)
                    _equals[root] = minRoot;
            }
        }

        /// <summary>
        ///   When overridden in a child class, should compute
        ///   whether two given shapes are near. Definition of
        ///   near is up to the implementation.
        /// </summary>
        /// 
        /// <returns>True if the two shapes are near; false otherwise.</returns>
        /// 
        protected abstract bool Near(T shape1, T shape2);

        /// <summary>
        ///   When overridden in a child class, should compute
        ///   an average of the shapes given as parameters.
        /// </summary>
        /// 
        /// <param name="labels">The label of each shape.</param>
        /// <param name="shapes">The shapes themselves.</param>
        /// <param name="neighborCounts">Should return how many neighbors each shape had.</param>
        /// 
        /// <returns>The averaged shapes found in the given parameters.</returns>
        /// 
        protected abstract T[] Average(int[] labels, T[] shapes, out int[] neighborCounts);
    }
}