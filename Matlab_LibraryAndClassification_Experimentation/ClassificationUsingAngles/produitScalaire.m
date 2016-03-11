function [ ang ] = produitScalaire( U,V )
%UNTITLED Summary of this function goes here
%   Detailed explanation goes here
    UX=U(1);
    UY=U(2);
    UZ=U(3);
    VX=V(1);
    VY=V(2);
    VZ=V(3);
    scalUV=dot(U,V);
    normU=sqrt(UX^2+UY^2+UZ^2);
    normV=sqrt(VX^2+VY^2+VZ^2);
 
  ang=(180/pi)*acos(scalUV/(normU*normV));
  
end
