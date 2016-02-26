function [t,dt,nbAngles,Theta,legendeAngle] = KinectLoadAngles(captureFolderPath,indexFile)
    % Load the angles values and legend from a capture file
    
    % Angles loading
    anglesCapture = load(strcat(captureFolderPath, num2str(indexFile), '_anglesData.txt'));
    t = anglesCapture(:,1);
    dt = t(2)-t(1);
    nbAngles = length(anglesCapture(1,:))-1;
    
    Theta = zeros(length(t),nbAngles); % Initialisation of the variable
    
    for i=1:nbAngles
        Theta(:,i) = anglesCapture(:,i+1);
    end
    
    % Angles legend loading
    fileAngleLegend = fopen(strcat(captureFolderPath, num2str(indexFile), '_anglesLegend.txt'));
    legendeAngle = textscan(fileAngleLegend,'%s','Delimiter','\n');
    fclose(fileAngleLegend);
    legendeAngle = legendeAngle{1};
end

