function [angleSigne ] = calculAnglePVectSigne(CoordEPauleD,CoordEPauleG,CoordCoudeD,CoordHip,CoordSpine)
%UNTITLED2 Summary of this function goes here
%   Detailed explanation goes here

i=1;
    axeEpaule(i,:)=[CoordEPauleD(i,1)-CoordEPauleG(i,1),CoordEPauleD(i,2)-CoordEPauleG(i,2),CoordEPauleD(i,3)-CoordEPauleG(i,3)];
    axeBuste(i,:)=[CoordSpine(i,1)-CoordHip(i,1),CoordSpine(i,2)-CoordHip(i,2),CoordSpine(i,3)-CoordHip(i,3)];
    axeDorsoVentral=[axeEpaule(i,3)*axeBuste(i,2)-axeEpaule(i,2)*axeBuste(i,3),axeEpaule(i,1)*axeBuste(i,3)-axeEpaule(i,3)*axeBuste(i,1),axeEpaule(i,2)*axeBuste(i,1)-axeEpaule(i,1)*axeBuste(i,2)];

 [angleSigne,pvect]=calculAnglePVect(CoordEPauleD,CoordEPauleG,CoordCoudeD);
 
%projection pvect dans le plan dorsoVentral
 pS=dot(axeDorsoVentral,pvect);
 normDV=norm(axeDorsoVentral);
 pvectProj=(pS/(normDV^2))*axeDorsoVentral;

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
     pSProj=dot(axeDorsoVentral,pvectProj);
%      normPVectProj=norm(pvectProj);
%      absAng=abs(acos(pSProj/(normDV*normPVectProj)));
     if pSProj <0
         angleSigne(i)=360-angleSigne;
     end
end     
 

