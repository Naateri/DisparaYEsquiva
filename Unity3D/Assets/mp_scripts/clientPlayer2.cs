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

    // Enemy 2

    public GameObject enemy2, clone2;

    // 0 -> no spawn
    // 1 -> spawn
    private int enemy2_spawn = 0;

    // Enemy 3

    public GameObject enemy3, clone3;

    // 0 -> no spawn
    // 1 -> spawn
    private int enemy3_spawn = 0;

    // Enemy 4

    public GameObject enemy4, clone4;

    // 0 -> no spawn
    // 1 -> spawn
    private int enemy4_spawn = 0;


    // Enemy 3 bullets

    public GameObject enemy3bullet, bullet_clone_x, bullet_clone_y;
    private Vector3 bulletSpeed_y = new Vector3(0.0f, -7.5f, 0.0f);
    private Vector3 bulletSpeed_x = new Vector3(7.5f, 0.0f, 0.0f);

    // 0 -> no spawn
    // 1 -> spawn
    private int enemy3b_spawn = 0;

    // Sockets

    Thread receiveThread, sendThread;
    UdpClient server; // connection to server
    Socket client_to_server;
    public int port1, port2;

    //String server_ip = "192.168.1.133";
    String server_ip = "26.65.123.2"; // RENU
    //String server_ip = "26.65.120.130"; // JAZ



    float pos_x, pos_y; // stores positions recieved from player 1

    private void update_position(float x, float y){
        player1.transform.position = new Vector3(x,y,0);
    }

    void SpawnEnemy1(float x, float y)
    {
        clone = Instantiate(cube, new Vector3(x, y, 0),
             Quaternion.identity);
        if (globalGameInfo.Dir_e1 == 0)
        {
            clone.GetComponent<Rigidbody>().velocity = new Vector3(5.0f, 0.0f, 0.0f);
        } else if (globalGameInfo.Dir_e1 == 1)
        {
            clone.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, -5.0f, 0.0f);
        }
       
        clone.GetComponent<Rigidbody>().useGravity = false;
    }

    void SpawnEnemy2(float x, float y)
    {
        clone2 = Instantiate(enemy2, new Vector3(x, y, 0),
            Quaternion.identity);
        if (globalGameInfo.Dir_e2 == 0)
        {
            clone2.GetComponent<Rigidbody>().velocity = new Vector3(5.0f, 0.0f, 0.0f);
        } else if (globalGameInfo.Dir_e2 == 1)
        {
            clone2.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, -5.0f, 0.0f);
        }
        clone2.GetComponent<Rigidbody>().useGravity = false;
    }

    void SpawnEnemy3(float x, float y)
    {
        clone3 = Instantiate(enemy3, new Vector3(x, y, 0),
            Quaternion.identity);
        if (globalGameInfo.Dir_e3 == 0)
        {
            clone3.GetComponent<Rigidbody>().velocity = new Vector3(4.0f, 0.0f, 0.0f);
        }
        else if (globalGameInfo.Dir_e3 == 1)
        {
            clone3.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, -4.0f, 0.0f);
        }
        clone3.GetComponent<Rigidbody>().useGravity = false;
    }

    void SpawnEnemy3Bullet(float x, float y)
    {
        Physics.IgnoreLayerCollision(9, 9);
        bullet_clone_x = Instantiate(enemy3bullet, new Vector3(x + 1.0f, y, 0),
            Quaternion.identity);
        bullet_clone_x.GetComponent<Rigidbody>().velocity = bulletSpeed_x;
        bullet_clone_x.GetComponent<Rigidbody>().useGravity = false;
        bullet_clone_x.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);

        bullet_clone_y = Instantiate(enemy3bullet, new Vector3(x, y - 1.0f, 0), Quaternion.identity);
        bullet_clone_y.GetComponent<Rigidbody>().velocity = bulletSpeed_y;
        bullet_clone_y.GetComponent<Rigidbody>().useGravity = false;
    }

    void SpawnEnemy4(float x, float y)
    {
        clone4 = Instantiate(enemy4, new Vector3(x, y, 0),
            Quaternion.identity);
        if (globalGameInfo.Dir_e4 == 0)
        {
            clone4.GetComponent<Rigidbody>().velocity = new Vector3(4.0f, 0.0f, 0.0f);
        }
        else if (globalGameInfo.Dir_e4 == 1)
        {
            clone4.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, -4.0f, 0.0f);
        }
        clone4.GetComponent<Rigidbody>().useGravity = false;
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
		print("Client mp");

		//MenuToGame.Alive = 1;
		//MenuToGame.Power_status = 1;
		//MenuToGame.Menu_Cellphone = 1; // can create menu socket again
        port1 = 5200; // server -> client
        port2 = 5300; // client -> server

        Physics.IgnoreLayerCollision(9, 9, true);

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

        if (enemy2_spawn == 1)
        {
            SpawnEnemy2(globalGameInfo.Sp_e2_x, globalGameInfo.Sp_e2_y);
            enemy2_spawn = 0;
        }

        if (enemy3_spawn == 1)
        {
            SpawnEnemy3(globalGameInfo.Sp_e3_x, globalGameInfo.Sp_e3_y);
            enemy3_spawn = 0;
        }

        if (enemy3b_spawn == 1)
        {
            SpawnEnemy3Bullet(globalGameInfo.E3_bx, globalGameInfo.E3_by);
            enemy3b_spawn = 0;
        }

        if (enemy4_spawn == 1)
        {
            SpawnEnemy4(globalGameInfo.Sp_e4_x, globalGameInfo.Sp_e4_y);
            enemy4_spawn = 0;
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
        GameObject[] instances = GameObject.FindGameObjectsWithTag("Enemy2");
        for (int i = 0; i < instances.Length; i++)
        {
            if (instances[i].transform.position.y <= MIN_DEPTH)
            {
                Destroy(instances[i]);
                break;
            }
        }
        GameObject[] instances = GameObject.FindGameObjectsWithTag("Enemy3");
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
                    int direction = int.Parse(strlist[1]);
                    //SpawnEnemy1(e1_posx, e2_posy);
                    globalGameInfo.Sp_e1_x = e1_posx;
                    globalGameInfo.Sp_e1_y = e1_posy;
                    globalGameInfo.Dir_e1 = direction;
                    enemy1_spawn = 1;
                } else if (strlist[0] == "2100") // enemy2 spawn
                {
                    float e2_posx = float.Parse(strlist[2]);
                    float e2_posy = float.Parse(strlist[3]);
                    int direction = int.Parse(strlist[1]);

                    globalGameInfo.Sp_e2_x = e2_posx;
                    globalGameInfo.Sp_e2_y = e2_posy;
                    globalGameInfo.Dir_e2 = direction;
                    enemy2_spawn = 1;
                }
                else if (strlist[0] == "2200") // enemy3 spawn
                {
                    float e3_posx = float.Parse(strlist[2]);
                    float e3_posy = float.Parse(strlist[3]);
                    int direction = int.Parse(strlist[1]);

                    globalGameInfo.Sp_e3_x = e3_posx;
                    globalGameInfo.Sp_e3_y = e3_posy;
                    globalGameInfo.Dir_e3 = direction;
                    enemy3_spawn = 1;
                } else if (strlist[0] == "2250") // enemy3 bullet spawn
                {
                    float e3b_posx = float.Parse(strlist[1]);
                    float e3b_posy = float.Parse(strlist[2]);

                    globalGameInfo.E3_bx = e3b_posx;
                    globalGameInfo.E3_by = e3b_posy;
                    enemy3b_spawn = 1;
                }
                else if (strlist[0] == "2300") // enemy4 spawn
                {
                    float e4_posx = float.Parse(strlist[2]);
                    float e4_posy = float.Parse(strlist[3]);
                    int direction = int.Parse(strlist[1]);

                    globalGameInfo.Sp_e4_x = e4_posx;
                    globalGameInfo.Sp_e4_y = e4_posy;
                    globalGameInfo.Dir_e4 = direction;
                    enemy4_spawn = 1;
                }
                else if (strlist[0] == "1500") // bullet from player1
                {
                    shot = 1;
                } else
                {
                    ; // do nothing
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
