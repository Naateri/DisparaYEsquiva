import cv2
import numpy as np
import math
import socket
import time

UDP_IP = "127.0.0.1"
UDP_PORT = 5070

print ("UDP target IP:", UDP_IP)
print ("UDP target port:", UDP_PORT)

sock = socket.socket(socket.AF_INET, # Internet
                     socket.SOCK_DGRAM) # UDP

SHOTS_PER_SECOND = 0.5
SHOOT_WAIT_TIME = False


#################################################

def send_socket(info):
    info_b = bytes(str(info), encoding='utf-8')
    sock.sendto(info_b, (UDP_IP, UDP_PORT)) #SENDING TO UNITY
    

video_font = 'http://192.168.1.136:4747/video'
cap = cv2.VideoCapture(video_font)

#cap = cv2.VideoCapture(0)
while(cap.isOpened()):
    ret, img = cap.read()
    cv2.rectangle(img,(300,300),(100,100),(0,255,0),0)
    crop_img = img[100:300, 100:300]
    grey = cv2.cvtColor(crop_img, cv2.COLOR_BGR2GRAY)
    value = (35, 35)
    blurred = cv2.GaussianBlur(grey, value, 0)
    _, thresh1 = cv2.threshold(blurred, 127, 255,
                               cv2.THRESH_BINARY_INV+cv2.THRESH_OTSU)
    cv2.imshow('Thresholded', thresh1)
    contours, hierarchy = cv2.findContours(thresh1.copy(),cv2.RETR_TREE, \
            cv2.CHAIN_APPROX_NONE)
    max_area = -1
    for i in range(len(contours)):
        cnt=contours[i]
        area = cv2.contourArea(cnt)
        if(area>max_area):
            max_area=area
            ci=i
    cnt=contours[ci]
    x,y,w,h = cv2.boundingRect(cnt)
    cv2.rectangle(crop_img,(x,y),(x+w,y+h),(0,0,255),0)
    hull = cv2.convexHull(cnt)
    drawing = np.zeros(crop_img.shape,np.uint8)
    cv2.drawContours(drawing,[cnt],0,(0,255,0),0)
    cv2.drawContours(drawing,[hull],0,(0,0,255),0)
    hull = cv2.convexHull(cnt,returnPoints = False)
    defects = cv2.convexityDefects(cnt,hull)
    count_defects = 0
    cv2.drawContours(thresh1, contours, -1, (0,255,0), 3)

    try:
        ans = len(defects.shape) >= 0
    except:
        ans = 0

    if not ans:
        continue
    
    for i in range(defects.shape[0]):
        s,e,f,d = defects[i,0]
        start = tuple(cnt[s][0])
        end = tuple(cnt[e][0])
        far = tuple(cnt[f][0])
        a = math.sqrt((end[0] - start[0])**2 + (end[1] - start[1])**2)
        b = math.sqrt((far[0] - start[0])**2 + (far[1] - start[1])**2)
        c = math.sqrt((end[0] - far[0])**2 + (end[1] - far[1])**2)
        angle = math.acos((b**2 + c**2 - a**2)/(2*b*c)) * 57
        if angle <= 90:
            count_defects += 1
            cv2.circle(crop_img,far,1,[0,0,255],-1)
        #dist = cv2.pointPolygonTest(cnt,far,True)
        cv2.line(crop_img,start,end,[0,255,0],2)
        #cv2.circle(crop_img,far,5,[0,0,255],-1)

    if count_defects >= 4:
        cv2.putText(img,"DISPARAAAA", (50,50), cv2.FONT_HERSHEY_SIMPLEX, 2, 2)

        if not SHOOT_WAIT_TIME: #shoot wait not activated
            start_time = time.perf_counter()
            SHOOT_WAIT_TIME = True
            info = "100"
        else: #shoot wait activated
            end_time = time.perf_counter()
            if (end_time - start_time) >= SHOTS_PER_SECOND: #can shoot again
                SHOOT_WAIT_TIME = False
            else:
                info = "0"

    else:
        cv2.putText(img,"NO HACER NADA", (50,50), cv2.FONT_HERSHEY_SIMPLEX, 2, 2)
        info = "0"

    print("Info", info)
    send_socket(info)
    #cv2.imshow('drawing', drawing)
    #cv2.imshow('end', crop_img)
    cv2.imshow('Gesture', img)
    all_img = np.hstack((drawing, crop_img))
    cv2.imshow('Contours', all_img)
    k = cv2.waitKey(10)
    if k == 27:
        break
