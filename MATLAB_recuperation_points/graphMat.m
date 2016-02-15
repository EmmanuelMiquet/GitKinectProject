function graphMat 
%UNTITLED Summary of this function goes here
%   Detailed explanation goes here
clear all
clc
close all


%% Initialisation 
start1=1;
end1=0;
m=3;
rep=0;

%% Fonction Callback
    function mypreview_fcn(obj,event,hImage)
        tstampstr = event.Timestamp;
        ht = getappdata(hImage,'HandleToTimestampLabel');
        ht.String = tstampstr;
        hImage.CData = event.Data;
    end 

    function Tracking(obj,event)
        val=obj.Value;
        map=obj.String;
        if (strcmp('oui',map(val)) && start1 ==1)
            start(vid);
            start1=0;
            end1=1;


        elseif (strcmp('non',map(val)) && end1 ==1)
            stop(vid);
            start1=0;
            end1=1;
        end
    end

    function pixelTracking(obj,event,popup1,popup4)
        val=obj.Value;
        map=obj.String;
        val2=popup1.Value;
        map2=popup1.String; 
        val4=popup4.Value;
        map4=popup4.String;
       state1 =0;
       a=map2(val2);
       b=map(val);
       c=map4(val4);
            %%rajouter fonction squelette while % si les 2 on doit faire un
            %%bouton et pas oublier la mise à jour ...

            while (strcmp('oui',a) && strcmp('oui',b)&& strcmp('non',c) )
                trigger(vid);
                depthMap=[];
                depthMetaData=[];
               [depthMap,~,depthMetaData]=getdata(vid);
               state1=1;
                GraphDrawPixelPoints( depthMetaData,state1 );
                
                
                %mise à jour bouton
                a1=get(obj,'Value');
                a2=get(obj,'String');
                b1=get(popup1,'Value');
                b2=get(popup1,'String');
                c1=get(popup4,'Value');
                c2=get(popup4,'String');
                a=a2(a1);
                b=b2(b1);
                c=c2(c1);
               disp(a);
               disp(b);
               disp(c);
            end
            if (state1 == 1 )
                state1=0;
                GraphDrawPixelPoints( depthMetaData,state1 );
            end
           
            end
     


function depthTracking(obj,event,popup1,popup4)
        val=obj.Value;
        map=obj.String;
        val2=popup1.Value;
        map2=popup1.String; 
        val4=popup4.Value;
        map4=popup4.String;
        state=0;
       
            %%rajouter fonction squelette while % si les 2 on doit faire un
            %%bouton et pas oublier la mise à jour ...

            while (strcmp('oui',map2(val2)) && strcmp('oui',map(val))&& strcmp('non',map4(val4)) )
                trigger(vid);
                depthMap=[];
                depthMetaData=[];
               [depthMap,~,depthMetaData]=getdata(vid); 
               state=1;
                GraphDrawDeapthPoints( depthMetaData,state );  
                %mise à jour bouton
                val=get(obj,'Value');
                map=get(obj,'String');
                val2=get(popup1,'Value');
                map2=get(popup1,'String');
                val4=get(popup4,'Value');
                map4=get(popup4,'String');
                
            end
            
           
            if (state == 1 )
                state=0;
                GraphDrawDeapthPoints( depthMetaData,state );
            end
           
end


function depthPixelTracking(obj,event)
        val=obj.Value;
        map=obj.String;
        state=0;
       
            %%rajouter fonction squelette while % si les 2 on doit faire un
            %%bouton et pas oublier la mise à jour ...

            while (strcmp('oui',map(val)) )
                trigger(vid);
                depthMap=[];
                depthMetaData=[];
               [depthMap,~,depthMetaData]=getdata(vid); 
               state=1;
                GraphDrawPoints( depthMetaData,state );  
                %mise à jour bouton
                val=get(obj,'Value');
                map=get(obj,'String');          
            end
            
            if (state == 1 )
                state=0;
                GraphDrawPoints( depthMetaData,state );
            end
           
end
   
%% Création du flu video
 imaqreset; 

% Create a video input object.

vid=videoinput('kinect',2);
set(getselectedsource(vid),'BodyPosture','standing');
triggerconfig(vid,'manual');
vid.FramesPerTrigger=1;
vid.TriggerRepeat=inf;
set(getselectedsource(vid),'TrackingMode','Skeleton');


% Create a figure window. 
% toolbar and menubar in the figure.
hFig = figure('Toolbar','none',...
       'Menubar', 'none',...
       'NumberTitle','off',...
       'Position',[0 0 1200 600],...
       'Name','Tracking Kinect',...
       'tag','interface');
            
%% Creation of the buttons

% Set up the push buttons
uicontrol('String', 'Start Preview',...
    'Callback', 'preview(vid)',...
    'Units','normalized',...
    'Position',[0.04 0.03 0.10 .05]);
uicontrol('String', 'Stop Preview',...
    'Callback', 'stoppreview(vid)',...
    'Units','normalized',...
    'Position',[0.17 0.03 0.10 .05]);
uicontrol('String', 'Close',...
    'Callback', 'close(gcf)',...
    'Units','normalized',...
    'Position',[0.3 0.03 .10 .05]);

   popup1=uicontrol('Style','popup',...
     'String',{'non','oui'},...
     'Position', [850 555 120 20],...
     'Callback', @Tracking);
 
 text1=uicontrol('Style','text',...
     'Position',[720 547 120 20],...
     'FontWeight','bold',...
     'String','Skeleton Tracking');
 
 popup4=uicontrol('Style','popup',...
     'String',{'non','oui'},...
     'Position', [850 415 120 20],...
     'Callback', @depthPixelTracking);
 
 text4=uicontrol('Style','text',...
     'Position',[720 410 120 28],...
     'FontWeight','bold',...
     'String','Depth & Pixel Tracking');
     
 
 popup2=uicontrol('Style','popup',...
     'String',{'non','oui'},...
     'Position', [850 505 120 20],...
     'Callback',{ @pixelTracking,popup1,popup4});
 
 text2=uicontrol('Style','text',...
     'Position',[720 500 120 20],...
     'FontWeight','bold',...
     'String','Pixel Tracking');
 
 popup3=uicontrol('Style','popup',...
     'String',{'non','oui'},...
     'Position', [850 460 120 20],...
     'Callback', {@depthTracking,popup1,popup4});
 
 text3=uicontrol('Style','text',...
     'Position',[720 460 120 20],...
     'FontWeight','bold',...
     'String','Depth Tracking');
 

% Create the text label for the timestamp
hTextLabel = uicontrol('style','text','String','Timestamp', ...
    'Units','normalized',...
    'Position',[0.85 -.04 .15 .08]);

% Create the image object in which you want to display the video preview data.
vidRes = vid.VideoResolution;
imWidth = vidRes(1);
imHeight = vidRes(2);
nBands = vid.NumberOfBands;



% Specify the size of the axes that contains the image object
figSize = get(hFig,'Position');
 figWidth = figSize(3);
 figHeight = figSize(4);

 %ax resize the image
 ax = axes( 'Unit','pixels','Position', [30  70  640 480]);
hImage = image( zeros(imHeight, imWidth, nBands),'Parent',ax );

gca.unit = 'pixels';
gca.position = [ ((figWidth - imWidth)/2)... 
               ((figHeight - imHeight)/2)...
               imWidth imHeight ];
           


%Set up the update preview window function.
setappdata(hImage,'UpdatePreviewWindowFcn',@mypreview_fcn);

%Make handle to text label available to update function.
setappdata(hImage,'HandleToTimestampLabel',hTextLabel);



 preview(vid, hImage);




    

end

