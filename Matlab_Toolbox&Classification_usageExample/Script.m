%% Initialisation
close all;
clear all;
clc;

% Add our functions to the path
addpath('KinectFunctions');

%% Data Load
% Index of the file we want to read
indexFile = 1;

% Path of the capture folder
captureFolderPath = 'ExperienceData/';

% Data loading
[t,dt,nbPoints,X,Y,Z,legende] = KinectLoadPosition(captureFolderPath,indexFile,'relative');
[~,~,nbAngles,Theta,legendeAngle] = KinectLoadAngles(captureFolderPath,indexFile);
[bonesLength,bonesNames] = KinectLoadBones(captureFolderPath,indexFile);
[Xn,Yn,Zn] = KinectNormalizePosition(X,Y,Z);

%% Plot spatial positions
KinectSpacePlot(X,Y,Z,nbPoints,legende);
title('Spatial positions of the joints');

%% Plot spatial relative positions
KinectSpacePlot(Xn,Yn,Zn,nbPoints,legende);
title('Relative positions of the joints');

%% Plot bones length
KinectBonesLengthPlot(bonesLength,bonesNames);

%% Derivate 1 & 2
% Position
dx = zeros(length(t)-2,nbPoints);
dx2 = zeros(length(t)-4,nbPoints);
dy = zeros(length(t)-2,nbPoints);
dy2 = zeros(length(t)-4,nbPoints);
dz = zeros(length(t)-2,nbPoints);
dz2 = zeros(length(t)-4,nbPoints);
for i=1:nbPoints
    [dx(:,i),dx2(:,i)] = VitAcc(X(:,i),dt);
    [dy(:,i),dy2(:,i)] = VitAcc(Y(:,i),dt);
    [dz(:,i),dz2(:,i)] = VitAcc(Z(:,i),dt);
end

% Angular
dTheta = zeros(length(t)-2,nbAngles);
dTheta2 = zeros(length(t)-4,nbAngles);
for i=1:nbAngles
    [dTheta(:,i),dTheta2(:,i)] = VitAcc(Theta(:,i),dt);
end

%% Plot Positions/Speeds/Accelerations
% Position
KinectTimePosPlot(t,X,Y,Z,nbPoints,legende);
KinectTimeVitPlot(t,dx,dy,dz,nbPoints,legende);
KinectTimeAccPlot(t,dx2,dy2,dz2,nbPoints,legende);

% Angular
KinectTimeAngPosPlot(t,Theta,nbAngles,legendeAngle);
KinectTimeAngVitPlot(t,dTheta,nbAngles,legendeAngle);
KinectTimeAngAccPlot(t,dTheta2,nbAngles,legendeAngle);