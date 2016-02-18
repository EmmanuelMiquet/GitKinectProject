function [] = KinectTimeVitPlot(t,dx,dy,dz,nbPoints,legende)
    figure;
    
    subplot(3,1,1);
    hold all;
    for i=1:nbPoints
        plot(t(1:length(dx(:,i))-4),dx((1:length(dx(:,i))-4),i));
    end
    hold off;
    title('Vitesses horizontales au cours du temps');
    xlabel('t (s)');
    ylabel('dX');
    
    subplot(3,1,2);
    hold all;
    for i=1:nbPoints
        plot(t(1:length(dy(:,i))-4),dy((1:length(dy(:,i))-4),i));
    end
    hold off;
    title('Vitesses verticales au cours du temps');
    xlabel('t (s)');
    ylabel('dY');

    subplot(3,1,3);
    hold all;
    for i=1:nbPoints
        plot(t(1:length(dz(:,i))-4),dz((1:length(dz(:,i))-4),i));
    end
    hold off;
    title('Vitesses de profondeur au cours du temps');
    xlabel('t (s)');
    ylabel('dZ');
    legend(legende);
end