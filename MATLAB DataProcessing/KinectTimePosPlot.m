function [] = KinectTimePosPlot(t,X,Y,Z,nbPoints,legende)
    figure;
    
    subplot(3,1,1);
    hold all;
    for i=1:nbPoints
        plot(t,X(:,i));
    end
    hold off;
    title('Abscisses au cours du temps');
    xlabel('t (s)');
    ylabel('X');
    
    subplot(3,1,2);
    hold all;
    for i=1:nbPoints
        plot(t,Y(:,i));
    end
    hold off;
    title('Ordonnées au cours du temps');
    xlabel('t (s)');
    ylabel('Y');

    subplot(3,1,3);
    hold all;
    for i=1:nbPoints
        plot(t,Z(:,i));
    end
    hold off;
    title('Profondeurs au cours du temps');
    xlabel('t (s)');
    ylabel('Z');
    legend(legende);
end
