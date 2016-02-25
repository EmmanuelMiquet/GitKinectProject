function [d] = meanDistClass(class,tmpClasses,distance_t)
    
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

