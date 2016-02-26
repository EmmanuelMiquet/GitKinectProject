function [Xn,Yn,Zn] = KinectNormalizePosition(X,Y,Z)
    % Does a normalisation of data, in which every point X,Y,Z 
    % in the space goes to a space between 0 - 1. This funcition receive,
    % a matrix n,m of data in X, other in Y, and finally in Z. Then it
    % search the maximum value in the matrix of disctance for each point
    % X,Y,Z. Finally with this norm value, the function does the 
    % normalisation dividing all the values for each matrix by the norm
    
    % Normalisation
    norm = max(max(sqrt(X.^2 + Y.^2 + Z.^2)));
    Xn = X/norm;
    Yn = Y/norm;
    Zn = Z/norm;

end

