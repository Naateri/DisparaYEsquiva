using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static MenuToGame;

public class SocketClientMP : MonoBehaviour {

	// Use this for initialization

	public GameObject hero;
	private float xPos = 10.0f;
	//private float yPos = -3.0f;
	private float yPos = 2.0f;
	private bool update_x = true;

	Thread receiveThread;
	UdpClient client;
	Socket send_score;
	public int port;

	// differentiate players

	public int player;

	//info

	public string lastReceivedUDPPacket = "";
	public string allReceivedUDPPackets = "";

	void Start () {

		if (player == 2)
        {
			xPos = 12.0f;
        }

		Physics.IgnoreLayerCollision(8, 10);

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

		//send_score = new UdpClient("127.0.0.1",5090);
		send_score = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
			ProtocolType.Udp);

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

				if (player == 2) // player2: move on y axis only
                {
					xPos = float.Parse(strlist[0]);
					yPos = float.Parse(strlist[1]);

					yPos -= 175.0f;

					//xPos *= 0.005f;
					xPos = 10.0f;
					yPos /= 20;
                }

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
		// send score to python program
		// update cellphone's frame
		IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5090);
		if (MenuToGame.Alive == 0){
			Byte[] sendBytes = Encoding.ASCII.GetBytes("Dead");
			try{
			    send_score.SendTo(sendBytes, anyIP);
			    send_score.Close();
			}
			catch ( Exception e ){
			    Console.WriteLine( e.ToString());
			}
		} else {
			Byte[] sendBytes = Encoding.ASCII.GetBytes(MenuToGame.Score.ToString());
			try{
			    send_score.SendTo(sendBytes, anyIP);
			}
			catch ( Exception e ){
			    Console.WriteLine( e.ToString());
			}
		}

		//try{
			//hero = GameObject.FindWithTag("PlayerObj");
			//hero.transform.position = new Vector3(-xPos+6.0f,-1.5f-yPos,0);
		if (player == 1)
			hero.transform.position = new Vector3(xPos,-1.5f-yPos,0);
		if (player == 2)
        {
			print(yPos);
			hero.transform.position = new Vector3(xPos, yPos, 0);
		}
		//} catch(Exception e){
		//}
		/*if (MenuToGame.Alive == 0){
			//receiveThread.Abort();
			//if (client != null)
			//	client.Close();
			//client.Close();
		}*/
	}

	void OnApplicationQuit(){
		if (receiveThread != null) {
			//if (client != null)
			//	client.Close();
			receiveThread.Abort();
			Debug.Log(receiveThread.IsAlive); //must be false
			send_score.Close();
		}
	}
}
