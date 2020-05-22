using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static MenuToGame;

public class SocketClient : MonoBehaviour {

	// Use this for initialization

	public GameObject hero;
	private float xPos = 10.0f;
	//private float yPos = -3.0f;
	private float yPos = 2.0f;
	private bool update_x = true;

	Thread receiveThread;
	UdpClient client;
	public int port;

	//info

	public string lastReceivedUDPPacket = "";
	public string allReceivedUDPPackets = "";

	void Start () {
		init();

		MenuToGame.Score = 0;
        GameObject[] gobjects = GameObject.FindGameObjectsWithTag("Music");

        for (var i = 0; i < gobjects.Length; i++)
            Destroy(gobjects[i]);

        GameObject MenuPointer, RealPointer;
        MenuPointer = GameObject.FindWithTag("CellphonePointer");
        RealPointer = GameObject.FindWithTag("RealPointer");
        Destroy(RealPointer);
        Destroy(MenuPointer);
    }

	void OnGUI(){
		/*Rect  rectObj=new Rect (40,10,200,400);
		
		GUIStyle  style  = new GUIStyle ();
		
	
		
		GUI .Box (rectObj,"# UDPReceive\n127.0.0.1 "+port +" #\n"
		          
		          //+ "shell> nc -u 127.0.0.1 : "+port +" \n"
		          
		          + "\nLast Packet: \n"+ lastReceivedUDPPacket
		          
		          //+ "\n\nAll Messages: \n"+allReceivedUDPPackets
		          
		          ,style );
*/
	}

	private IEnumerator Wait(float time){
		yield return new WaitForSeconds(time);
		receiveThread = new Thread (new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start ();
	}

	private void init(){
		print ("UPDSend.init()");

		port = 5065;

		//client = new UdpClient (port);

		print ("Sending to 127.0.0.1 : " + port);

		StartCoroutine(Wait(0.8f));

		//receiveThread = new Thread (new ThreadStart(ReceiveData));
		//receiveThread.IsBackground = true;
		//receiveThread.Start ();

	}

	private void ReceiveData(){
		print("Recieve thread MOVE");
		client = new UdpClient (port);
		while (true) {
		//while(MenuToGame.Alive == 1){
			try{
				//client = new UdpClient (port);
				if (MenuToGame.Alive == 0){
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
				//xPos *= 0.021818f;
				xPos /= 20;
				
				yPos = float.Parse(strlist[1]);
				yPos *= 0.005f;
				//client.Close();
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
	void Update () {
		//try{
			//hero = GameObject.FindWithTag("PlayerObj");
			//hero.transform.position = new Vector3(-xPos+6.0f,-1.5f-yPos,0);
			hero.transform.position = new Vector3(xPos,-1.5f-yPos,0);
		//} catch(Exception e){
			;
		//}
		/*if (MenuToGame.Alive == 0){
			//receiveThread.Abort();
			//if (client != null)
			//	client.Close();
			//client.Close();
		}*/
	}

	/*void OnApplicationQuit(){
		if (receiveThread != null) {
			//if (client != null)
			//	client.Close();
			receiveThread.Abort();
			Debug.Log(receiveThread.IsAlive); //must be false
		}
	}*/
}
