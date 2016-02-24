%% Initialisation et chargement des vecteurs
close all;
clear all;
clc;

% Index des fichiers � lire
indexFile = 0;

% Chemin du dossier de capture
captureFolderPath = '../SkeletonTracking-WPF/bin/Debug/KinectCapture/';

% Chargement des donn�es
[t,dt,nbPoints,X,Y,Z,legende] = KinectLoadPosition(captureFolderPath,indexFile,'relative');
[~,~,nbAngles,Theta,legendeAngle] = KinectLoadAngles(captureFolderPath,indexFile);
[bonesLength,bonesNames] = KinectLoadBones(captureFolderPath,indexFile);
[Xn,Yn,Zn] = KinectNormalizePosition(X,Y,Z);

%% Traces des positions dans l'espace
KinectSpacePlot(X,Z,Y,nbPoints,legende);
title('Positions spatiales des joints');

%% Traces des positions normalisees dans l'espace
KinectSpacePlot(Xn,Zn,Yn,nbPoints,legende);
title('Positions relatives des joints');

%% Traces des longueurs des membres
KinectBonesLengthPlot(bonesLength,bonesNames);

%% Calcul des derivees 1 et 2
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

% Angulaire
dTheta = zeros(length(t)-2,nbAngles);
dTheta2 = zeros(length(t)-4,nbAngles);
for i=1:nbAngles
    [dTheta(:,i),dTheta2(:,i)] = VitAcc(Theta(:,i),dt);
end

%% Traces des positions/vitesses/accelerations
% Position
KinectTimePosPlot(t,X,Y,Z,nbPoints,legende);
KinectTimeVitPlot(t,dx,dy,dz,nbPoints,legende);
KinectTimeAccPlot(t,dx2,dy2,dz2,nbPoints,legende);

% Angulaire
KinectTimeAngPosPlot(t,Theta,nbAngles,legendeAngle);
KinectTimeAngVitPlot(t,dTheta,nbAngles,legendeAngle);
KinectTimeAngAccPlot(t,dTheta2,nbAngles,legendeAngle);