function [] = KinectTimeAccPlot(t,dx2,dy2,dz2,nbPoints,legende)
    figure;
    
    subplot(3,1,1);
    hold all;
    for i=1:nbPoints
        plot(t(1:length(dx2(:,i))-4),dx2((1:length(dx2(:,i))-4),i));
    end
    hold off;
    title('Acc�l�rations horizontales au cours du temps');
    xlabel('t (s)');
    ylabel('d^2X');
    legend(legende);
    
    subplot(3,1,2);
    hold all;
    for i=1:nbPoints
        plot(t(1:length(dy2(:,i))-4),dy2((1:length(dy2(:,i))-4),i));
    end
    hold off;
    title('Acc�l�rations verticales au cours du temps');
    xlabel('t (s)');
    ylabel('d^2Y');
    legend(legende);

    subplot(3,1,3);
    hold all;
    for i=1:nbPoints
        plot(t(1:length(dz2(:,i))-4),dz2((1:length(dz2(:,i))-4),i));
    end
    hold off;
    title('Acc�l�rations de profondeur au cours du temps');
    xlabel('t (s)');
    ylabel('d^2Z');
    legend(legende);
end