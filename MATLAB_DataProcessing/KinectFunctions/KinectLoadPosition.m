function [t,dt,nbPoints,X,Y,Z,legende] = KinectLoadPosition(captureFolderPath,indexFile,relative)
    
    % Test du nombre d'arguments
    if nargin < 3
        relative = 0;
    end
    
    % Chargement des positions
    if(relative == 'relative')
        pointsCapture = load(strcat(captureFolderPath, num2str(indexFile), '_relativePointsCapture.txt'));
    else
        pointsCapture = load(strcat(captureFolderPath, num2str(indexFile), '_pointsCapture.txt'));
    end
    t = pointsCapture(:,1);
    dt = t(2)-t(1);
    nbPoints = ((length(pointsCapture(1,:))-1)/3);
    
    X = zeros(length(t),nbPoints);
    Y = zeros(length(t),nbPoints);
    Z = zeros(length(t),nbPoints);
    
    for i=1:nbPoints
        X(:,i) = pointsCapture(:,3*i-1);
        Y(:,i) = pointsCapture(:,3*i);
        Z(:,i) = pointsCapture(:,3*i+1);
    end
    
    % Chargement des legendes pour les anciennes versions de MATLAB
    fileLegend = fopen(strcat(captureFolderPath, num2str(indexFile), '_jointsLegend.txt'));
    legende = textscan(fileLegend,'%s');
    fclose(fileLegend);
    legende = legende{1};
    
    % Chargement des legendes pour les nouvelles versions de MATLAB
    %legende = table2array(readtable(strcat(captureFolderPath, num2str(indexFile), '_jointsLegend.txt'),'ReadVariableNames',false));
end

