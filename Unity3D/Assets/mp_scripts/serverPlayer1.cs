using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static MenuToGame;
using static globalGameInfo;

public class serverPlayer1: MonoBehaviour
{
    
	private int lives = 10;
	private int score = 0;
	AudioSource cube_audio;
	private ParticleSystem particle;
    public Font myFont;
	public GameObject player1, player2;

    Thread receiveThread, sendThread;
    UdpClient client;
    Socket server_to_client;
    public int port1, port2;

    float pos_x, pos_y; // stores positions recieved from player 2

    String client_ip;// = "192.168.1.61";

    // Bullets

    public GameObject bullet, b_clone;
    public AudioSource gunshot;
    private Vector3 speed = new Vector3(0.0f, 7.5f, 0.0f);

    private int shot = 0; // 0->dont shoot, 1->shoot

    // ...

    private void update_position(float x, float y){
        player2.transform.position = new Vector3(x,y,0);
    }

	void Start(){
		print("Server mp");

		//MenuToGame.Alive = 1;
		//MenuToGame.Power_status = 1;
		//MenuToGame.Menu_Cellphone = 1; // can create menu socket again
        port1 = 5200; // server -> client
        port2 = 5300; // client -> server

        //client_ip = "192.168.1.61";
        client_ip = "26.65.120.130";

        server_to_client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
            ProtocolType.Udp);

        StartCoroutine(Wait(0.8f));
	}

    private IEnumerator Wait(float time){
        yield return new WaitForSeconds(time);
        receiveThread = new Thread (new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start ();
    }
    
    String spawn_enemy1()
    {
        if (globalGameInfo.Sp_e1 == 1) // has spawned
        {
            String position, str_posx, str_posy;
            float enemy1_pos_x, enemy1_pos_y;
            enemy1_pos_x = globalGameInfo.Sp_e1_x;
            enemy1_pos_y = globalGameInfo.Sp_e1_y;

            str_posx = enemy1_pos_x.ToString();
            str_posy = enemy1_pos_y.ToString();

            position = "2000 0 " + str_posx + " " + str_posy;

            globalGameInfo.Sp_e1 = 0;
            return position;
        }
        return "None"; // if no spawn
    }

    void Spawn_shot() // player2 shot
    {
        print("Player 2 shot");
        b_clone = Instantiate(bullet, new Vector3(player2.transform.position.x,
            player2.transform.position.y + 0.2f, 0), Quaternion.identity);
        b_clone.GetComponent<Rigidbody>().velocity = speed;
        gunshot = b_clone.GetComponent<AudioSource>();
        gunshot.Play();
    }

    String send_shot() // sending shot notification to player 2
    {
        String position, str_posx, str_posy;
        float player_pos_x, player_pos_y;
        player_pos_x = player1.transform.position.x;
        player_pos_y = player1.transform.position.y;

        str_posx = player_pos_x.ToString();
        str_posy = player_pos_y.ToString();

        position = "1500 " + str_posx + " " + str_posy;

        globalGameInfo.Sp_b = 0;
        return position;
    }

    void Update(){

        IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse(client_ip), port1);
        
        // Send player 1's position

        String position, str_posx, str_posy;
        float player_pos_x, player_pos_y;
        player_pos_x = player1.transform.position.x;
        player_pos_y = player1.transform.position.y;

        str_posx = player_pos_x.ToString();
        str_posy = player_pos_y.ToString();

        position = "1000 " + str_posx + " " + str_posy;

        String enemy1pos = spawn_enemy1();

        Byte[] enemy1pos_bytes = Encoding.ASCII.GetBytes(enemy1pos);

        Byte[] sendBytes = Encoding.ASCII.GetBytes(position);

        try{
            server_to_client.SendTo(sendBytes, anyIP); // sends position
            if (enemy1pos != "None")
            {
                server_to_client.SendTo(enemy1pos_bytes, anyIP); // sends spawn position only when it happens
            }

            if (globalGameInfo.Sp_b == 1) // player has shot, send bullet info
            {
                String bullet_notif = send_shot();
                //print("Sending " + bullet_notif);
                Byte[] send_bullet = Encoding.ASCII.GetBytes(bullet_notif);
                server_to_client.SendTo(send_bullet, anyIP);
            }

            //server_to_client.Close();
        }
        catch ( Exception e ){
            Console.WriteLine( e.ToString());
        }

        update_position(pos_x, pos_y); // update player 2's position
        if (shot == 1)
        {
            Spawn_shot(); // Shoot bullet
            shot = 0;
        }
		//this.score = MenuToGame.Score;
	}

    private void ReceiveData(){
        //recieve from client at port2
        print("Server recieve thread");
        client = new UdpClient (port2);
        while (true) {
        //while(MenuToGame.Alive == 1){
            try{
                //print("Recieving new msg");
                //client = new UdpClient (port);
                if (MenuToGame.Alive == 0){
                    print("Closing channel");
                    client.Close();
                    break;
                }
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse(client_ip), port2);

                byte[] data = client.Receive(ref anyIP);

                string text = Encoding.UTF8.GetString(data);
                //print (">> " + text);

                String[] separation = { " " }; 
                Int32 count = 2; 

                //String[] strlist = text.Split(separation, count, 
                //StringSplitOptions.RemoveEmptyEntries); 

                String[] strlist = text.Split(' ');

                if (strlist[0] == "1000"){ // player2 position update
                    //print("Position " + pos_x);
                    
                    pos_x = float.Parse(strlist[1]);
                    pos_y = float.Parse(strlist[2]);
                    //update_position(pos_x, pos_y);
                } else if (strlist[0] == "1500") // player2 shot
                {
                    shot = 1;
                }
                //client.Close();
            }catch(Exception e){
                print (e.ToString());
            }
        }
        //client.Close();
    }

    void OnApplicationQuit(){
        if (receiveThread != null) {
            //if (client != null)
            //  client.Close();
            receiveThread.Abort();
            Debug.Log(receiveThread.IsAlive); //must be false
        }
    }

    /*void OnGUI(){
    	// print other players stats

		Rect rectObj= new Rect (45,70,200,400);
		
		GUIStyle style = new GUIStyle ();
		
		style.alignment = TextAnchor.UpperLeft;
        style.font = myFont;
        style.fontSize = 35;
        style.normal.textColor = Color.yellow;
		if (this.lives > 0){
			GUI.Box (rectObj,"VIDAS: " + this.lives, 
		          style );
		} else {
			GUI.Box (rectObj,"PERDISTE", 
		          style );
		}
      
        Rect powerNotif = new Rect(800, 70, 200, 350);
        style.fontSize = 30;
        style.normal.textColor = Color.green;
        if (MenuToGame.Power_status == 0){
			GUI.Box (powerNotif, "PODER NO DISPONIBLE", style);
		} else if (MenuToGame.Power_status == 1){
			GUI.Box (powerNotif, "PODER DISPONIBLE", style);
		} else if (MenuToGame.Power_status == 2){
			GUI.Box (powerNotif, "PODER ACTIVADO", style);
		}

		Rect rectObj2 = new Rect(800, 300, 200, 350);
		GUIStyle style2 = new GUIStyle();

		style.alignment = TextAnchor.UpperLeft;

		//GUI.Box(rectObj2, "SCORE: " + this.score, style2);

	}*/

}
