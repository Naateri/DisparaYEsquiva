# import libs
import cv2
import numpy as np
import time
import socket

##SENDING INFORMATION TO UNITY

UDP_IP = "127.0.0.1"
UDP_PORT = 5065

print ("UDP target IP:", UDP_IP)
print ("UDP target port:", UDP_PORT)

sock = socket.socket(socket.AF_INET, # Internet
                     socket.SOCK_DGRAM) # UDP

###SOCKET PREPARATION FINISHED

# begin streaming

video_font = 'http://192.168.1.136:4747/video'

LIGHT_CONTOUR = 10

cap = cv2.VideoCapture(video_font)

last_x = 0
last_y = 0
    
while True:
    _, frame = cap.read()

    # convert frame to monochrome and blur
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    blur = cv2.GaussianBlur(gray, (9, 9), 0)

    # use function to identify threshold intensities and locations
    (minVal, maxVal, minLoc, maxLoc) = cv2.minMaxLoc(blur)
    # print(minVal, maxVal, minLoc, maxLoc)
    # threshold the blurred frame accordingly
    hi, threshold = cv2.threshold(blur, maxVal-20, 255, cv2.THRESH_BINARY)
    thr = threshold.copy()

    # resize frame for ease
    cv2.resize(thr, (300, 300))
    # find contours in thresholded frame
    edged = cv2.Canny(threshold, 50, 150)
    lightcontours, hierarchy = cv2.findContours(edged, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)

#check if the list of contours is greater than 0 and if any circles are detected
    if len(lightcontours) > 0:
        #Find the Maxmimum Contour, this is assumed to be the light beam
        maxcontour = max(lightcontours, key=cv2.contourArea)
        #avoids random spots of brightness by making sure the contour is reasonably sized
        if cv2.contourArea(maxcontour) > LIGHT_CONTOUR:
            (x, final_y), radius = cv2.minEnclosingCircle(maxcontour)
            cv2.circle(frame, (int(x), int(final_y)), int(radius), (0, 255, 0), 4)
            cv2.rectangle(frame, (int(x) - 5, int(final_y) - 5),
                          (int(x) + 5, int(final_y) + 5), (0, 128, 255), -1)

            pos_x = int(x)
            last_x = pos_x
            pos_y = int(final_y)
            last_y = pos_y

    print("posx", last_x)
    bl= bytes(str(last_x),encoding='utf-8')
    sock.sendto(bl,(UDP_IP,UDP_PORT)) #SENDING TO UNITY
    #print("posy", last_y)
    #display frames and exit
    cv2.imshow('light', thr)
    cv2.imshow('frame', frame)

    cv2.waitKey(4)

cv2.destroyAllWindows()
cap.release()
