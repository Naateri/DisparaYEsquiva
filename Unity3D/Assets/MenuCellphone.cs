using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static MenuToGame;

// y goes from -6 to 6
// x goes from -10 to 10

public class MenuCellphone : MonoBehaviour {

	// Use this for initialization

	public GameObject pointer;
	private float xPos = 0.0f;
	//private float yPos = -3.0f;
	private float yPos = 0.0f;
	private bool update_x = true;
    private float waitTime = 0.0002f;
    private float timer = 0.0f;
    private int button = 0;
    private int prev_button = 0;
 

    // just before timer to choose button
    // is over (0.1s?): switch_scene = true

    private bool switch_scene = false;

	Thread receiveThread;
	UdpClient client;
	public int port;

	//info

	public string lastReceivedUDPPacket = "";
	public string allReceivedUDPPackets = "";

	void Start () {
		if (MenuToGame.Menu_Cellphone == 1){
			init();
		} else {
			print("Do nothing");
		}

    }

	void OnGUI(){
		Rect  rectObj=new Rect (100,10,200,400);
		
		GUIStyle  style  = new GUIStyle ();
		
		style .alignment  = TextAnchor.UpperLeft;
		
		GUI .Box (rectObj,"# UDPReceive Circle\n127.0.0.1 "+port +" #\n"
		          
		          //+ "shell> nc -u 127.0.0.1 : "+port +" \n"
		          
		          + "\nLast Packet: \n"+ lastReceivedUDPPacket
		          
		          //+ "\n\nAll Messages: \n"+allReceivedUDPPackets
		          
		          ,style );

	}

	private IEnumerator Wait(float time){
		yield return new WaitForSeconds(time);
		receiveThread = new Thread (new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start ();
	}

	private void init(){
		print ("UPDSend.init()");

		port = 5080;

		MenuToGame.Menu_Cellphone = 0;

		//client = new UdpClient (port);

		print ("Sending to 127.0.0.1 : " + port);

		StartCoroutine(Wait(0.8f));

		//receiveThread = new Thread (new ThreadStart(ReceiveData));
		//receiveThread.IsBackground = true;
		//receiveThread.Start ();

	}

	private void ReceiveData(){
		print("Recieve thread MENU_POS");
		client = new UdpClient (port);
		while (true) {
		//while(MenuToGame.Alive == 1){
			try{
				//client = new UdpClient (port);
				if (switch_scene){
					client.Close();
					break;
				}
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

				byte[] data = client.Receive(ref anyIP);

				string text = Encoding.UTF8.GetString(data);
				//print (">> " + text);

				String[] separation = { " " }; 
				Int32 count = 2; 

				String[] strlist = text.Split(separation, count, 
              	StringSplitOptions.RemoveEmptyEntries); 

				lastReceivedUDPPacket=text;
				allReceivedUDPPackets=allReceivedUDPPackets+text;
				xPos = float.Parse(strlist[0]);

				float x_factor = 10.0f/300.0f;
				float y_factor = 6.0f/225.0f;
				//xPos *= 0.021818f;
				xPos *= x_factor;
				
				yPos = float.Parse(strlist[1]);
				//yPos *= 0.005f;
				yPos *= y_factor;
				//client.Close();
				//print("MENU CELLPHONE " + xPos + " " + yPos);
			}catch(Exception e){
				print (e.ToString());
			}
		}
		//client.Close();
	}

	public string getLatestUDPPacket(){
		allReceivedUDPPackets = "";
		return lastReceivedUDPPacket;
	}

    // Update is called once per frame
    void Update()
    {

        pointer.transform.position = new Vector3(xPos, yPos, 0);
     
        for (int i = 0; i < 4; ++i)
        {
            int posy = 70 - i * 60;
            Rect rect = new Rect(0,posy , 180, 40);
           // print(pointer.transform.position);

            Vector3 startPos = Vector3.zero;
            startPos = (new Vector2(pointer.transform.position.x/10.0f*300.0f, pointer.transform.position.y /6.0f * 300.0f));
            //print(startPos);
            if (rect.Contains(startPos))
            {
                button = i;
                timer = Time.realtimeSinceStartup;
                //print("BOTON");
                //print(i);
               // print("TIMER");

                //print(timer);
             
                //print(i);
                if (prev_button == button)
                {

                    print("ELAPSED");
                    float elapsed = Time.realtimeSinceStartup - timer;
                    print(elapsed);
                    if (elapsed >= waitTime)
                    {
                        print("Entrar al boton");
                        print(button);

                        prev_button = 0;
                        timer = 0.0f;
                    }

                }
                else
                {
                    prev_button = button;
                    timer = 0.0f;
                    
                }
            }

        }
    
    }

	void OnApplicationQuit(){
		if (receiveThread != null) {
			//if (client != null)
			//	client.Close();
			receiveThread.Abort();
			Debug.Log(receiveThread.IsAlive); //must be false
		}
	}
}
