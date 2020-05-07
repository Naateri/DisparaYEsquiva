import cv2
import numpy as np
import math
import socket
import time

#################################################

def send_socket(info, sock, UDP_IP, UDP_PORT):
    info_b = bytes(str(info), encoding='utf-8')
    sock.sendto(info_b, (UDP_IP, UDP_PORT)) #SENDING TO UNITY

################SCREEN COLOR##################

class ScreenColor():

    total_x = 600 #maximum value of x, pos_x = total_x - min_x
    min_x = 200 #minimum value of x, pos_x = 0

    x_difference = total_x - min_x 
    substract_x = x_difference / 2

    #if max_x = 400, we want min_x = -200 and max_x = 200

    azulBajo = np.array([100,100,20],np.uint8)
    azulAlto = np.array([125,255,255],np.uint8)

    amarilloBajo = np.array([15,100,20],np.uint8)
    amarilloAlto = np.array([45,255,255],np.uint8)

    redBajo1 = np.array([0,100,20],np.uint8)
    redAlto1 = np.array([5,255,255],np.uint8)

    redBajo2 = np.array([175,100,20],np.uint8)
    redAlto2 = np.array([179,255,255],np.uint8)

    moradoBajo = np.array([128,0,128],np.uint8)
    moradoAlto = np.array([148,50,211],np.uint8)

    verdeBajo = np.array([100,20,100], np.uint8)
    verdeAlto = np.array([255,255,125], np.uint8)

    UDP_PORT = 5065
    UDP_IP = "127.0.0.1"

    font = cv2.FONT_HERSHEY_SIMPLEX

    def __init__(self, cap, color, sock): #cap = VideoCapture

        # color can be:
        # "blue"
        # "red"
        # "yellow"
        
        self.cap = cap
        self.color = color

        self.pos_x = 0
        self.pos_y = 0

        print(self.substract_x)

        self.sock = sock

    def dibujar(self, mask,color,frame):
            contornos,_ = cv2.findContours(mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
            if len(contornos) > 0:
                max_val = np.argmax([cv2.contourArea(c) for c in contornos])
            for c in contornos:
                    area = cv2.contourArea(c)
                    if area > 3000:
                        M = cv2.moments(c)
                        if (M["m00"]==0): M["m00"]=1
                        x = int(M["m10"]/M["m00"])
                        y = int(M["m01"]/M["m00"])
                        nuevoContorno = cv2.convexHull(c)
                        cv2.circle(frame,(x,y),7,(0,255,0),-1)
                        cv2.putText(frame,'{},{}'.format(x,y),(x+10,y),self.font, 0.75,(0,255,0),1,cv2.LINE_AA)
                        cv2.drawContours(frame, [nuevoContorno], 0, color, 3)
                            
                        self.pos_x = x - self.substract_x #eg: x = x - 200, goes from 0 to 400
                        self.pos_x -= self.substract_x #eg: x = x - 200, goes from -200 to 200
                        self.pos_y = y

    def loop(self, frame):
            frameHSV = cv2.cvtColor(frame,cv2.COLOR_BGR2HSV)
            if (self.color == "blue"):
                mask = cv2.inRange(frameHSV,self.azulBajo,self.azulAlto)
                draw_color = (255,0,0)
            elif self.color == "red":
                mask = cv2.inRange(frameHSV,self.redBajo2,self.redAlto2)
                draw_color = (0,0,255)
            elif self.color == "yellow":
                mask = cv2.inRange(frameHSV,self.amarilloBajo,self.amarilloAlto)
                draw_color = (0,255,255)
            else:
                return

            self.dibujar(mask, draw_color,frame)
            pos_info = str(self.pos_x) + " " + str(self.pos_y)
        
            info= bytes(pos_info,encoding='utf-8')
            self.sock.sendto(info,(self.UDP_IP,self.UDP_PORT)) #SENDING TO UNITY
                    
            cv2.imshow('frame',frame)
    

def main():

    UDP_IP = "127.0.0.1"
    ####HAND
    UDP_HAND_PORT = 5070

    SHOTS_PER_SECOND = 0.5
    SHOOT_WAIT_TIME = False


    print ("UDP target IP:", UDP_IP)
    print ("UDP hand target port:", UDP_HAND_PORT)

    sock = socket.socket(socket.AF_INET, # Internet
                     socket.SOCK_DGRAM) # UDP

    video_font = 'http://192.168.1.136:4747/video'
    cap = cv2.VideoCapture(video_font)

    screenColor = ScreenColor(cap, "yellow", sock)

    #cap = cv2.VideoCapture(0)
    while(cap.isOpened()):
        ret, img = cap.read()
        cv2.rectangle(img,(200,150),(50,300),(0,255,0),0)
        crop_img = img[150:300, 50:200]
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

        #print("Info", info)
        send_socket(info, sock, UDP_IP, UDP_HAND_PORT)
        #cv2.imshow('drawing', drawing)
        #cv2.imshow('end', crop_img)
        #cv2.imshow('Gesture', img)
        all_img = np.hstack((drawing, crop_img))
        cv2.imshow('Contours', all_img)
        k = cv2.waitKey(10)
        if k == 27:
            break

        screenColor.loop(img)

main()
