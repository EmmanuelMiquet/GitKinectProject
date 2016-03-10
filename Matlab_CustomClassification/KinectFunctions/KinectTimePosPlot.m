function [] = KinectTimePosPlot(t,X,Y,Z,nbPoints,legende)
    % Plot the positions values in time with legend

    figure;
    
    subplot(3,1,1);
    hold all;
    for i=1:nbPoints
        plot(t,X(:,i));
    end
    hold off;
    title('Horizontal positions in time');
    xlabel('t (s)');
    ylabel('X (m)');
    
    subplot(3,1,2);
    hold all;
    for i=1:nbPoints
        plot(t,Y(:,i));
    end
    hold off;
    title('Vertical positions in time');
    xlabel('t (s)');
    ylabel('Y (m)');

    subplot(3,1,3);
    hold all;
    for i=1:nbPoints
        plot(t,Z(:,i));
    end
    hold off;
    title('Depth positions in time');
    xlabel('t (s)');
    ylabel('Z (m)');
    legend(legende);

end
