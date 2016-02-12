function [d,d2] = VitAcc(x,dt)
    d = zeros(length(x)-2,1);

    for i= 1:length(x)-2
        d(i) = (((x(i+1) - x(i))/dt) + ((x(i+2) - x(i+1))/dt))/2;
    end
    
    d2 = zeros(length(d)-2,1);

    for i= 1:length(d)-2
        d2(i) = (((d(i+1) - d(i))/dt) + ((d(i+2) - d(i+1))/dt))/2;
    end
end

