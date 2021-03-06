﻿// Accord Vision Library
// The Accord.NET Framework (LGPL) 
// http://accord-framework.net
//
// Copyright © César Souza, 2009-2015
// cesarsouza at gmail.com
//
// Copyright © Masakazu Ohtsuka, 2008
//   This work is partially based on the original Project Marilena code,
//   distributed under a 2-clause BSD License. Details are listed below.
//
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
//   * Redistribution's of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//
//   * Redistribution's in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//
// This software is provided by the copyright holders and contributors "as is" and
// any express or implied warranties, including, but not limited to, the implied
// warranties of merchantability and fitness for a particular purpose are disclaimed.
// In no event shall the Intel Corporation or contributors be liable for any direct,
// indirect, incidental, special, exemplary, or consequential damages
// (including, but not limited to, procurement of substitute goods or services;
// loss of use, data, or profits; or business interruption) however caused
// and on any theory of liability, whether in contract, strict liability,
// or tort (including negligence or otherwise) arising in any way out of
// the use of this software, even if advised of the possibility of such damage.
//

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Lenneth.Core.Framework.ImageProcessor.Common.Extensions;
using Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.ObjectDetection.HaarCascade;

namespace Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.ObjectDetection
{
    /// <summary>
    ///   Viola-Jones Object Detector based on Haar-like features.
    /// </summary>
    /// <remarks>
    /// 
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
    ///   Jones and may be subject to restrictions for use in commercial applications. The code
    ///   has been implemented with full support for tilted Haar features from the ground up.</para>
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
    ///         http://en.wikipedia.org/wiki/Viola-Jones_object_detection_framework</a>
    ///       </description></item>
    ///     </list>
    ///   </para>
    /// </remarks>
    /// 
    public class HaarObjectDetector
    {

        private List<Rectangle> _detectedObjects;
        private HaarClassifier _classifier;

        private ObjectDetectorSearchMode _searchMode = ObjectDetectorSearchMode.NoOverlap;
        private ObjectDetectorScalingMode _scalingMode = ObjectDetectorScalingMode.GreaterToSmaller;

        // TODO: Support ROI
        //  private Rectangle searchWindow;

        private Size _minSize = new Size(15, 15);
        private Size _maxSize = new Size(500, 500);
        private float _factor = 1.2f;
       // private int channel = new Color32().R;

        private Rectangle[] _lastObjects;
        private int _steadyThreshold = 2;

        private int _baseWidth;
        private int _baseHeight;

        private int _lastWidth;
        private int _lastHeight;
        private float[] _steps;

        private RectangleGroupMatching _match;

        #region Constructors

        /// <summary>
        ///   Constructs a new Haar object detector.
        /// </summary>
        /// 
        /// <param name="cascade">
        ///   The <see cref="Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.ObjectDetection.HaarCascade"/> to use in the detector's classifier.
        ///   For the default face cascade, please take a look on
        ///   <see cref="Cascades.FaceHaarCascade"/>.
        /// </param>
        /// 
        public HaarObjectDetector(HaarCascade.HaarCascade cascade)
            : this(cascade, 15) { }

        /// <summary>
        ///   Constructs a new Haar object detector.
        /// </summary>
        /// 
        /// <param name="cascade">
        ///   The <see cref="Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.ObjectDetection.HaarCascade"/> to use in the detector's classifier.
        ///   For the default face cascade, please take a look on
        ///   <see cref="Cascades.FaceHaarCascade"/>.</param>
        /// <param name="minSize">
        ///   Minimum window size to consider when searching for 
        ///   objects. Default value is <c>15</c>.</param>
        /// 
        public HaarObjectDetector(HaarCascade.HaarCascade cascade, int minSize)
            : this(cascade, minSize, ObjectDetectorSearchMode.NoOverlap) { }

        /// <summary>
        ///   Constructs a new Haar object detector.
        /// </summary>
        /// 
        /// <param name="cascade">
        ///   The <see cref="Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.ObjectDetection.HaarCascade"/> to use in the detector's classifier.
        ///   For the default face cascade, please take a look on
        ///   <see cref="Cascades.FaceHaarCascade"/>.
        /// </param>
        /// <param name="minSize">
        ///   Minimum window size to consider when searching for
        ///   objects. Default value is <c>15</c>.</param>
        /// <param name="searchMode">The <see cref="ObjectDetectorSearchMode"/> to use
        ///   during search. Please see documentation of <see cref="ObjectDetectorSearchMode"/>
        ///   for details. Default value is <see cref="ObjectDetectorSearchMode.NoOverlap"/></param>
        /// 
        public HaarObjectDetector(HaarCascade.HaarCascade cascade, int minSize, ObjectDetectorSearchMode searchMode)
            : this(cascade, minSize, searchMode, 1.2f) { }

        /// <summary>
        ///   Constructs a new Haar object detector.
        /// </summary>
        /// 
        /// <param name="cascade">
        ///   The <see cref="Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.ObjectDetection.HaarCascade"/> to use in the detector's classifier.
        ///   For the default face cascade, please take a look on
        ///   <see cref="Cascades.FaceHaarCascade"/>.</param>
        /// <param name="minSize">
        ///   Minimum window size to consider when searching for
        ///   objects. Default value is <c>15</c>.</param>
        /// <param name="searchMode">
        ///   The <see cref="ObjectDetectorSearchMode"/> to use
        ///   during search. Please see documentation of <see cref="ObjectDetectorSearchMode"/>
        ///   for details. Default value is <see cref="ObjectDetectorSearchMode.NoOverlap"/></param>
        /// <param name="scaleFactor">The re-scaling factor to use when re-scaling the window during search.</param>
        /// 
        public HaarObjectDetector(HaarCascade.HaarCascade cascade, int minSize,
            ObjectDetectorSearchMode searchMode, float scaleFactor)
            : this(cascade, minSize, searchMode, scaleFactor, ObjectDetectorScalingMode.SmallerToGreater) { }

        /// <summary>
        ///   Constructs a new Haar object detector.
        /// </summary>
        /// 
        /// <param name="cascade">
        ///   The <see cref="Lenneth.Core.Framework.ImageProcessor.Imaging.Filters.ObjectDetection.HaarCascade"/> to use in the detector's classifier.
        ///   For the default face cascade, please take a look on
        ///   <see cref="Cascades.FaceHaarCascade"/>. </param>
        /// <param name="minSize">
        ///   Minimum window size to consider when searching for
        ///   objects. Default value is <c>15</c>.</param>
        /// <param name="searchMode">The <see cref="ObjectDetectorSearchMode"/> to use
        ///   during search. Please see documentation of <see cref="ObjectDetectorSearchMode"/>
        ///   for details. Default is <see cref="ObjectDetectorSearchMode.NoOverlap"/>.</param>
        /// <param name="scaleFactor">The scaling factor to rescale the window
        ///   during search. Default value is <c>1.2f</c>.</param>
        /// <param name="scalingMode">The <see cref="ObjectDetectorScalingMode"/> to use
        ///   when re-scaling the search window during search. Default is
        ///   <see cref="ObjectDetectorScalingMode.SmallerToGreater"/>.</param>
        /// 
        public HaarObjectDetector(HaarCascade.HaarCascade cascade, int minSize,
            ObjectDetectorSearchMode searchMode, float scaleFactor,
            ObjectDetectorScalingMode scalingMode)
        {
            _classifier = new HaarClassifier(cascade);
            this._minSize = new Size(minSize, minSize);
            this._searchMode = searchMode;
            ScalingMode = scalingMode;
            _factor = scaleFactor;
            _detectedObjects = new List<Rectangle>();

            _baseWidth = cascade.Width;
            _baseHeight = cascade.Height;

            _match = new RectangleGroupMatching(0, 0.2);
        }
        #endregion

        #region Properties
        /// <summary>
        ///   Minimum window size to consider when searching objects.
        /// </summary>
        /// 
        public Size MinSize
        {
            get { return _minSize; }
            set { _minSize = value; }
        }

        /// <summary>
        /// Maximum window size to consider when searching objects.
        /// </summary>
        public Size MaxSize
        {
            get { return _maxSize; }
            set { _maxSize = value; }
        }

        /// <summary>
        ///   Gets or sets the color channel to use when processing color images. 
        /// </summary>
        /// 
        //public int Channel
        //{
        //    get { return channel; }
        //    set { channel = value; }
        //}

        /// <summary>
        ///   Gets or sets the scaling factor to rescale the window during search.
        /// </summary>
        /// 
        public float ScalingFactor
        {
            get { return _factor; }
            set
            {
                if (value != _factor)
                {
                    _factor = value;
                    _steps = null;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the desired searching method.
        /// </summary>
        /// 
        public ObjectDetectorSearchMode SearchMode
        {
            get { return _searchMode; }
            set { _searchMode = value; }
        }

        /// <summary>
        ///   Gets or sets the desired scaling method.
        /// </summary>
        /// 
        public ObjectDetectorScalingMode ScalingMode
        {
            get { return _scalingMode; }
            set
            {
                if (value != _scalingMode)
                {
                    _scalingMode = value;
                    _steps = null;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the minimum threshold used to suppress rectangles which
        ///   have not been detected sufficient number of times. This property only
        ///   has effect when <see cref="SearchMode"/> is set to <see cref="ObjectDetectorSearchMode.Average"/>.
        /// </summary>
        /// 
        /// <remarks>
        /// <para>
        ///   The value of this property represents the minimum amount of detections
        ///   made inside a region to report this region as an actual detection. For
        ///   example, setting this property to two will discard all regions which 
        ///   had not achieved at least two detected rectangles within it.</para>
        ///   
        /// <para>
        ///   Setting this property to a value higher than zero may decrease the
        ///   number of false positives.</para>
        /// </remarks>
        /// 
        public int Suppression
        {
            get { return _match.MinimumNeighbors; }
            set { _match.MinimumNeighbors = value; }
        }

        /// <summary>
        ///   Gets the detected objects bounding boxes.
        /// </summary>
        /// 
        public Rectangle[] DetectedObjects
        {
            get { return _detectedObjects.ToArray(); }
        }

        /// <summary>
        ///   Gets the internal Cascade Classifier used by this detector.
        /// </summary>
        /// 
        public HaarClassifier Classifier
        {
            get { return _classifier; }
        }

        /// <summary>
        ///   Gets how many frames the object has
        ///   been detected in a steady position.
        /// </summary>
        /// <value>
        ///   The number of frames the detected object
        ///   has been in a steady position.</value>
        ///   
        public int Steady { get; private set; }

        #endregion


        /// <summary>
        ///   Performs object detection on the given frame.
        /// </summary>
        /// 
        //public Rectangle[] ProcessFrame(Bitmap frame)
        //{
        //    using (FastBitmap fastBitmap = new FastBitmap(frame))
        //    {
        //        return ProcessFrame(fastBitmap);
        //    }
        //}

        /// <summary>
        ///   Performs object detection on the given frame.
        /// </summary>
        /// 
        public Rectangle[] ProcessFrame(Bitmap image)
        {
          //  int colorChannel =
           //   image.PixelFormat == PixelFormat.Format8bppIndexed ? 0 : channel;

            Rectangle[] objects;

            // Creates an integral image representation of the frame
            using (var fastBitmap = new FastBitmap(image, _classifier.Cascade.HasTiltedFeatures))
            {
                // Creates a new list of detected objects.
                _detectedObjects.Clear();

                var width = fastBitmap.Width;
                var height = fastBitmap.Height;

                // Update parameters only if different size
                if (_steps == null || width != _lastWidth || height != _lastHeight)
                    Update(width, height);


                var window = Rectangle.Empty;

                // For each scaling step
                for (var i = 0; i < _steps.Length; i++)
                {
                    var scaling = _steps[i];

                    // Set the classifier window scale
                    _classifier.Scale = scaling;

                    // Get the scaled window size
                    window.Width = (int)(_baseWidth * scaling);
                    window.Height = (int)(_baseHeight * scaling);

                    // Check if the window is lesser than the minimum size
                    if (window.Width < _minSize.Width || window.Height < _minSize.Height)
                    {
                        // If we are searching in greater to smaller mode,
                        if (_scalingMode == ObjectDetectorScalingMode.GreaterToSmaller)
                        {
                            break; // it won't get bigger, so we should stop.
                        }
                        else continue; // continue until it gets greater.
                    }

                    // Check if the window is greater than the maximum size
                    else if (window.Width > _maxSize.Width || window.Height > _maxSize.Height)
                    {
                        // If we are searching in greater to smaller mode,
                        if (_scalingMode == ObjectDetectorScalingMode.GreaterToSmaller)
                        {
                            continue; // continue until it gets smaller.
                        }

                        break; // it won't get smaller, so we should stop. 
                    }

                    // Grab some scan loop parameters
                    var xStep = window.Width >> 3;
                    var yStep = window.Height >> 3;

                    var xEnd = width - window.Width;
                    var yEnd = height - window.Height;


                    // Parallel mode. Scan the integral image searching
                    // for objects in the window with parallelization.
                    var bag = new System.Collections.Concurrent.ConcurrentBag<Rectangle>();

                    var numSteps = (int)Math.Ceiling((double)yEnd / yStep);

                    // For each pixel in the window column
                    var window1 = window;
                    Parallel.For(
                        0,
                        numSteps,
                        (j, options) =>
                        {
                            var y = j * yStep;

                            // Create a local window reference
                            var localWindow = window1;

                            localWindow.Y = y;

                            // For each pixel in the window row
                            for (var x = 0; x < xEnd; x += xStep)
                            {
                                if (options.ShouldExitCurrentIteration) return;

                                localWindow.X = x;

                                // Try to detect and object inside the window
                                if (_classifier.Compute(fastBitmap, localWindow))
                                {
                                    // an object has been detected
                                    bag.Add(localWindow);

                                    if (_searchMode == ObjectDetectorSearchMode.Single)
                                        options.Stop();
                                }
                            }
                        });

                    // If required, avoid adding overlapping objects at
                    // the expense of extra computation. Otherwise, only
                    // add objects to the detected objects collection.
                    if (_searchMode == ObjectDetectorSearchMode.NoOverlap)
                    {
                        foreach (var obj in bag)
                        {
                            if (!Overlaps(obj))
                            {
                                _detectedObjects.Add(obj);
                            }
                        }
                    }
                    else if (_searchMode == ObjectDetectorSearchMode.Single)
                    {
                        if (bag.TryPeek(out window))
                        {
                            _detectedObjects.Add(window);
                            break;
                        }
                    }
                    else
                    {
                        foreach (var obj in bag)
                        {
                            _detectedObjects.Add(obj);
                        }
                    }
                }
            }

            objects = _detectedObjects.ToArray();

            if (_searchMode == ObjectDetectorSearchMode.Average)
            {
                objects = _match.Group(objects);
            }

            CheckSteadiness(objects);
            _lastObjects = objects;

            return objects; // Returns the array of detected objects.
        }

        private void Update(int width, int height)
        {
            var listSteps = new List<float>();

            // Set initial parameters according to scaling mode
            if (_scalingMode == ObjectDetectorScalingMode.SmallerToGreater)
            {
                var start = 1f;
                var stop = Math.Min(width / (float)_baseWidth, height / (float)_baseHeight);
                var step = _factor;

                for (var f = start; f < stop; f *= step)
                    listSteps.Add(f);
            }
            else
            {
                var start = Math.Min(width / (float)_baseWidth, height / (float)_baseHeight);
                var stop = 1f;
                var step = 1f / _factor;

                for (var f = start; f > stop; f *= step)
                    listSteps.Add(f);
            }

            _steps = listSteps.ToArray();

            _lastWidth = width;
            _lastHeight = height;
        }

        private void CheckSteadiness(Rectangle[] rectangles)
        {
            if (_lastObjects == null ||
                rectangles == null ||
                rectangles.Length == 0)
            {
                Steady = 0;
                return;
            }

            var equals = true;
            foreach (var current in rectangles)
            {
                var found = false;
                foreach (var last in _lastObjects)
                {
                    if (current.IsEqual(last, _steadyThreshold))
                    {
                        found = true;
                        continue;
                    }
                }

                if (!found)
                {
                    equals = false;
                    break;
                }
            }

            if (equals)
            {
                Steady++;
            }
            else
            {
                Steady = 0;
            }
        }

        private bool Overlaps(Rectangle rect)
        {
            foreach (var r in _detectedObjects)
            {
                if (rect.IntersectsWith(r))
                {
                    return true;
                }
            }

            return false;
        }
    }
}