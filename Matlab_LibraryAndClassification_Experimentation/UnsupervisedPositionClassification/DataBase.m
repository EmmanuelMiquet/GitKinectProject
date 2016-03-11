close all
clear all
clc


%% Lecture des points
load('pointsCapture.txt');

%% Extraction des points "Bon"
% for i =1:1:5
% X(:,i)=Bon(:,i);
% Y(:,i)=Bon(:,i+1);
% Z(:,i)=Bon(:,i+2);
% 
% XY(:,1+2*(i-1))=X(:,i);
% XY(:,2+2*(i-1))=Y(:,i);
% end
% 
%  XY=XY(:,1:2);

 %% Extraction des points "pointsCapteurs"
 
for i =1:1:5
X(:,i)=pointsCapture(:,3*(i-1)+2);
Y(:,i)=pointsCapture(:,3*(i-1)+3);
Z(:,i)=pointsCapture(:,3*(i-1)+4);

XY(:,2*i+1)=X(:,i);
XY(:,2*i+2)=Y(:,i);
end

%% Kmean: on trouve les positions

idx=[];
C=[];
sumd=[];

for i=1:1:5
    [idx(:,i),C(:,1+2*(i-1):2*i),sumd(:,i)]=kmeans(XY(:,2*i+1:2*(i+1)),5);
end

%% Detection Position

Load('test.txt');
normTest=0;
minNorme=1;

for i=1:1:5
    normTest=sqrt((C(5))^2+()^2)
end


%% FIgures
figure(1)
for i=1:1:5
subplot(3,2,i)
plot(C(:,1+2*(i-1)),C(:,2*i),'r*','MarkerSize',5,'LineWidth',5);
hold on
plot(X(:,i),Y(:,i),'k*','MarkerSize',5);
hold off
end

%% Kmean 1 seul barycentre


% [idx1,C1,sumd,D]=kmeans(XY(:,1:2),1);
% [idx2,C2,sumd2,D2]=kmeans(XY(:,3:4),1);
% [idx3,C3,sumd3,D3]=kmeans(XY(:,5:6),1);
% [idx4,C4,sumd4,D4]=kmeans(XY(:,7:8),1);
% [idx5,C5,sumd5,D5]=kmeans(XY(:,9:10),1);
% 
% for i=1:1:size(C,1)
%     plot(C(i,1),C(i,2),'r+','MarkerSize',15);
%     hold on
% end
% 
% figure(1)
% for i=1:1:size(X,1)
%     if idx(i)==1
%         
%         plot(X(i,1),Y(i,1),'r*','MarkerSize',5);
%         hold on;
%     elseif idx(i)==2 
%          plot(X(i,1),Y(i,1),'b*','MarkerSize',5);
%          hold on;
%     end
% end

% %% FIgures
% figure(1)
% for i=1:1:5
% subplot(3,2,i)
% plot(XY(idx==1,1),XY(idx==1,2));
% plot(X(:,i),Y(:,i),'k*','MarkerSize',5);
% end
