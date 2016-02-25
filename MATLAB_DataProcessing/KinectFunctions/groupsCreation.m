function [groups] = groupsCreation(indiceChange,dataLength)

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

