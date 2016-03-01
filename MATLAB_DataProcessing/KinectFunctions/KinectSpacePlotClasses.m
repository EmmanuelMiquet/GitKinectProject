function [] = KinectSpacePlotClasses(predict,Xtest,Ytest,Ztest)
    % Plot the positions of each data predicted by the classifier
    % in different colors. The data rejected by the classifier 
    % are not shown
    
	classes = unique(predict);
    classes = classes(classes~=0);
    h = zeros(length(classes),length(Xtest(1,:)));
   
    figure;
    hold all;
    for j = 1:length(classes)
        k = 1;
        for i = 1:length(predict)
            if predict(i) == classes(j)
                C{k,j} = i;
                k = k + 1;
            end
        end
        h(j,:) = plot3(Xtest([C{:,j}],:),Ztest([C{:,j}],:),Ytest([C{:,j}],:),'Color',[rand() rand() rand()],'LineWidth',1.25);
        legendInfo{j,1} = ['Class ' num2str(classes(j))];
    end
    
    xlabel('x');
    ylabel('z');
    zlabel('y');
    title('Spatial distribution of the predicted classes');
    grid on;
    view (3), rotate3d on;
    legend(h(:,1),legendInfo);
end

