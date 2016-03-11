function GraphDrawPixelPoints( depthMetaData,state )

persistent rep h1 ;
if (isempty(rep))
    rep=0;
end

if (state== 1)
        if(rep~=0)
            set(h1,'Visible','off');
        end


        if sum(depthMetaData.IsSkeletonTracked)>0
            rep=1;
            skeletonJoints1=depthMetaData.JointImageIndices(:,:,depthMetaData.IsSkeletonTracked);
            hold on;
            h1=plot(skeletonJoints1(1:20,1),skeletonJoints1(1:20,2),'g*');
            hold off;
        end
elseif(state==0)
        set(h1,'Visible','off');
        rep=0;
end
    
end

