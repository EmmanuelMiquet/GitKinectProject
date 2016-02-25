%% Initialisation
close all;
clear all;
clc;

% Add our functions to the path
addpath('KinectFunctions');

%% Data Load
% Trainning data
[~,~,nbAngles,trainningAng,legendAngles] = KinectLoadAngles('TrainningData/',0);
[t_trainning,~,nbPoints,Xt,Yt,Zt,legende] = KinectLoadPosition('TrainningData/',0,'relative');
[Xt,Yt,Zt] = KinectNormalizePosition(Xt,Yt,Zt);

% Testing data
[~,~,~,testAng,~] = KinectLoadAngles('ExperienceData/',1);
[t_test,~,~,Xe,Ye,Ze,~] = KinectLoadPosition('ExperienceData/',1,'relative');
[Xe,Ye,Ze] = KinectNormalizePosition(Xe,Ye,Ze);

trainningPos = [Xt Yt Zt];
testPos = [Xe Ye Ze];

trainning = [trainningAng/max(max(trainningAng)) trainningPos/max(max(trainningPos))];
test = [testAng/max(max(testAng)) testPos/max(max(trainningPos))];

%% Class vector trainning
groups = groupsCreation([0 81 154],length(t_trainning));

%% Classification of test data
% Parametres
epsilon = 0.2; % Global tolerance
K = 3; % Number of neighbors to consider

[predict,IDX,distance,seuil] = KinectClassificationKNN(trainning,groups,test,epsilon,K);

%% Plot results
figure;
subplot(3,1,1);
plot([0 max(t_test)],[seuil,seuil]);
legend('Threshold');
hold all;
for i = 1:K
    plot(t_test,distance(:,i));
end
hold off;
grid on;
title('Minimum distance(s) between trainning and test data');
xlabel('Time (s)'); 
ylabel('Distance between Neighbors');

subplot(3,1,2);
hold all;
for i = 1:K
    plot(t_test,t_test(IDX(:,i),1))
end
hold off;
grid on;
title('Time correspondance Trainning/Test');
xlabel('Testing time (s)'); 
ylabel('Trainning time (s)');

subplot(3,1,3);
plot(t_test,predict);
grid on;
title('Classes predicted by the classifier');
xlabel('Time (s)'); 
ylabel('Classes');

KinectSpacePlotClasses(predict,Xe,Ye,Ze);