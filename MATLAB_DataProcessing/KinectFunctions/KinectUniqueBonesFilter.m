function [uBonesLength,uBonesNames] = KinectUniqueBonesFilter(bonesLength,bonesNames)
    % Filter the bones length vector and the bones names to eliminate
    % duplicates values

    match = zeros(length(bonesLength)); % Initialization of local variables
    uBonesLength = [];
    uBonesNames = {};
    
    for i=1:length(bonesLength)
        for j=i:length(bonesLength)
           if(bonesLength(i) ~= bonesLength(j))
               match(i,j) = 1; % If length are different
           else
               match(i,j) = 0; % If length are the same
           end
        end
        
        if(sum(match(i,:)) == length(bonesLength)-i)
            uBonesLength = [uBonesLength ; bonesLength(i)];
            uBonesNames = [uBonesNames ; bonesNames{i}];
        end
    end
    
end

