DataStreamDisplayAndGraphs: Graphic interface
SkeletonAndJointsDisplay: Display points (you control the poits displayed)
UnsupervisedPositionClassification: Learning method: Kmean, tracked data: 3D position
ClassificationUsingAngles: Learning method: Kmean, tracked data: Angle

Methods of "Image Acquisition Toolbox"
- JointDepthIndices => 20 x 2 x 6 double matrix of x-and y-coordinates for 20 joints in pixels relative to the depth image,
- JointImageIndices => 20 x 2 x 6 double matrix of x-and y-coordinates for 20 joints in pixels relative to the color image
- JointWorldCoordinates => 20 x 3 x 6 double matrix of x-, y- and z-coordinates for 20 joints, in meters from the sensor,
- IsSkeletonTracked => 1 x 6 Boolean matrix of true/false values for the tracked state of each of the six skeletons. A 1 indicates it is tracked and a 0 indicates it is not.