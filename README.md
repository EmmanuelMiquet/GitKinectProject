# GitKinectProject
Git repository for Kinect Projet Long in LAAS (feb - mar 2016)

# Content
- Matlab_CustomClassification
	- ExperienceData : Folder for experience data
	- KinectFunctions : Contains all our developed Kinect functions (this folder is added to the path within MATLAB to use our functions)
	- TrainingDataBase : Contains our data files for our three positions (a lot of points ...) Positions and/or data can be added (see readme in the folder)
	- Script_ClassificationDistance : Script to classify the data with our classification function based on KNN (with the use of the database, how to manage the load of multiple files)

- Matlab_Toolbox&Classification_usageExample
	- Examples on how to use our Kinect functions to plot, process and classify Kinect data
	- ExperienceData : Folder for experience data
	- KinectFunctions : Contains all our developed Kinect functions (this folder is added to the path within MATLAB to use our functions)
	- TrainingData : Folder for training data
	- Script : Script to visualize the extracted data from Kinect, space plot, evaluate the speed and acceleration ...
	- Script_ClassificationDistance : Script to classify the data with our classification function based on KNN (it's an example of use)

- Orange
	- ExperienceData : Folder for experience data (in .tab)
	- TrainingData : Folder for training data (in .tab)
	- KinectPositions.ows : Orange Project file
	- 3 .tab : Some output files with learning and prediction

- Release
	- Contains the release version of our Windows application for points extraction (with the .dll if it's needed)

- Report&Presentation
	- Contains the report (in French) and our presentation (in English)

- SkeletonTracking-WPF
	- Contains the source code of our Windows (C#) application (just launch the project file)
	- MainWindows.xaml : graphical interface
	- MainWindows.xaml.cs : graphical interface code
	- KinectTools.cs : our portable toolbox for Kinect projects

- Useful_links
	- Contains the links to download the Kinect for Windows SDK (needed to launch our application because it installs the driver), the Kinect for Windows Developer Toolbox (to have some example code) and the link to the MSDN page for Kinect skeleton tracking
