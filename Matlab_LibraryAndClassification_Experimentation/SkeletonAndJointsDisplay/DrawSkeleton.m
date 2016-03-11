function [ ] = DrawSkeleton( depthMetaData,nSkeleton )
%UNTITLED2 Summary of this function goes here
%   Detailed explanation goes here

% Find the tracked squeleton

skeleton=depthMetaData.JointDepthIndices(:,:,depthMetaData.IsSkeletonTracked);

 SkeletonConnectionMap = [[1 2]; % Spine
                         [2 3];
                         [3 4];
                         [3 5]; %Left Hand
                         [5 6];
                         [6 7];
                         [7 8];
                         [3 9]; %Right Hand
                         [9 10];
                         [10 11];
                         [11 12];
                         [1 17]; % Right Leg
                         [17 18];
                         [18 19];
                         [19 20];
                         [1 13]; % Left Leg
                         [13 14];
                         [14 15];
                         [15 16]];
 

for i= 1:1:19
 if nSkeleton >= 1
         X2 = [skeleton(SkeletonConnectionMap(i,1),1,1) skeleton(SkeletonConnectionMap(i,2),1,1)];
         Y2 = [skeleton(SkeletonConnectionMap(i,1),2,1) skeleton(SkeletonConnectionMap(i,2),2,1)];
         line(X2,Y2,'LineWidth',1.5);
 end
  hold on; 
end
hold off;
end

