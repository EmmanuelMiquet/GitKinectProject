function [] = KinectTimeAngVitPlot(t,dTheta,nbAngles,legendeAngle)
    
    figure;
    hold all;
    for i=1:nbAngles
        plot(t(1:length(dTheta(:,i))-4),dTheta((1:length(dTheta(:,i))-4),i));
    end
    hold off;
    title('Vitesses angulaires au cours du temps');
    xlabel('t (s)');
    ylabel('d\Theta/dt (Deg/s)');
    legend(legendeAngle);

end