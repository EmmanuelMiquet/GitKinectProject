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
    using System.Windows.Forms;

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
        /// Path of the capture folder
        /// </summary>
        static string folderPath = "KinectCapture/";

        /// <summary>
        /// Path to write joints in a file
        /// </summary>
        string pointsCapturePath = folderPath + "pointsCapture.txt";

        /// <summary>
        /// Path to write relative joints in a file
        /// </summary>
        string relativePointsCapturePath = folderPath + "relativePointsCapture.txt";

        /// <summary>
        /// Path to write the joints legend
        /// </summary>
        string jointsLegendPath = folderPath + "jointsLegend.txt";

        /// <summary>
        /// Path to write angles in a file
        /// </summary>
        string anglesDataPath = folderPath + "anglesData.txt";
        
        /// <summary>
        /// Path to write the angles legend
        /// </summary>
        string anglesLegendPath = folderPath + "anglesLegend.txt";

        /// <summary>
        /// Path to write the bones length
        /// </summary>
        string bonesLengthPath = folderPath + "bonesLength.txt";

        /// <summary>
        /// List of joints that we want to track
        /// </summary>
        List<JointType> wantedJoints = new List<JointType>();

        /// <summary>
        /// Folder dialog box to change capture folder path
        /// </summary>
        FolderBrowserDialog fbd = new FolderBrowserDialog();

        /// <summary>
        /// Variable to store the checkbox state when chosing seated mode
        /// </summary>
        bool[] tmpState = new bool[10];

        /// <summary>
        /// Personnal set of tools for our project
        /// </summary>
        KinectTools tools = new KinectTools();

        /// <summary>
        /// File number for the name header of the file
        /// </summary>
        int fileIdentifier = 0;

        /// <summary>
        /// Variable for display all joints button
        /// </summary>
        bool displayAllJoints = false;

        /// <summary>
        /// Variable for testify that the capture is on
        /// </summary>
        bool captureOn = false;

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
            }

            if (null == this.sensor)
            {
                this.statusBarText.Text = Properties.Resources.NoKinectReady;
            }

            // Show the capture directory to the user
            CurrentPathBox.Text = "Capture folder path : \n" + Directory.GetCurrentDirectory().ToString() + "\\" + folderPath.Replace("/","\\");
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
                                //Our function to save the wanted joints
                                timeBox.Text = (string.Format("{0:0.0000}", tools.joints2list(skel, wantedJoints)).Replace(",",".") + " s");
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
                if (wantedJoints.Contains(joint.JointType) || displayAllJoints) //Handles the "displayAllJoint" checkBox
                {
                    if (joint.TrackingState == JointTrackingState.Tracked)
                    {
                        drawBrush = this.trackedJointBrush;
                    }
                    else if (joint.TrackingState == JointTrackingState.Inferred)
                    {
                        drawBrush = this.inferredJointBrush;
                    }
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
            if (!captureOn)
            {
                if (null != this.sensor)
                {
                    if (this.checkBoxSeatedMode.IsChecked.GetValueOrDefault())
                    {
                        this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;

                        // Save current checkbox state
                        tmpState[0] = (bool)this.HipLeft.IsChecked;
                        tmpState[1] = (bool)this.KneeLeft.IsChecked;
                        tmpState[2] = (bool)this.AnkleLeft.IsChecked;
                        tmpState[3] = (bool)this.FootLeft.IsChecked;
                        tmpState[4] = (bool)this.HipRight.IsChecked;
                        tmpState[5] = (bool)this.KneeRight.IsChecked;
                        tmpState[6] = (bool)this.AnkleRight.IsChecked;
                        tmpState[7] = (bool)this.FootRight.IsChecked;
                        tmpState[8] = (bool)this.HipCenter.IsChecked;
                        tmpState[9] = (bool)this.Spine.IsChecked;

                        // Uncheck all the checkbox influenced by seated mode
                        this.HipLeft.IsChecked = false;
                        this.KneeLeft.IsChecked = false;
                        this.AnkleLeft.IsChecked = false;
                        this.FootLeft.IsChecked = false;
                        this.HipRight.IsChecked = false;
                        this.KneeRight.IsChecked = false;
                        this.AnkleRight.IsChecked = false;
                        this.FootRight.IsChecked = false;
                        this.HipCenter.IsChecked = false;
                        this.Spine.IsChecked = false;

                        // Disable the uncheked boxes
                        this.HipLeft.IsEnabled = false;
                        this.KneeLeft.IsEnabled = false;
                        this.AnkleLeft.IsEnabled = false;
                        this.FootLeft.IsEnabled = false;
                        this.HipRight.IsEnabled = false;
                        this.KneeRight.IsEnabled = false;
                        this.AnkleRight.IsEnabled = false;
                        this.FootRight.IsEnabled = false;
                        this.HipCenter.IsEnabled = false;
                        this.Spine.IsEnabled = false;
                    }
                    else
                    {
                        this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Default;

                        // Restore previous checked state
                        this.HipLeft.IsChecked = tmpState[0];
                        this.KneeLeft.IsChecked = tmpState[1];
                        this.AnkleLeft.IsChecked = tmpState[2];
                        this.FootLeft.IsChecked = tmpState[3];
                        this.HipRight.IsChecked = tmpState[4];
                        this.KneeRight.IsChecked = tmpState[5];
                        this.AnkleRight.IsChecked = tmpState[6];
                        this.FootRight.IsChecked = tmpState[7];
                        this.HipCenter.IsChecked = tmpState[8];
                        this.Spine.IsChecked = tmpState[9];
                        
                        // Disable the uncheked boxes
                        this.HipLeft.IsEnabled = true;
                        this.KneeLeft.IsEnabled = true;
                        this.AnkleLeft.IsEnabled = true;
                        this.FootLeft.IsEnabled = true;
                        this.HipRight.IsEnabled = true;
                        this.KneeRight.IsEnabled = true;
                        this.AnkleRight.IsEnabled = true;
                        this.FootRight.IsEnabled = true;
                        this.HipCenter.IsEnabled = true;
                        this.Spine.IsEnabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the checking or unchecking of the capture on combo box
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void CheckBoxCaptureOn(object sender, RoutedEventArgs e)
        {
            if (null != this.sensor)
            {
                if (this.Capture_On.IsChecked.GetValueOrDefault())
                {
                    captureOn = true;
                    REC.Visibility = Visibility.Visible;

                    this.NewFiles.IsEnabled = false;
                    this.ProcessData.IsEnabled = false;
                    this.ClearCapture.IsEnabled = false;
                    this.checkBoxSeatedMode.IsEnabled = false;
                    this.FolderPathButton.IsEnabled = false;

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
                    REC.Visibility = Visibility.Hidden;
                    this.ProcessData.IsEnabled = true;
                    this.ClearCapture.IsEnabled = true;
                }
            }
        }

        /// <summary>
        /// Handles the checking or unchecking of the capture on combo box
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void CheckBoxDisplayAllJoints(object sender, RoutedEventArgs e)
        {
            if (this.displayAll.IsChecked.GetValueOrDefault())
            {
                displayAllJoints = true;
            }
            else
            {
                displayAllJoints = false;
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
                        if (this.WarningBox != null)
                        {
                            this.WarningBox.Visibility = Visibility.Hidden;
                        }
                    }
                }
                else
                {
                    wantedJoints.Remove(JointType.Spine);
                    if (this.WarningBox != null)
                    {
                        this.WarningBox.Visibility = Visibility.Visible;
                    }
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

        /// <summary>
        /// Process and write the data in the files on the press of the Process Data button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickProcessData(object sender, RoutedEventArgs e)
        {
            //Re-initialize the time count
            this.timeBox.Text = "0.0000 s";

            //Create capture directory
            if (Directory.Exists(folderPath))
            {
                foreach (string file in Directory.GetFiles(folderPath))
                {
                    FileInfo tmp = new FileInfo(file);
                    tmp.Delete();
                }
            }
            Directory.CreateDirectory(folderPath);

            //Modify the names of the differents files
            if (fileIdentifier == 0)
            {
                pointsCapturePath = pointsCapturePath.Replace(folderPath,folderPath + fileIdentifier.ToString() + "_");
                relativePointsCapturePath = relativePointsCapturePath.Replace(folderPath, folderPath + fileIdentifier.ToString() + "_");
                jointsLegendPath = jointsLegendPath.Replace(folderPath, folderPath + fileIdentifier.ToString() + "_");
                anglesDataPath = anglesDataPath.Replace(folderPath, folderPath + fileIdentifier.ToString() + "_");
                anglesLegendPath = anglesLegendPath.Replace(folderPath, folderPath + fileIdentifier.ToString() + "_");
                bonesLengthPath = bonesLengthPath.Replace(folderPath, folderPath + fileIdentifier.ToString() + "_");
            }
            else
            {
                pointsCapturePath = pointsCapturePath.Replace(folderPath + (fileIdentifier-1).ToString(), folderPath + fileIdentifier.ToString());
                relativePointsCapturePath = relativePointsCapturePath.Replace(folderPath + (fileIdentifier - 1).ToString(), folderPath + fileIdentifier.ToString());
                jointsLegendPath = jointsLegendPath.Replace(folderPath + (fileIdentifier - 1).ToString(), folderPath + fileIdentifier.ToString());
                anglesDataPath = anglesDataPath.Replace(folderPath + (fileIdentifier - 1).ToString(), folderPath + fileIdentifier.ToString());
                anglesLegendPath = anglesLegendPath.Replace(folderPath + (fileIdentifier - 1).ToString(), folderPath + fileIdentifier.ToString());
                bonesLengthPath = bonesLengthPath.Replace(folderPath + (fileIdentifier - 1).ToString(), folderPath + fileIdentifier.ToString());
            }

            //Write joints in a file
            tools.joints2file(pointsCapturePath, jointsLegendPath, wantedJoints);

            //Write relative position in a file
            tools.relativeJoints2file(relativePointsCapturePath, wantedJoints);

            //Calculate and write angles and bone length
            tools.manageAngles(anglesLegendPath, bonesLengthPath, anglesDataPath, wantedJoints);

            //Clear all the local variables in knect tools
            tools.clearData();

            //Unabling to take a new capture (and other buttons)
            this.Capture_On.IsEnabled = false;
            this.ProcessData.IsEnabled = false;
            this.ClearCapture.IsEnabled = false;

            this.NewFiles.IsEnabled = true;
        }

        /// <summary>
        /// Create new pathes (increment the file identifier) for the capture files on the press of the New Files button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickNewFiles(object sender, RoutedEventArgs e)
        {
            //Increment the file identifier number
            fileIdentifier++;

            //Enable/Disable buttons and checkboxes
            this.NewFiles.IsEnabled = false;

            this.Capture_On.IsEnabled = true;
            this.checkBoxSeatedMode.IsEnabled = true;
            this.FolderPathButton.IsEnabled = true;

            
            this.ShoulderCenter.IsEnabled = true;
            this.Head.IsEnabled = true;
            this.ShoulderLeft.IsEnabled = true;
            this.ElbowLeft.IsEnabled = true;
            this.WristLeft.IsEnabled = true;
            this.HandLeft.IsEnabled = true;
            this.ShoulderRight.IsEnabled = true;
            this.ElbowRight.IsEnabled = true;
            this.WristRight.IsEnabled = true;
            this.HandRight.IsEnabled = true;
            
            if(!(bool)this.checkBoxSeatedMode.IsChecked)
            {
                this.HipLeft.IsEnabled = true;
                this.KneeLeft.IsEnabled = true;
                this.AnkleLeft.IsEnabled = true;
                this.FootLeft.IsEnabled = true;
                this.HipRight.IsEnabled = true;
                this.KneeRight.IsEnabled = true;
                this.AnkleRight.IsEnabled = true;
                this.FootRight.IsEnabled = true;
                this.HipCenter.IsEnabled = true;
                this.Spine.IsEnabled = true;
            }
        }

        /// <summary>
        /// Clear capture data points on the press of the ClearCapture button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickClearCapture(object sender, RoutedEventArgs e)
        {
            //Clear the capture data
            tools.clearData();

            //Re-initialize the time count
            this.timeBox.Text = "0.0000 s";

            //Enable/Disable buttons and checkboxes
            this.ClearCapture.IsEnabled = false;
            this.ProcessData.IsEnabled = false;

            this.Capture_On.IsEnabled = true;
            this.checkBoxSeatedMode.IsEnabled = true;
            this.FolderPathButton.IsEnabled = true;

            this.HipCenter.IsEnabled = true;
            this.Spine.IsEnabled = true;
            this.ShoulderCenter.IsEnabled = true;
            this.Head.IsEnabled = true;
            this.ShoulderLeft.IsEnabled = true;
            this.ElbowLeft.IsEnabled = true;
            this.WristLeft.IsEnabled = true;
            this.HandLeft.IsEnabled = true;
            this.ShoulderRight.IsEnabled = true;
            this.ElbowRight.IsEnabled = true;
            this.WristRight.IsEnabled = true;
            this.HandRight.IsEnabled = true;
            this.HipLeft.IsEnabled = true;
            this.KneeLeft.IsEnabled = true;
            this.AnkleLeft.IsEnabled = true;
            this.FootLeft.IsEnabled = true;
            this.HipRight.IsEnabled = true;
            this.KneeRight.IsEnabled = true;
            this.AnkleRight.IsEnabled = true;
            this.FootRight.IsEnabled = true;
        }

        /// <summary>
        /// Refresh the connection with the kinect to know if there is one ready
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickRefreshConnection(object sender, RoutedEventArgs e)
        {
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
                else
                {
                    this.sensor = null;
                }
            }
            if(KinectSensor.KinectSensors.Count == 0)
            {
                this.sensor = null;
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
                    this.statusBarText.Text = "Click 'Seated' to change skeletal pipeline type!";
                }
                catch (IOException)
                {
                    this.sensor = null;
                }
            }

            if (null == this.sensor)
            {
                this.statusBarText.Text = Properties.Resources.NoKinectReady;
            }
        }

        /// <summary>
        /// Give the user the ability to change the capture folder path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickFolderPath(object sender, RoutedEventArgs e)
        {
            fbd.Description = "Select a directory to put \\KinectCapture\\ \n Current selected folder : " + "\\" + folderPath.Replace("/","\\");
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (System.Windows.MessageBox.Show("Your files will be stored at : " + fbd.SelectedPath.ToString() + "\\KinectCapture", "Confirm selected path", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    //Update the folder path
                    folderPath = fbd.SelectedPath.ToString().Replace("\\", "\\") + "\\KinectCapture\\";
                    pointsCapturePath = folderPath + "pointsCapture.txt";
                    relativePointsCapturePath = folderPath + "relativePointsCapture.txt";
                    jointsLegendPath = folderPath + "jointsLegend.txt";
                    anglesDataPath = folderPath + "anglesData.txt";
                    anglesLegendPath = folderPath + "anglesLegend.txt";
                    bonesLengthPath = folderPath + "bonesLength.txt";

                    // Update the path showed to the user
                    CurrentPathBox.Text = "Capture folder path : \n" + folderPath;
                }
            }
        }
    }
}