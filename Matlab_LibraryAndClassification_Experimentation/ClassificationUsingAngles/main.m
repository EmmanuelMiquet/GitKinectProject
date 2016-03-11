clear all
close all
clc

% ATTENTION, TOUTS LES JOINTS DOIVENT ETRE ENREGISTRE POUR CE
% PROGRAMMES (sinon probleme de rang)

%% Recuperation des points

load ('Test/0_anglesData.txt');
load('Test/0_pointsCapture.txt')

%on change le nom pour adapter au programme
pointsCapture=X0_pointsCapture;
anglesData=X0_anglesData;

%% Organisation des points

angTron=anglesData(:,3);
angCoudeD=anglesData(:,4);
angEpauleD(:,1)=anglesData(:,8);
angEpauleG(:,1)=anglesData(:,12);
angSpine(:,1)=anglesData(:,3);

%Produit vectoriel pour l'épaule
for i=1:1:size(pointsCapture,1)
    CoordHipC=   [pointsCapture(i,2),pointsCapture(i,3),pointsCapture(i,4)];
    CoordSpine=  [pointsCapture(i,5),pointsCapture(i,6),pointsCapture(i,7)];
    CoordEPauleC=[pointsCapture(i,2+2*3),pointsCapture(i,3*3),pointsCapture(i,1+3*3)];
    CoordEPauleG=[pointsCapture(i,2+4*3),pointsCapture(i,5*3),pointsCapture(i,1+5*3)];
    CoordCoudeG= [pointsCapture(i,2+5*3),pointsCapture(i,6*3),pointsCapture(i,1+6*3)];
    CoordEPauleD=[pointsCapture(i,2+7*3),pointsCapture(i,8*3),pointsCapture(i,1+8*3)];
    CoordCoudeD= [pointsCapture(i,2+8*3),pointsCapture(i,9*3),pointsCapture(i,1+9*3)];
    CoordHipR=   [pointsCapture(i,2+10*3),pointsCapture(i,11*3),pointsCapture(i,1+11*3)];
    CoordHipL=   [pointsCapture(i,2+13*3),pointsCapture(i,14*3),pointsCapture(i,1+14*3)];
   
   
    
    angEpauleD(i,2)=calculAnglePVectSigne(CoordEPauleD,CoordEPauleG,CoordCoudeD,CoordHipC,CoordSpine);
    angEpauleG(i,2)=calculAnglePVectSigne(CoordEPauleG,CoordEPauleD,CoordCoudeG,CoordHipC,CoordSpine);
    angSpine(i,2)=calculAnglePVectSigne1(CoordSpine,CoordEPauleC,CoordHipC, CoordHipR, CoordHipL);
end

yEpauleD=pointsCapture(:,12);
yEpauleG=pointsCapture(:,24);
diffEpaule=abs(yEpauleD-yEpauleG);

%% Transformation en cercle
angCoudeTronX=cos(pi/180*angSpine(:,2));
angCoudeTronY=sin(pi/180*angSpine(:,2));

angCoudeDCercleX=cos(pi/180*angCoudeD);
angCoudeDCercleY=sin(pi/180*angCoudeD);

angEpauleDCercleX=cos(pi/180*angEpauleD(:,2));
angEpauleDCercleY=sin(pi/180*angEpauleD(:,2));


%% Kmean: on trouve les positions
KEpaule=[angEpauleDCercleX,angEpauleDCercleY];

idx=[];
C=[];
sumd=[];

%nbreDeTypeDePoints
nTP=5;

% On met le nombre de barycentres
nbreBary=10;
for i=1:1:nTP
   [idx,C]=kmeans(KEpaule,nbreBary);
end

%% Point Test
load('MauvaisDataBase/pointsCapture.txt')
epauleghaut=pointsCapture(16,12);
epauledhaut=pointsCapture(16,24);
diffEpauleTest=abs(epauleghaut-epauledhaut);
%1:angle [epaule,Tron,Coude] 2:epaules dte gche
PointTest={[cos(156.963112977664*(pi/180)) , sin(156.963112977664*(pi/180)),175,165.362821194248],epauleghaut,epauledhaut};

[numeroBarycentre]=testMouvement1(C,PointTest);
fprintf('Tentative du mouvement: %d \n',numeroBarycentre);

%% Vraissemblance

%Référence Coude Droit
angCoudeDRefMin=min(angCoudeD);
angCoudeDRefMax=max(angCoudeD);

angFenCoudeDDeg=[angCoudeDRefMin,angCoudeDRefMax];
angFenCoudeDRad=angFenCoudeDDeg*(pi/180);

%Référence colonne
angTronRefMin=min(angTron);
angTronRefMax=max(angTron);

angFenTronDeg=[angTronRefMin,angTronRefMax];
angFenTronRad=angFenTronDeg*(pi/180);

%Réunir tous les fenétrages 
angTabRef={[angFenCoudeDRad],[angFenTronRad]};

fprintf('Qualité du mouvement:')
critiqueMouvemet( numeroBarycentre,angTabRef,PointTest)


%% Affichage

figure('Name','Modèle')
subplot(2,2,1)
plot(cos(PointTest{1}(3)*(pi/180)),sin(PointTest{1}(3)*(pi/180)),'g*','MarkerSize',10,'LineWidth',10);
hold on
plot(angCoudeTronX,angCoudeTronY,'b+');
title('Hip Angle ');
hold off

subplot(2,2,2)
plot(cos(PointTest{1}(4)*(pi/180)),sin(PointTest{1}(4)*(pi/180)),'g*','MarkerSize',10,'LineWidth',10);
hold on
plot(angCoudeDCercleX,angCoudeDCercleY,'b+');
title('Right Elbow');
hold off

subplot(2,2,3)
plot(angEpauleDCercleX,angEpauleDCercleY,'b+');
hold on
plot(PointTest{1}(1),PointTest{1}(2),'g+','LineWidth',10)
for i=1:1:size(C,2)
plot(C(:,1),C(:,2),'r*','MarkerSize',10,'LineWidth',10);
hold on
end
hold off
title('Right shoulder');

subplot(2,2,4)
plot(angEpauleD(:,1),diffEpaule);
hold on
%plot(angEpauleD(:,1),diffEpauleTest,'g+','LineWidth',10);
title('Differential shoulders');

