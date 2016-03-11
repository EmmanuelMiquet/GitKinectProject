function [ ang ,pvect] = produitVectoriel( U,V )
%UNTITLED Summary of this function goes here
%   Detailed explanation goes here
    UX=U(1);
    UY=U(2);
    UZ=U(3);
    VX=V(1);
    VY=V(2);
    VZ=V(3);
    normUV=sqrt((UY*VZ-VY*UZ)^2+(UZ*VX-UX*VZ)^2+(UX*VY-VX*UY)^2);
    normU=sqrt(UX^2+UY^2+UZ^2);
    normV=sqrt(VX^2+VY^2+VZ^2);
 
  pvect = [UY*VZ-VY*UZ ; UZ*VX-UX*VZ ; UX*VY-VX*UY];
  ang=(180/pi)*asin(normUV/(normU*normV));
  
end

