function [bonesLength,bonesNames] = KinectLoadBones(captureFolderPath,indexFile)
    % Load the bones length values and names from a capture file
    
    % Bones length and names loading
    fileBonesLength = fopen(strcat(captureFolderPath, num2str(indexFile), '_bonesLength.txt'));
    bonesLength = textscan(fileBonesLength,'%s %s %f %s %f','Delimiter',{'\n','\t'});
    fclose(fileBonesLength);
    
    bonesNames = [bonesLength{2};bonesLength{4}];
    bonesLength = [bonesLength{3};bonesLength{5}];
    [bonesLength,bonesNames] = KinectUniqueBonesFilter(bonesLength,bonesNames);

end

