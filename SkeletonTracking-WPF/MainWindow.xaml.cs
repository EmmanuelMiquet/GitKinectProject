//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.SkeletonBasics
{
    using System.IO;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;
    using Microsoft.Kinect;
    using PL.Kinect;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Width of output drawing
        /// </summary>
        private const float RenderWidth = 640.0f;

        /// <summary>
        /// Height of our output drawing
        /// </summary>
        private const float RenderHeight = 480.0f;

        /// <summary>
        /// Thickness of drawn joint lines
        /// </summary>
        private const double JointThickness = 3;

        /// <summary>
        /// Thickness of body center ellipse
        /// </summary>
        private const double BodyCenterThickness = 10;

        /// <summary>
        /// Thickness of clip edge rectangles
        /// </summary>
        private const double ClipBoundsThickness = 10;

        /// <summary>
        /// Brush used to draw skeleton center point
        /// </summary>
        private readonly Brush centerPointBrush = Brushes.Blue;

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Brush used for drawing joints that are currently inferred
        /// </summary>        
        private readonly Brush inferredJointBrush = Brushes.Yellow;

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor sensor;

        /// <summary>
        /// Drawing group for skeleton rendering output
        /// </summary>
        private DrawingGroup drawingGroup;

        /// <summary>
        /// Drawing image that we will display
        /// </summary>
        private DrawingImage imageSource;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a stram to write joints in a file
        /// </summary>
        TextWriter TAB = new StreamWriter("pointsCapture.txt");
        TextWriter jointsLegend = new StreamWriter("jointsLegend.txt");

        /// <summary>
        /// New list of joints that we want to track
        /// </summary>
        List<JointType> wantedJoints = new List<JointType>();

        /// <summary>
        /// New instance of our objects to acces our tools
        /// </summary>
        KinectTools tools = new KinectTools();

        /// <summary>
        /// Draws indicators to show which edges are clipping skeleton data
        /// </summary>
        /// <param name="skeleton">skeleton to draw clipping information for</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private static void RenderClippedEdges(Skeleton skeleton, DrawingContext drawingContext)
        {
            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Bottom))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, RenderHeight - ClipBoundsThickness, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Top))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Left))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, ClipBoundsThickness, RenderHeight));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Right))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(RenderWidth - ClipBoundsThickness, 0, ClipBoundsThickness, RenderHeight));
            }
        }

        /// <summary>
        /// Execute startup tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // Create the drawing group we'll use for drawing
            this.drawingGroup = new DrawingGroup();

            // Create an image source that we can use in our image control
            this.imageSource = new DrawingImage(this.drawingGroup);

            // Display the drawing using our image control
            Image.Source = this.imageSource;

            // Look through all sensors and start the first connected one.
            // This requires that a Kinect is connected at the time of app startup.
            // To make your app robust against plug/unplug, 
            // it is recommended to use KinectSensorChooser provided in Microsoft.Kinect.Toolkit (See components in Toolkit Browser).

            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (null != this.sensor)
            {
                // Turn on the skeleton stream to receive skeleton frames
                this.sensor.SkeletonStream.Enable();

                // Add an event handler to be called whenever there is new color frame data
                this.sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;

                // Start the sensor!
                try
                {
                    this.sensor.Start();
                }
                catch (IOException)
                {
                    this.sensor = null;
                }
                //Add joints to track to the list
                //wantedJoints.Add(JointType.HipCenter);
                //wantedJoints.Add(JointType.Spine);
                //wantedJoints.Add(JointType.ShoulderCenter);
                //wantedJoints.Add(JointType.Head);

                //wantedJoints.Add(JointType.ShoulderLeft);
                //wantedJoints.Add(JointType.ElbowLeft);
                //wantedJoints.Add(JointType.WristLeft);
                //wantedJoints.Add(JointType.ShoulderRight);
                //wantedJoints.Add(JointType.ElbowRight);
                //wantedJoints.Add(JointType.WristRight);

                //wantedJoints.Add(JointType.HipLeft);
                //wantedJoints.Add(JointType.KneeLeft);
                //wantedJoints.Add(JointType.AnkleLeft);
                //wantedJoints.Add(JointType.AnkleRight);
                //wantedJoints.Add(JointType.HipRight);
                //wantedJoints.Add(JointType.KneeRight);
                
                          
            }

            if (null == this.sensor)
            {
                this.statusBarText.Text = Properties.Resources.NoKinectReady;
            }
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != this.sensor)
            {
                this.sensor.Stop();
            }

            tools.joints2file(TAB);
            //Write the tracked joints for legend
            foreach (JointType joint in wantedJoints)
            {
                jointsLegend.WriteLine(joint.ToString().PadRight(14));
            }
            TAB.Close();
            jointsLegend.Close();
        }

        /// <summary>
        /// Event handler for Kinect sensor's SkeletonFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            using (DrawingContext dc = this.drawingGroup.Open())
            {
                // Draw a transparent background to set the render size
                dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));

                if (skeletons.Length != 0)
                {
                    foreach (Skeleton skel in skeletons)
                    {
                        RenderClippedEdges(skel, dc);

                        if (skel.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            this.DrawBonesAndJoints(skel, dc);
                            if (captureOn)
                            {
                                tools.joints2list(skel, wantedJoints); //Notre fonction
                            }
                        }
                        else if (skel.TrackingState == SkeletonTrackingState.PositionOnly)
                        {
                            dc.DrawEllipse(
                            this.centerPointBrush,
                            null,
                            this.SkeletonPointToScreen(skel.Position),
                            BodyCenterThickness,
                            BodyCenterThickness);
                        }
                    }
                }

                // prevent drawing outside of our render area
                this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
            }
        }

        /// <summary>
        /// Draws a skeleton's bones and joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawBonesAndJoints(Skeleton skeleton, DrawingContext drawingContext)
        {
            // Render Joints
            foreach (Joint joint in skeleton.Joints)
            {
                Brush drawBrush = null;

                if (joint.TrackingState == JointTrackingState.Tracked)
                {
                    drawBrush = this.trackedJointBrush;
                }
                else if (joint.TrackingState == JointTrackingState.Inferred)
                {
                    drawBrush = this.inferredJointBrush;                    
                }

                if (drawBrush != null)
                {
                    drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
                }
            }
        }

        /// <summary>
        /// Maps a SkeletonPoint to lie within our render space and converts to Point
        /// </summary>
        /// <param name="skelpoint">point to map</param>
        /// <returns>mapped point</returns>
        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            // Convert point to depth space.  
            // We are not using depth directly, but we do want the points in our 640x480 output resolution.
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }

        /// <summary>
        /// Handles the checking or unchecking of the seated mode combo box
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void CheckBoxSeatedModeChanged(object sender, RoutedEventArgs e)
        {
            if (null != this.sensor)
            {
                if (this.checkBoxSeatedMode.IsChecked.GetValueOrDefault())
                {
                    this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
                }
                else
                {
                    this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Default;
                }
            }
        }

        /// <summary>
        /// Handles the checking or unchecking of the capture on combo box
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        bool captureOn = false;
        private void CheckBoxCaptureOn(object sender, RoutedEventArgs e)
        {
            if (null != this.sensor)
            {
                if (this.Capture_On.IsChecked.GetValueOrDefault())
                {
                    captureOn = true;
                    this.HipCenter.IsEnabled = false;
                    this.Spine.IsEnabled = false;
                    this.ShoulderCenter.IsEnabled = false;
                    this.Head.IsEnabled = false;
                    this.ShoulderLeft.IsEnabled = false;
                    this.ElbowLeft.IsEnabled = false;
                    this.WristLeft.IsEnabled = false;
                    this.HandLeft.IsEnabled = false;
                    this.ShoulderRight.IsEnabled = false;
                    this.ElbowRight.IsEnabled = false;
                    this.WristRight.IsEnabled = false;
                    this.HandRight.IsEnabled = false;
                    this.HipLeft.IsEnabled = false;
                    this.KneeLeft.IsEnabled = false;
                    this.AnkleLeft.IsEnabled = false;
                    this.FootLeft.IsEnabled = false;
                    this.HipRight.IsEnabled = false;
                    this.KneeRight.IsEnabled = false;
                    this.AnkleRight.IsEnabled = false;
                    this.FootRight.IsEnabled = false;
                }
                else
                {
                    captureOn = false;
                }
            }
        }

        /// <summary>
        /// Handles the checking or unchecking of the joints combo box
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void CheckBoxJoints(object sender, RoutedEventArgs e)
        {
            if (!captureOn)
            {
                if (this.HipCenter != null && this.HipCenter.IsChecked.GetValueOrDefault())
                {
                    if(!wantedJoints.Contains(JointType.HipCenter))
                    {
                        wantedJoints.Add(JointType.HipCenter);
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.HipCenter);
                }

                if (this.Spine != null && this.Spine.IsChecked.GetValueOrDefault())
                {
                    if (!wantedJoints.Contains(JointType.Spine))
                    {
                        wantedJoints.Add(JointType.Spine);
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.Spine);
                }

                if (this.ShoulderCenter != null && this.ShoulderCenter.IsChecked.GetValueOrDefault())
                {
                    if (!wantedJoints.Contains(JointType.ShoulderCenter))
                    {
                        wantedJoints.Add(JointType.ShoulderCenter);
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.ShoulderCenter);
                }

                if (this.Head != null && this.Head.IsChecked.GetValueOrDefault())
                {
                    if (!wantedJoints.Contains(JointType.Head))
                    {
                        wantedJoints.Add(JointType.Head);
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.Head);
                }

                if (this.ShoulderLeft != null && this.ShoulderLeft.IsChecked.GetValueOrDefault())
                {
                    if (!wantedJoints.Contains(JointType.ShoulderLeft))
                    {
                        wantedJoints.Add(JointType.ShoulderLeft);
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.ShoulderLeft);
                }

                if (this.ElbowLeft != null && this.ElbowLeft.IsChecked.GetValueOrDefault())
                {
                    if (!wantedJoints.Contains(JointType.ElbowLeft))
                    {
                        wantedJoints.Add(JointType.ElbowLeft);
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.ElbowLeft);
                }

                if (this.WristLeft != null && this.WristLeft.IsChecked.GetValueOrDefault())
                {
                    if (!wantedJoints.Contains(JointType.WristLeft))
                    {
                        wantedJoints.Add(JointType.WristLeft);
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.WristLeft);
                }

                if (this.HandLeft != null && this.HandLeft.IsChecked.GetValueOrDefault())
                {
                    if (!wantedJoints.Contains(JointType.HandLeft))
                    {
                        wantedJoints.Add(JointType.HandLeft);
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.HandLeft);
                }

                if (this.ShoulderRight != null && this.ShoulderRight.IsChecked.GetValueOrDefault())
                {
                    if (!wantedJoints.Contains(JointType.ShoulderRight))
                    {
                        wantedJoints.Add(JointType.ShoulderRight);
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.ShoulderRight);
                }

                if (this.ElbowRight != null && this.ElbowRight.IsChecked.GetValueOrDefault())
                {
                    if (!wantedJoints.Contains(JointType.ElbowRight))
                    {
                        wantedJoints.Add(JointType.ElbowRight);
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.ElbowRight);
                }

                if (this.WristRight != null && this.WristRight.IsChecked.GetValueOrDefault())
                {
                    if (!wantedJoints.Contains(JointType.WristRight))
                    {
                        wantedJoints.Add(JointType.WristRight);
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.WristRight);
                }

                if (this.HandRight != null && this.HandRight.IsChecked.GetValueOrDefault())
                {
                    if (!wantedJoints.Contains(JointType.HandRight))
                    {
                        wantedJoints.Add(JointType.HandRight);
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.HandRight);
                }

                if (this.HipLeft != null && this.HipLeft.IsChecked.GetValueOrDefault())
                {
                    if (!wantedJoints.Contains(JointType.HipLeft))
                    {
                        wantedJoints.Add(JointType.HipLeft);
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.HipLeft);
                }

                if (this.KneeLeft != null && this.KneeLeft.IsChecked.GetValueOrDefault())
                {
                    if (!wantedJoints.Contains(JointType.KneeLeft))
                    {
                        wantedJoints.Add(JointType.KneeLeft);
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.KneeLeft);
                }

                if (this.AnkleLeft != null && this.AnkleLeft.IsChecked.GetValueOrDefault())
                {
                    if (!wantedJoints.Contains(JointType.AnkleLeft))
                    {
                        wantedJoints.Add(JointType.AnkleLeft);
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.AnkleLeft);
                }

                if (this.FootLeft != null && this.FootLeft.IsChecked.GetValueOrDefault())
                {
                    if (!wantedJoints.Contains(JointType.FootLeft))
                    {
                        wantedJoints.Add(JointType.FootLeft);
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.FootLeft);
                }

                if (this.HipRight != null && this.HipRight.IsChecked.GetValueOrDefault())
                {
                    if (!wantedJoints.Contains(JointType.HipRight))
                    {
                        wantedJoints.Add(JointType.HipRight);
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.HipRight);
                }

                if (this.KneeRight != null && this.KneeRight.IsChecked.GetValueOrDefault())
                {
                    if (!wantedJoints.Contains(JointType.KneeRight))
                    {
                        wantedJoints.Add(JointType.KneeRight);
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.KneeRight);
                }

                if (this.AnkleRight != null && this.AnkleRight.IsChecked.GetValueOrDefault())
                {
                    if (!wantedJoints.Contains(JointType.AnkleRight))
                    {
                        wantedJoints.Add(JointType.AnkleRight);
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.AnkleRight);
                }

                if (this.FootRight != null && this.FootRight.IsChecked.GetValueOrDefault())
                {
                    if (!wantedJoints.Contains(JointType.FootRight))
                    {
                        wantedJoints.Add(JointType.FootRight);
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.FootRight);
                }
            }
        }
    }
}