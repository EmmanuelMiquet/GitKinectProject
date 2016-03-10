%% Initialisation
close all;
clear all;
clc;

% Add our functions to the path
addpath('KinectFunctions');

% Change the path to the data
trainingDataPath = 'TrainingDataBase/';
experienceDataPath = 'ExperienceData/';

%% Data Load
% training data
trainingPos = []; trainingAng = []; l = 0; %variable initialization
classDir = dir([trainingDataPath, '/Pos*/']); %search all Pos* directories
for i = 1:length(classDir([classDir.isdir]))
    tmp = {classDir.name};
    dataDir = dir([trainingDataPath tmp{i},'/*relativePointsCapture.txt']); %search all capture files in Pos* directory
    for j = 0:length(dataDir(not([dataDir.isdir])))-1
        [~,~,nbAngles,trainingAngTMP,legendAngles] = KinectLoadAngles([trainingDataPath tmp{i} '/'],j);
        [t_training,~,nbPoints,Xt,Yt,Zt,legende] = KinectLoadPosition([trainingDataPath tmp{i} '/'],j,'relative');
        [Xt,Yt,Zt] = KinectNormalizePosition(Xt,Yt,Zt);

        trainingPosTMP = [Xt Yt Zt];
        trainingPos = [trainingPos ; trainingPosTMP];
        trainingAng = [trainingAng ; trainingAngTMP];
    end
    l = [l length(trainingAng(:,1))];
end

for i = 1:length(trainingPos(:,1))
   t_training(i,1) = i/30; 
end

% Testing data
[~,~,~,testAng,~] = KinectLoadAngles(experienceDataPath,0);
[t_test,~,~,Xe,Ye,Ze,~] = KinectLoadPosition(experienceDataPath,0,'relative');
[Xe,Ye,Ze] = KinectNormalizePosition(Xe,Ye,Ze);

testPos = [Xe Ye Ze];

training = [trainingAng/max(max(trainingAng)) trainingPos/max(max(trainingPos))];
test = [testAng/max(max(testAng)) testPos/max(max(trainingPos))];

%% Class vector training
groups = groupsCreation(l,length(trainingPos(:,1)));

%% Classification of test data
% Parametres
epsilon = 0.25; % Global tolerance
K = 3; % Number of neighbors to consider

[predict,IDX,distance,seuil] = KinectClassificationKNN(training,groups,test,epsilon,K);

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
title('Minimum distance(s) between training and test data');
xlabel('Time (s)'); 
ylabel('Distance between Neighbors');

subplot(3,1,2);
hold all;
for i = 1:K
    plot(t_test,t_training(IDX(:,i),1))
end
hold off;
grid on;
title('Time correspondance training/Test');
xlabel('Testing time (s)'); 
ylabel('training time (s)');

subplot(3,1,3);
plot(t_test,predict);
grid on;
title('Classes predicted by the classifier');
xlabel('Time (s)'); 
ylabel('Classes');

KinectSpacePlotClasses(predict,Xe,Ye,Ze);