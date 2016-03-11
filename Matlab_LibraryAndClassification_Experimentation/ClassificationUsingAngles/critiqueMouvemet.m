function [ statut ] = critiqueMouvemet( classe,angTabRef, PointTest)
%UNTITLED Summary of this function goes here
%   Detailed explanation goes here

%POUR LE MOMENT, LES ANGLES DE REFERENCES NE DEPENDENT PAS DU MOUVEMENT

% Extraction des bornes du mouvement
thetaCoudeMin=angTabRef{1}(1);
thetaCoudeMax=angTabRef{1}(2);
thetaTronMin=angTabRef{2}(1);
thetaTronMax=angTabRef{2}(2);

% Extraction des points du mouvement
thetaTron=PointTest{1}(3);
thetaCoude=PointTest{1}(4);
hauteurEp1=PointTest{2};
hauteurEp2=PointTest{3};

%renvoie erreur (statut=0 ou pas statut=1
statut=0;

%contrainte angulaire coude en fct de la classe
if thetaCoude < thetaCoudeMin
    statut=1;
    fprintf('Coude pas assez tendu\n');
end

%contrainte angulaire colonne en fct de la classe
if (thetaTron < thetaTronMin || thetaTron > thetaTronMax)
    statut=1;
    fprintf('Colonne pas droite\n');
end

%contrainte d(alignement (pour les épaules)
hEp2max=hauteurEp2+0.2;
hEp2min=hauteurEp2-0.2;
if (hauteurEp1>hEp2max || hauteurEp1<hEp2min)
    statut=1;
    fprintf('Epaules non alignées\n');
    
    %%on pourrait aussi faire 'mauvais gainage dorsal'

if statut ==0
    fprintf('Position correcte\n');
end

end

