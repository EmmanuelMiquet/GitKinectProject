function [] = KinectTimeAngVitPlot(t,dTheta,nbAngles,legendeAngle)
    % Plot the angles speed in time with legend

    figure;
    hold all;
    for i=1:nbAngles
        plot(t(1:length(dTheta(:,i))-4),dTheta((1:length(dTheta(:,i))-4),i));
    end
    hold off;
    title('Anglular speeds in time');
    xlabel('t (s)');
    ylabel('d\Theta/dt (Deg/s)');
    legend(legendeAngle);

end