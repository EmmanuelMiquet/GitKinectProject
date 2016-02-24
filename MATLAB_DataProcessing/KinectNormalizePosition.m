function [Xn,Yn,Zn] = KinectNormalizePosition(X,Y,Z)
    
    % Normalisation
    norm = max(max(sqrt(X.^2 + Y.^2 + Z.^2)));
    Xn = X/norm;
    Yn = Y/norm;
    Zn = Z/norm;

end

