function [  ] = DrawPoints( depthMetaData )
%UNTITLED4 Summary of this function goes here
%   Detailed explanation goes here
    

    skeletonJoints=depthMetaData.JointDepthIndices(:,:,depthMetaData.IsSkeletonTracked);
    skeletonJoints1=depthMetaData.JointImageIndices(:,:,depthMetaData.IsSkeletonTracked);
        hold on;
        plot(skeletonJoints(1:20,1),skeletonJoints(1:20,2),'r*');
        plot(skeletonJoints1(1:20,1),skeletonJoints(1:20,2),'g*');
        hold off;


end

