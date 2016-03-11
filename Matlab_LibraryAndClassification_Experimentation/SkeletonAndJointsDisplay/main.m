clear all
close all 
clc 

%% Capteurs de type Kinnect 
info=imaqhwinfo('Kinect');


%% Test death sensor, take joints et put the coordinates on a tab

clc;
clear all; 
close all;

tableau_TPS=[];
tableau_X=[];
tableau_Y=[];
tableau_Z=[];
timeIsUp = 0 ;
sz=0;

imaqreset; % Disconnect and delete all image acquisition objects
depthVid=videoinput('Kinect',2);


%%properties

%Depth vision
%mode standing ou Seated
set(getselectedsource(depthVid),'BodyPosture','standing');


triggerconfig(depthVid,'manual');
depthVid.FramesPerTrigger=1;
depthVid.TriggerRepeat=inf;
set(getselectedsource(depthVid),'TrackingMode','Skeleton');


start(depthVid);

%attend une touche
fprintf('Wait');  
fprintf('\nPress any key: ') ;
   ch = getkey ;
   fprintf('%c\n',ch) ;

   % timeisup => temps de record
while (timeIsUp <= 200)
    
trigger(depthVid);
[depthMap,~,depthMetaData]=getdata(depthVid);
%On spécifie quel skelette on prend
skeletonJointsMeters=depthMetaData.JointWorldCoordinates(:,:,depthMetaData.IsSkeletonTracked);

%Affichage de l'image en profondeur
imshow(depthMap,[0 9000]);

%Variable du nombre de skelettes
nSkeleton=sum(depthMetaData.IsSkeletonTracked);
  
if nSkeleton>0
    
    timeIsUp =timeIsUp +1;
    DrawPoints(depthMetaData);
    DrawSkeleton( depthMetaData,nSkeleton );
        
    %recuperation des valeurs
        
        tableau_TPS=[ tableau_TPS ; depthMetaData.AbsTime(6) ];
        sz=size(tableau_TPS,1);
        for r=1:1:20
        tableau_X(sz,r)= skeletonJointsMeters(r,1);
        tableau_Y(sz,r)= skeletonJointsMeters(r,2);
        tableau_Z(sz,r)= skeletonJointsMeters(r,3);
        end
end
    timeIsUp =timeIsUp +1;
    
end

stop(depthVid);

%numero des joints voulus entre 1 & 20
% 1 HipCenter
% 2 Spine
% 3 ShoulderCenter
% 4 Head
% 5 Shoulder Left
% 6 Elbow Left
% 7 Wrist Left
% 8 Hand Left
% 9 Shoulder Right
% 10 Elbow Right
% 11 Wrist Right
% 12 Hand Right
% 13 Hip Left
% 14 Knee Left
% 15 Ankle Left
% 16 Foot Left
% 17 Hip Right
% 18 Knee Right
% 19 Ankle Right
% 20 Foot Right

TabName= {'HipCenter','Spine','ShoulderCenter','Head','Shoulder Left','Elbow Left','Wrist Left','Hand Left',...
           'Shoulder Right','Elbow Right','Wrist Right','Hand Right','Hip Left','Knee Left','Ankle Left',...
           'Foot Left','Hip Right','Knee Right','Ankle Right','Foot Right'};

% ON MET LES JOINTS VOULUS
tab=[1 13 14 15 17 18];

%creation légende
TabLegend=[];
for i=1:1:size(tab,2)
    TabLegend{i}=[TabName{tab(i)}];
end
figure(3)
for m = 1:1:6
plot(tableau_TPS,tableau_X(:,tab(m)));
hold on
end
hold off
title('Position X');
xlabel('temps(s)');
ylabel('Position X');
legend(TabLegend);
figure (4)
for m = 1:1:6
plot(tableau_TPS,tableau_Y(:,tab(m)));
hold on
end
hold off
title('Position Y');
xlabel('temps(s)');
ylabel('Position Y');
legend(TabLegend);
figure(5)
for m = 1:1:6
plot(tableau_TPS,tableau_Z(:,tab(m)));
hold on
end
hold off 
title('Position Z');
xlabel('temps(s)');
ylabel('Position Z');
legend(TabLegend);