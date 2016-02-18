%% Initialisation et chargement des vecteurs
close all;
clear all;
clc;

%Index of the file we want to load
indexFile = 0;

pointsCapture = load(strcat('../SkeletonTracking-WPF/bin/Debug/KinectCapture/', num2str(indexFile), '_pointsCapture.txt'));
t = pointsCapture(:,1);
dt = t(2)-t(1);
nbPoints = ((length(pointsCapture(1,:))-1)/3);
for i=1:nbPoints
    X(:,i) = pointsCapture(:,3*i-1);
    Y(:,i) = pointsCapture(:,3*i);
    Z(:,i) = pointsCapture(:,3*i+1);
end

% Load for older versions of MATLAB
fiLegend = fopen(strcat('../SkeletonTracking-WPF/bin/Debug/KinectCapture/', num2str(indexFile), '_jointsLegend.txt'));
legende = textscan(fiLegend,'%s');
fclose(fiLegend);
legende = legende{1};

% Load for newer versions of MATLAB
%legende = table2array(readtable('../SkeletonTracking-WPF/bin/Debug/jointsLegend.txt','ReadVariableNames',false));

%% Traces des positions dans l'espace
KinectSpacePlot(X,Z,Y,nbPoints,legende);

%% Calcul des derivees 1 et 2
for i=1:nbPoints
    [dx(:,i),dx2(:,i)] = VitAcc(X(:,i),dt);
    [dy(:,i),dy2(:,i)] = VitAcc(Y(:,i),dt);
    [dz(:,i),dz2(:,i)] = VitAcc(Z(:,i),dt);
end

%% Traces des positions/vitesses/accelerations
KinectTimePosPlot(t,X,Y,Z,nbPoints,legende);
KinectTimeVitPlot(t,dx,dy,dz,nbPoints,legende);
KinectTimeAccPlot(t,dx2,dy2,dz2,nbPoints,legende);