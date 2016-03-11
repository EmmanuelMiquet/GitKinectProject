function [angleSigne ] = calculAnglePVectSigne1(CoordSpine,CoordEPauleC,CoordHipC, CoordHipR, CoordHipL)
%UNTITLED2 Summary of this function goes here
%   Detailed explanation goes here

i=1;
    axeBassin(i,:)=[CoordHipL(i,1)-CoordHipR(i,1),CoordHipL(i,2)-CoordHipR(i,2),CoordHipL(i,3)-CoordHipR(i,3)];
    axeBuste(i,:)=[CoordSpine(i,1)-CoordHipC(i,1),CoordSpine(i,2)-CoordHipC(i,2),CoordSpine(i,3)-CoordHipC(i,3)];
    axeBassinVentral=[axeBassin(i,3)*axeBuste(i,2)-axeBassin(i,2)*axeBuste(i,3),axeBassin(i,1)*axeBuste(i,3)-axeBassin(i,3)*axeBuste(i,1),axeBassin(i,2)*axeBuste(i,1)-axeBassin(i,1)*axeBuste(i,2)];
    
    
 [angleSigne,pvect]=calculAnglePVect1(CoordSpine,CoordHipC, CoordEPauleC,axeBassin);
 
%projection pvect dans le plan dorsoVentral
 pS=dot(axeBassinVentral,pvect);
 normDV=norm(axeBassinVentral);
 pvectProj=(pS/(normDV^2))*axeBassinVentral;

 %Affichage des vecteurs résultants du produits vectoriel et de leur
 %projeté
%  figure(1)
%  quiver3(0,0,0,axeDorsoVentral(1),axeDorsoVentral(2),axeDorsoVentral(3));
%  hold on
%  quiver3(0,0,0,pvect(1),pvect(2),pvect(3));
%  hold on
%  quiver3(0,0,0,pvectProj(1),pvectProj(2),pvectProj(3));
%  hold off


 %Calcul du signe
     pSProj=dot(axeBassinVentral,pvectProj);
     if pSProj <0
         angleSigne(i)=360-angleSigne;
     end
end

