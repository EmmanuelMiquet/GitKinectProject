function [ angle , pvect] = calculAnglePVect( centre,droite,gauche )
%UNTITLED2 Summary of this function goes here
%   Detailed explanation goes here
   i=1;
        vect1(i,:)=[droite(3*(i-1)+1)-centre(3*(i-1)+1),droite(3*(i-1)+2)-centre(3*(i-1)+2),droite(3*i)-centre(3*i)];
        vect2(i,:)=[gauche(3*(i-1)+1)-centre(3*(i-1)+1),gauche(3*(i-1)+2)-centre(3*(i-1)+2),gauche(3*i)-centre(3*i)];
        pvect= cross(vect1(i,:),vect2(i,:));
        angle=produitScalaire(vect1(i,:),vect2(i,:));
end

