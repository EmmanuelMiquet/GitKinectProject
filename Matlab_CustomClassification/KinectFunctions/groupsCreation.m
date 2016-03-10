function [groups] = groupsCreation(indiceChange,dataLength)
    % Generates a vector of classes 
    % Uses the vector indices Change, that indicates where the 
    % changes between classes are for the data set used for the classifier 
    % Also receives the values that indicates the number of data for 
    % training. With that, the function makes a vector of classes  of the same
    % length as the data set used for training
    
    groups = zeros(dataLength,1);

    for i = 1:dataLength
        for k = 1:length(indiceChange)
            if k == length(indiceChange)
                if (i > indiceChange(k))
                    groups(i,1) = k;
                end
            else
                if (i > indiceChange(k) && i <= indiceChange(k+1))
                    groups(i,1) = k;
                end
            end
        end
    end      

end

