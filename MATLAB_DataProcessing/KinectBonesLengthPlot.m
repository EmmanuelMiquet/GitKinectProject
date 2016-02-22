function [] = KinectBonesLengthPlot(bonesLength,bonesNames)
    figure;
    hold all;
    for i=1:length(bonesLength)
        bar(i,bonesLength(i),'FaceColor',[rand(),rand(),rand()]);
    end
    hold off;
    grid;
    title('Longueurs des membres du sujet');
    ylabel('Longeur membre (m)');
    legend(bonesNames);
end

