function [] = KinectSpacePlot(X,Z,Y,nbPoints,legende)
    figure;
    hold all;
    for i=1:nbPoints
        plot3(X(:,i),Z(:,i),Y(:,i));
    end
    hold off;
    xlabel('x');
    ylabel('z');
    zlabel('y');
    legend(legende);
    grid on;
end
