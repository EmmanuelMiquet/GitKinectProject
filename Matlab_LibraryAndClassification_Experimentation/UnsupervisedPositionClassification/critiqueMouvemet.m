function [ statut ] = critiqueMouvemet( mouvement,thetaCoudeMin, donneesMove)
%UNTITLED Summary of this function goes here
%   Detailed explanation goes here

thetaCoude= 168;
hauteurEp1=0.7;
hauteurEp2=0.9;



%renvoie erreur (statut=0 ou pas statut=1
statut=0;

%contrainte angulaire fct de la classe
if thetaCoude < thetaCoudeMin(mouvement)
    statut=1;
    fprintf('Coude pas assez tendu\n');
end

%contrainte d(alignement (pour les épaules)
if hauteurEp1>hauteurEp2+0.2 || hauteurEp1<hauteurEp2-0.2
    statut=1;
    fprintf('Epaules non alignées\n');
    
    %%on pourrait aussi faire 'mauvais gainage dorsal'

if statut ==1
    fprintf('Position correcte\n');
end

end

