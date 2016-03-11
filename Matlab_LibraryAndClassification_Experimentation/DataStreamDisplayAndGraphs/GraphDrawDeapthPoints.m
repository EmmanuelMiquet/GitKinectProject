function GraphDrawDeapthPoints( depthMetaData,state )

persistent rep1 h2 ;
if (isempty(rep1))
    rep1=0;
end

if (state ==1)
    if(rep1~=0)
        set(h2,'Visible','off');
    end

    if sum(depthMetaData.IsSkeletonTracked)>0
        rep1=1;
        skeletonJoints=depthMetaData.JointDepthIndices(:,:,depthMetaData.IsSkeletonTracked);
        hold on;
        h2=plot(skeletonJoints(1:20,1),skeletonJoints(1:20,2),'r*');
        hold off;
    end

elseif(state==0)
        set(h2,'Visible','off');
        rep1=0;
end
end

