function [] = KinectTimeAccPlot(t,dx2,dy2,dz2,nbPoints,legende)
    % Plot the acceleration in time with legend
    
    figure;
    
    subplot(3,1,1);
    hold all;
    for i=1:nbPoints
        plot(t(1:length(dx2(:,i))-4),dx2((1:length(dx2(:,i))-4),i));
    end
    hold off;
    title('Horizontal accelerations in time');
    xlabel('t (s)');
    ylabel('d^2X/dt (m/s^2)');
    
    subplot(3,1,2);
    hold all;
    for i=1:nbPoints
        plot(t(1:length(dy2(:,i))-4),dy2((1:length(dy2(:,i))-4),i));
    end
    hold off;
    title('Vertical accelerations in time');
    xlabel('t (s)');
    ylabel('d^2Y/dt (m/s^2)');

    subplot(3,1,3);
    hold all;
    for i=1:nbPoints
        plot(t(1:length(dz2(:,i))-4),dz2((1:length(dz2(:,i))-4),i));
    end
    hold off;
    title('Depth accelerations in time');
    xlabel('t (s)');
    ylabel('d^2Z/dt (m/s^2)');
    legend(legende);

end
