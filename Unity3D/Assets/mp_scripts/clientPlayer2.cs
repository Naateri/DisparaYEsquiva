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

public class clientPlayer2 : MonoBehaviour
{
    private int lives = 10;
	private int score = 0;
	AudioSource cube_audio;
	private ParticleSystem particle;
    public Font myFont;
	public GameObject player1, player2;

    /// <summary>
    /// Enemies
    /// </summary>
    /// 

    // Bullets

    public GameObject bullet, b_clone;
    public AudioSource gunshot;
    private Vector3 speed = new Vector3(0.0f, 7.5f, 0.0f);

    private int shot = 0; // 0->dont shoot, 1->shoot

    // Enemy 1

    private float MIN_DEPTH = -5.5f;
    public GameObject cube, clone;

    // 0 -> no spawn
    // 1 -> spawn
    private int enemy1_spawn = 0; 

    // Sockets

    Thread receiveThread, sendThread;
    UdpClient server; // connection to server
    Socket client_to_server;
    public int port1, port2;

    //String server_ip = "192.168.1.133";
    String server_ip = "26.65.123.2";

    float pos_x, pos_y; // stores positions recieved from player 1

    private void update_position(float x, float y){
        player1.transform.position = new Vector3(x,y,0);
    }

    void SpawnEnemy1(float x, float y)
    {
        clone = Instantiate(cube, new Vector3(x, y, 0),
             Quaternion.identity);
    }

    void Spawn_shot() // player1 shot
    {
        b_clone = Instantiate(bullet, new Vector3(player1.transform.position.x,
            player1.transform.position.y + 0.2f, 0), Quaternion.identity);
        b_clone.GetComponent<Rigidbody>().velocity = speed;
        gunshot = b_clone.GetComponent<AudioSource>();
        gunshot.Play();
    }

    String send_shot() // sending shot notification to player 2
    {
        String position, str_posx, str_posy;
        float player_pos_x, player_pos_y;
        player_pos_x = player2.transform.position.x;
        player_pos_y = player2.transform.position.y;

        str_posx = player_pos_x.ToString();
        str_posy = player_pos_y.ToString();

        position = "1500 " + str_posx + " " + str_posy;

        globalGameInfo.Sp_b = 0;
        return position;
    }


    void Start(){
		print("Server mp");

		//MenuToGame.Alive = 1;
		//MenuToGame.Power_status = 1;
		//MenuToGame.Menu_Cellphone = 1; // can create menu socket again
        port1 = 5200; // server -> client
        port2 = 5300; // client -> server

        client_to_server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
            ProtocolType.Udp);

        StartCoroutine(Wait(0.8f));
	}

    private IEnumerator Wait(float time){
        yield return new WaitForSeconds(time);
        receiveThread = new Thread (new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start ();
    }

	void Update(){

        IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse(server_ip), port2);
        
        // Send player 2's position

        String position, str_posx, str_posy;
        float player_pos_x, player_pos_y;
        player_pos_x = player2.transform.position.x;
        player_pos_y = player2.transform.position.y;

        str_posx = player_pos_x.ToString();
        str_posy = player_pos_y.ToString();

        position = "1000 " + str_posx + " " + str_posy;

        Byte[] sendBytes = Encoding.ASCII.GetBytes(position);
        try{
            client_to_server.SendTo(sendBytes, anyIP);

            if (globalGameInfo.Sp_b == 1) // player has shot, send bullet info
            {
                String bullet_notif = send_shot();
                Byte[] send_bullet = Encoding.ASCII.GetBytes(bullet_notif);
                client_to_server.SendTo(send_bullet, anyIP);
            }
            //server_to_client.Close();
        }
        catch ( Exception e ){
            Console.WriteLine( e.ToString());
        }

        update_position(pos_x, pos_y); // update player 1's position

        if (shot == 1)
        {
            Spawn_shot(); // Shoot bullet
            shot = 0;
        }

        // spawn enemy 1

        if (enemy1_spawn == 1)
        {
            SpawnEnemy1(globalGameInfo.Sp_e1_x, globalGameInfo.Sp_e1_y);
            enemy1_spawn = 0; // reseting values
        }

        // Clearing enemy 1 if needed
        GameObject[] instances = GameObject.FindGameObjectsWithTag("Enemy1");
        for (int i = 0; i < instances.Length; i++)
        {
            if (instances[i].transform.position.y <= MIN_DEPTH)
            {
                Destroy(instances[i]);
                break;
            }
        }

        //this.score = MenuToGame.Score;
    }

    private void ReceiveData(){
        //recieve from server at port1
        print("Server recieve thread");
        server = new UdpClient (port1);
        while (true) {
        //while(MenuToGame.Alive == 1){
            try{
                //client = new UdpClient (port);
                if (MenuToGame.Alive == 0){
                    server.Close();
                    break;
                }
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse(server_ip), port1);

                byte[] data = server.Receive(ref anyIP);

                string text = Encoding.UTF8.GetString(data);
                //print (">> " + text);

                String[] separation = { " " }; 
                Int32 count = 2; 

                //String[] strlist = text.Split(separation, count, 
                //StringSplitOptions.RemoveEmptyEntries); 

                String[] strlist = text.Split(' ');

                if (strlist[0] == "1000"){ // player1 position update
                    
                    pos_x = float.Parse(strlist[1]);
                    pos_y = float.Parse(strlist[2]);
                    //update_position(pos_x, pos_y);
                } else if (strlist[0] == "2000") { // enemy1 spawn
                    float e1_posx = float.Parse(strlist[2]);
                    float e1_posy = float.Parse(strlist[3]);
                    //SpawnEnemy1(e1_posx, e2_posy);
                    globalGameInfo.Sp_e1_x = e1_posx;
                    globalGameInfo.Sp_e1_y = e1_posy;
                    enemy1_spawn = 1;
                } else if (strlist[0] == "1500") // bullet from player1
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
