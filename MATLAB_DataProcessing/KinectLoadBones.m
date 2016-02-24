function [bonesLength,bonesNames] = KinectLoadBones(captureFolderPath,indexFile)
    
    % Chargement des legendes pour les anciennes versions de MATLAB
    fileBonesLength = fopen(strcat(captureFolderPath, num2str(indexFile), '_bonesLength.txt'));
    bonesLength = textscan(fileBonesLength,'%s %s %f %s %f','Delimiter',{'\n','\t'});
    fclose(fileBonesLength);
    
    bonesNames = [bonesLength{2};bonesLength{4}];
    bonesLength = [bonesLength{3};bonesLength{5}];
    [bonesLength,bonesNames] = KinectUniqueBonesFilter(bonesLength,bonesNames);

end

