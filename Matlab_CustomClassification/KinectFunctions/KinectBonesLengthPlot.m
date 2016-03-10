function [] = KinectBonesLengthPlot(bonesLength,bonesNames)
    % Plots a histogram of the bones length with lengend 

    figure;
    hold all;
    for i=1:length(bonesLength)
        bar(i,bonesLength(i),'FaceColor',[rand(),rand(),rand()]);
    end
    hold off;
    grid;
    title('Subject''s bones length');
    ylabel('Bones length (m)');
    legend(bonesNames);

end

