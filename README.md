# GitKinectProject
Git repository for Kinect Projet Long in LAAS (feb - mar 2016)

# Content
- Matlab_CustomClassification : Automated supervised classes learning (big database) + prediction with custom KNN algorithm
	- ExperienceData : Folder for experience (test) data, only takes one test file for now, could recursively test different files using same principle as for learning data loading
	- KinectFunctions : Contains all our developed Kinect functions (this folder is added to the path within MATLAB to use our functions)
	- TrainingDataBase : Contains our data files for our three positions (a lot of points ...) Positions and/or data can be added (see readme in the folder)
	- Script_ClassificationDistance : Script to classify the data with our classification function based on KNN (with the use of the database, how to manage the load of multiple files)

- Matlab_LibraryAndClassification_Experimentation : Tests on native Matlab Image Processing Toolbox and tests on classification alternatives (unsupervised with K-Means, using angles...)
	- ClassificationUsingAngles : Folder containing scripts and functions for angle calculation (oriented angles using vector product and projections) and classification using these data (plus a sample database for this example)
	- DataStreamDisplayAndGraphs : Folder containing scripts and functions to display joints obtained with RGB and IR trackings for comparison
	- SkeletonAndJointsDisplay : Folder containing scripts and functions to display a skeleton and plot retrieved data
	- UnsupervisedPositionClassification : Folder containing scripts and functions to classify a simple movement using simple absolute positions and using K-Means unsupervised learning
	Note: Require the "Image acquisition toolbox" and a Matlab 2014 version or newer

- Matlab_Toolbox&Classification_usageExample : Simple and unitary tests to use our KinectFunctions library
	- Examples on how to use our Kinect functions to plot, process and classify Kinect data
	- ExperienceData : Folder for experience data
	- KinectFunctions : Contains all our developed Kinect functions (this folder is added to the path within MATLAB to use our functions)
	- TrainingData : Folder for training data
	- Script : Script to visualize the extracted data from Kinect, space plot, evaluate the speed and acceleration ...
	- Script_ClassificationDistance : Script to classify the data with our classification function based on KNN (it's an example of use)

- Orange : Tests on Orange software using GUI
	- Kinect : Contains Kinect positions learning and prediction tests (Orange project and input/output data)
	- Swimmers : Contains Swimmers movements Orange project, input files in correct format and output extracted swim data
	- Readme on Orange basics

- Release
	- Contains the release version of our Windows application for points extraction (with the .dll if it's needed)

- Report&Presentation
	- Contains the report (in French) and our presentation (in English) for project review at ENSEEIHT

- SkeletonTracking-WPF
	- Contains the source code of our Windows (C#) application (just launch the project file)
	- MainWindows.xaml : graphical interface
	- MainWindows.xaml.cs : graphical interface code
	- KinectTools.cs : our portable toolbox for Kinect projects

- Useful_links
	- Contains the links to download the Kinect for Windows SDK (needed to launch our application because it installs the driver), the Kinect for Windows Developer Toolbox (to have some example code) and the link to the MSDN page for Kinect skeleton tracking
