function  [numeroBarycentre]=testMouvement1(TabBary,PointTest)
%UNTITLED2 Summary of this function goes here
%   Detailed explanation goes here

pointClasse=[];
pointClasse(1)=PointTest{1}(1);
pointClasse(2)=PointTest{1}(2);

%rentrer n rentrer struc2
n=size(TabBary,1);

%Initialisation
T=[];
X=[];

%classement position de la plus haute à la plus basse
rangTabBary=[];
TabBaryResize=[];

m=size(TabBary,1);
loc_val_max=0;
nbreFin=m;
rang=1;

while nbreFin > 0
    for j=1:1:m
        if TabBary(j,2)>= loc_val_max
            loc_val_max=TabBary(j,2);
            rang=j;
        end
    end
    TabBaryResize(nbreFin,:)=TabBary(rang,:);
    nbreFin=nbreFin-1;
    TabBary( rang,2)=0;
    loc_val_max=0;
end

%On reforme le tableau des barycentres ordonée
TabBary=TabBaryResize;
for i=1:1:n
    %revoir appellation
    ThetaX=TabBary(i,1);
    ThetaY=TabBary(i,2);
    T=[T sqrt((pointClasse(1)-ThetaX)^2+(pointClasse(2)-ThetaY)^2)];
end

%fonction renvoie le min et donc la classe la plus proche
[M,numeroBarycentre]=min(T);
    

    
end

