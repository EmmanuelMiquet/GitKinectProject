function [X,Y,Z] = KinectCleanSingularity(X,Y,Z,nbPoints,dt)
    %Fonction obsolète (pris en charge par le C#)
    ndt = 0;
    for i=1:nbPoints
        for j = 1:length(X(:,i))
           if(X(j,i) == 0)
               for k = j:length(X(:,i))
                   ndt = ndt + 1;
                   if(X(k,i) ~= 0)
                       break;
                   end
               end
               if(k ~= length(X(:,i)))
                   if(j == 1)
                       X(j,i) = X(k,i);
                   else
                       X(j,i) = ((X(k,i) - X(j-1,i))/(ndt*dt))*dt+X(j-1,i);
                   end
               else
                   X(j,i) = X(j-1,i);
               end
               ndt = 0;
           end
           if(Y(j,i) == 0)
              for k = j:length(Y(:,i))
                   ndt = ndt + 1;
                   if(Y(k,i) ~= 0)
                       break;
                   end
              end 
              if(k ~= length(Y(:,i)))
                  if(j == 1)
                       Y(j,i) = Y(k,i);
                  else
                       Y(j,i) = ((Y(k,i) - Y(j-1,i))/(ndt*dt))*dt+Y(j-1,i);
                  end
              else
                    Y(j,i) = Y(j-1,i);
              end
              ndt = 0;
           end
           if(Z(j,i) == 0)
              for k = j:length(Z(:,i))
                   ndt = ndt + 1;
                   if(Z(k,i) ~= 0)
                       break;
                   end
              end 
              if(k ~= length(Z(:,i)))
                  if(j == 1)
                       Z(j,i) = Z(k,i);
                  else
                       Z(j,i) = ((Z(k,i) - Z(j-1,i))/(ndt*dt))*dt+Z(j-1,i);
                  end
              else
                    Z(j,i) = Z(j-1,i);
              end
              ndt = 0;
           end
        end
    end
    
end

