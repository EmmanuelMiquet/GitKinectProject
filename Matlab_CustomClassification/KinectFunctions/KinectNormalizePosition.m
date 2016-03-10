function [Xn,Yn,Zn] = KinectNormalizePosition(X,Y,Z)
    % Normalize the positon data to get rid of variation between people's
    % height
    
    % Normalisation
    norm = max(max(sqrt(X.^2 + Y.^2 + Z.^2)));
    Xn = X/norm;
    Yn = Y/norm;
    Zn = Z/norm;

end

