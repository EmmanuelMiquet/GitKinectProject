clear all
close all
clc
%% Lecture des points
load('pointsCapture.txt');

%% Extraction des points "pointsCapteurs"

%Addition de la hauteur Kinect
addKin=0.8;

% le "i" indique les joints trackés
for i = 1:1:5
X(:,i)=pointsCapture(:,3*(i-1)+2);
Y(:,i)=pointsCapture(:,3*(i-1)+3)+addKin;
Z(:,i)=pointsCapture(:,3*(i-1)+4);

%tableau XYZ utilisé par Kmean
XYZ(:,3*i+1)=X(:,i);
XYZ(:,3*i+2)=Y(:,i);
XYZ(:,3*i+3)=Z(:,i);

end

%% Kmean: on trouve les positions

idx=[];
C=[];
sumd=[];


% Nombre de classes à former
nbreClass=5;

% Dire sur
i=5;
[G,C(:,1+3*(i-1):3*i)]=kmeans(XYZ(:,3*i+1:3*(i+1)),nbreClass);


%% Figures

%# show points and clusters (color-coded)
K=5;
clr = lines(K);
figure, 

for i=1:1:3
scatter3(X(:,i), Z(:,i),  Y(:,i),'k', 'Marker','.')
hold on
end

for i=4:1:5
scatter3(X(:,i), Z(:,i),  Y(:,i), 36, clr(G,:), 'Marker','.')
hold on
end


for j=1:1:nbreClass
        scatter3(C(j,1+3*(i-1)), C(j,3*i),C(j,2+3*(i-1)),  'Marker','o', 'LineWidth',20)
        hold on
end

%Tableau contenant les coordonnées des barycentres 
%en fonction du nbre de bary choisis (ici 5)
for i=1:1:nbreClass
    WristleTabBary(i,1:3)=C(i,13:15);
end
TabBary=WristleTabBary;

%Vecteur/Variables avec les angles et paramètres corporels
thetaCoudeMin=[170 165 170 150 1600];% au hasard pr le moment
hauteurRef=[]



%% Point testé du poignet 
		
x=0.70284370;
y=0.2614180+addKin;
z=2.33234400;
thetaCoude=165;
PointTest=[x y z thetaCoude];

%les classes sont ordonnées dans le sens croissant de la hauteur (Y) [la 1
%en bas, puis la 2 plus haut ...
[numeroBarycentre]=testMouvement1(TabBary,PointTest);

scatter3(x,z,y,'r+','LineWidth',30);
hold off
view(3), axis vis3d, box on, rotate3d on
xlabel('x'), ylabel('z'), zlabel('y')

%% Affichage du résultat
fprintf('Tentative du mouvement: ')
switch numeroBarycentre 
    case 1
        disp('1');
    case 2
        disp('2');
    case 3
        disp('3');
    case 4
        disp('4');
    case 5
        disp('5');
end

fprintf('\n Etat de réalisation:\n ');
donneesMove= [4.96666700	0.08105250	0.25084980	2.52666200	0.07233410	0.60274120	2.53179400	0.27216870	0.53202610	2.53143200	0.56530650	0.55926330	2.55801200	0.80410040	0.56948260	2.52784500];
%doit avoir acces à toutes les info!!
    critiqueMouvemet( numeroBarycentre,thetaCoudeMin,donneesMove );


