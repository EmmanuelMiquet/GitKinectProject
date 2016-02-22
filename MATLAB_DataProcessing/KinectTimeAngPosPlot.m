function [] = KinectTimeAngPosPlot(t,Theta,nbAngles,legendeAngle)
    figure;
    hold all;
    for i=1:nbAngles
        plot(t,Theta(:,i));
    end
    hold off;
    title('Angles au cours du temps');
    xlabel('t (s)');
    ylabel('\Theta (Deg)');
    legend(legendeAngle);
end
