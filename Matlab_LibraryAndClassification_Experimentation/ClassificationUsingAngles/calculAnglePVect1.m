function [ angle , pvect] = calculAnglePVect1( centre,droite,gauche,axeBassin )
%UNTITLED2 Summary of this function goes here
%   Detailed explanation goes here
   i=1;
        vect1(i,:)=[droite(3*(i-1)+1)-centre(3*(i-1)+1),droite(3*(i-1)+2)-centre(3*(i-1)+2),droite(3*i)-centre(3*i)];
        vect2(i,:)=[gauche(3*(i-1)+1)-centre(3*(i-1)+1),gauche(3*(i-1)+2)-centre(3*(i-1)+2),gauche(3*i)-centre(3*i)];
        
        %projection du vecteur Spine-shoulderCenter(vect1) par rapport à
        %HipCenter-Spine(vect2)
         pS=dot(vect1,vect2);
         normDV=norm(vect2);
         vect12=(pS/(normDV^2))*vect2;
        
         %projection du vecteur Spine-shoulderCenter(vect1) par rapport à
        %HipRight-Left(vect2)
         pS=dot(vect1,axeBassin);
         normDV=norm(axeBassin);
         vect13=(pS/(normDV^2))*axeBassin;
         
         vect14=vect12 + vect13;
         
%           figure(2)
%  quiver3(0,0,0,vect1(i,1),vect1(i,2),vect1(i,3));
%  hold on
%  quiver3(0,0,0,vect2(i,1),vect2(i,2),vect2(i,3));
%  hold on
%  quiver3(0,0,0,vect12(i,1),vect12(i,2),vect12(i,3));
%   hold on
%  quiver3(0,0,0,vect13(i,1),vect13(i,2),vect13(i,3));
%    hold on
%  quiver3(0,0,0,vect14(i,1),vect14(i,2),vect14(i,3));
% hold off
        
        pvect= cross(vect1(i,:),vect2(i,:));
        angle=produitScalaire(vect14(i,:),vect2(i,:));
end