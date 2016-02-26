function [d] = meanDistClass(class,tmpClasses,distance_t)
    % When you make a prediction using more than one nearest 
    % neighbor, this function calculates the average distance of the 
    % classes found in the k nearest neighbors. Then the function returns
    % the mean distance between the distances calculated for each class
    % found
    
    d = 0;
    c = 0;
    
    for i = 1:length(tmpClasses)
       if tmpClasses(i) == class
          d = d + distance_t(i);
          c = c + 1;
       end
    end
    
    if c ~= 0
        d = d/c;
    else
        d = Inf;
    end
end

