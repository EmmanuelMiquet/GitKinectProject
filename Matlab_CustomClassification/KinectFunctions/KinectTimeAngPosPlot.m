function [] = KinectTimeAngPosPlot(t,Theta,nbAngles,legendeAngle)
    % Plot the angles values in time with legend

    figure;
    hold all;
    for i=1:nbAngles
        plot(t,Theta(:,i));
    end
    hold off;
    title('Angles values in time');
    xlabel('t (s)');
    ylabel('\Theta (Deg)');
    legend(legendeAngle);

end
