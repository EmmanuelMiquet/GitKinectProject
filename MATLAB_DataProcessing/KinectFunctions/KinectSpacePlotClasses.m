function [] = KinectSpacePlotClasses(predict,Xtest,Ytest,Ztest)
    % Shows the position of each data predicted by the classifier
    % belonging to a different class. The data rejected by the classifier 
    % are not shown
    
    figure;
    hold all;
    for j = 1:max(predict)
        k = 1;
        for i = 1:length(predict)
            if predict(i) == j
                C{k,j} = i;
                k = k + 1;
            end
        end
        plot3(Xtest([C{:,j}],:),Ztest([C{:,j}],:),Ytest([C{:,j}],:),'Color',[rand() rand() rand()]);
    end
    
    xlabel('x');
    ylabel('z');
    zlabel('y');
    title('Spatial distribution of the predicted classes');
    grid on;
    view (3), rotate3d on;
end

