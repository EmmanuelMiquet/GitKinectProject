function [uBonesLength,uBonesNames] = KinectUniqueBonesFilter(bonesLength,bonesNames)
    
    match = zeros(length(bonesLength)); % Initialisation de la matrice de correspondance
    uBonesLength = [];
    uBonesNames = {};
    
    for i=1:length(bonesLength)
        for j=i:length(bonesLength)
           if(bonesLength(i) ~= bonesLength(j))
               match(i,j) = 1; % Si les longueurs sont differentes
           else
               match(i,j) = 0; % Si les longueurs sont identiques
           end
        end
        
        if(sum(match(i,:)) == length(bonesLength)-i)
            uBonesLength = [uBonesLength ; bonesLength(i)];
            uBonesNames = [uBonesNames ; bonesNames{i}];
        end
    end
    
end

