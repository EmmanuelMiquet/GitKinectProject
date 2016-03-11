function [ anglesData ] = angleTransformation( pointsCapture,anglesData )
%UNTITLED Summary of this function goes here
%   Detailed explanation goes here
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
   
    anglesData(i,8)=calculAnglePVectSigne(CoordEPauleD,CoordEPauleG,CoordCoudeD,CoordHipC,CoordSpine);
    anglesData(i,13)=calculAnglePVectSigne(CoordEPauleG,CoordEPauleD,CoordCoudeG,CoordHipC,CoordSpine);
    anglesData(i,3)=calculAnglePVectSigne1(CoordSpine,CoordEPauleC,CoordHipC, CoordHipR, CoordHipL);
end

    
end

