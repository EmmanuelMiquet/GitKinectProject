function GraphDrawPoints( depthMetaData,state )

persistent rep3 h3 h4;
if (isempty(rep3))
    rep3=0;
end
if (state ==1)
    if(rep3~=0)
        set(h3,'Visible','off');
        set(h4,'Visible','off');
    end

    if sum(depthMetaData.IsSkeletonTracked)>0
        rep3=1;
        skeletonJoints=depthMetaData.JointDepthIndices(:,:,depthMetaData.IsSkeletonTracked);
        skeletonJoints1=depthMetaData.JointImageIndices(:,:,depthMetaData.IsSkeletonTracked);
        hold on;
        h3=plot(skeletonJoints(1:20,1),skeletonJoints(1:20,2),'r*');
        h4=plot(skeletonJoints1(1:20,1),skeletonJoints(1:20,2),'g*');
        hold off;
    end

elseif (state ==0)
    set(h3,'Visible','off');
    set(h4,'Visible','off')
    rep3=0;
end
end

