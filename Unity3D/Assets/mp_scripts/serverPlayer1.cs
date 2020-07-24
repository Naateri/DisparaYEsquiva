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
using static info;
/*
 * Códigos:
 * 1000: posicion
 * 1500: disparo
 * 1750: vida adicional al otro jugador
 * 2000: enemigo 1
 * 2100: enemigo 2
 * 2200: enemigo 3
 * 2300: enemigo 4
 * 2250: bala enemigo 3
 */
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
    private Vector3 speed = new Vector3(-7.5f, 0.0f, 0.0f);

    private int shot = 0; // 0->dont shoot, 1->shoot
    private int extra_life = 0; // 0->nothing happened, 1->add extra life
    private int looses_life = 0;

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

        //client_ip = "192.168.1.61"; // LAN
        client_ip = "26.65.120.130"; // JAZ

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

            String str_dir;
            int direction;

            direction = globalGameInfo.Dir_e1;
            str_dir = direction.ToString();

            //position = "2000 0 " + str_posx + " " + str_posy;
            position = "2000 " + str_dir + " " + str_posx + " " + str_posy;
            // position: 2000 direction posx posy
            globalGameInfo.Sp_e1 = 0;
            return position;
        }
        return "None"; // if no spawn
    }

    String spawn_enemy2()
    {
        if (globalGameInfo.Sp_e2 == 1) // has spawned
        {
            String position, str_posx, str_posy;
            float enemy2_pos_x, enemy2_pos_y;
            enemy2_pos_x = globalGameInfo.Sp_e2_x;
            enemy2_pos_y = globalGameInfo.Sp_e2_y;

            str_posx = enemy2_pos_x.ToString();
            str_posy = enemy2_pos_y.ToString();

            String str_dir;
            int direction;

            direction = globalGameInfo.Dir_e2;
            str_dir = direction.ToString();

            //position = "2100 0 " + str_posx + " " + str_posy;
            position = "2100 " + str_dir + " " +  str_posx + " " + str_posy;

            globalGameInfo.Sp_e2 = 0;
            return position;
        }
        return "None"; // if no spawn
    }

    String spawn_enemy3()
    {
        if (globalGameInfo.Sp_e3 == 1) // has spawned
        {
            String position, str_posx, str_posy;
            float enemy3_pos_x, enemy3_pos_y;
            enemy3_pos_x = globalGameInfo.Sp_e3_x;
            enemy3_pos_y = globalGameInfo.Sp_e3_y;

            str_posx = enemy3_pos_x.ToString();
            str_posy = enemy3_pos_y.ToString();

            String str_dir;
            int direction;

            direction = globalGameInfo.Dir_e3;
            str_dir = direction.ToString();

            position = "2200 " + str_dir + " " + str_posx + " " + str_posy;

            globalGameInfo.Sp_e3 = 0;
            return position;
        }
        return "None"; // if no spawn
    }

    String spawn_enemy3bullet()
    {
        if (globalGameInfo.E3_b == 1) { // enemy3 bullet spawn
            String position, str_posx, str_posy;
            float enemy3b_pos_x, enemy3b_pos_y;
            enemy3b_pos_x = globalGameInfo.E3_bx;
            enemy3b_pos_y = globalGameInfo.E3_by;

            str_posx = enemy3b_pos_x.ToString();
            str_posy = enemy3b_pos_y.ToString();

            // Forma: 2250 posx posy
            position = "2250 " + str_posx + " " + str_posy;

            globalGameInfo.E3_b = 0;
            return position;
        }
        return "None"; // if no spawn
    }

    String spawn_enemy4()
    {
        if (globalGameInfo.Sp_e4 == 1) // has spawned
        {
            String position, str_posx, str_posy;
            float enemy4_pos_x, enemy4_pos_y;
            enemy4_pos_x = globalGameInfo.Sp_e4_x;
            enemy4_pos_y = globalGameInfo.Sp_e4_y;

            str_posx = enemy4_pos_x.ToString();
            str_posy = enemy4_pos_y.ToString();

            String str_dir;
            int direction;

            direction = globalGameInfo.Dir_e4;
            str_dir = direction.ToString();

            position = "2300 " + str_dir + " " + str_posx + " " + str_posy;

            globalGameInfo.Sp_e4 = 0;
            return position;
        }
        return "None"; // if no spawn
    }

    void Spawn_shot() // player2 shot
    {
        print("Player 2 shot");
        b_clone = Instantiate(bullet, new Vector3(player2.transform.position.x - 1.5f,
            player2.transform.position.y, 0), Quaternion.identity);
        b_clone.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);
        b_clone.GetComponent<Rigidbody>().velocity = speed;
        gunshot = b_clone.GetComponent<AudioSource>();
        gunshot.Play();
    }

    String send_shot() // sending shotshot notification to player 2
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

    String send_extralife() // sending life to player 2
    {
        String protocol;
        if (globalGameInfo.Send_life == 1)
        {
            protocol = "1750 1"; // 1 looses life
            globalGameInfo.Send_life = 0;
            return protocol;
        }
        return "None";
    }

    void update_life() // Sent life, update local values
    {
        globalGameInfo.Player1_lives -= 1;
        globalGameInfo.Player2_lives += 1;
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3.0f);
        print("Loading scene");
        info.player1 = true;
        SceneManager.LoadScene("stats");
        

    }

    void Update(){

        if (MenuToGame.Alive == 0)
        {
            server_to_client.Close();
            StartCoroutine(EndGame());
            return;
        }

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

        String enemy2pos = spawn_enemy2();

        Byte[] enemy2pos_bytes = Encoding.ASCII.GetBytes(enemy2pos);

        String enemy3pos = spawn_enemy3();

        Byte[] enemy3pos_bytes = Encoding.ASCII.GetBytes(enemy3pos);

        String enemy3bullet = spawn_enemy3bullet();

        Byte[] enemy3bpos_bytes = Encoding.ASCII.GetBytes(enemy3bullet);

        String enemy4pos = spawn_enemy4();

        Byte[] enemy4pos_bytes = Encoding.ASCII.GetBytes(enemy4pos);

        Byte[] sendBytes = Encoding.ASCII.GetBytes(position);

        String addlife_notif = send_extralife();

        Byte[] sendExtraLife_bytes = Encoding.ASCII.GetBytes(addlife_notif);

        try
        {
            server_to_client.SendTo(sendBytes, anyIP); // sends position
            if (enemy1pos != "None")
            {
                server_to_client.SendTo(enemy1pos_bytes, anyIP); // sends spawn position only when it happens
            }
            if (enemy2pos != "None")
            {
                server_to_client.SendTo(enemy2pos_bytes, anyIP); // sends spawn position only when it happens
            }
            if (enemy3pos != "None")
            {
                server_to_client.SendTo(enemy3pos_bytes, anyIP); // sends spawn position only when it happens
            }
            if (enemy3bullet != "None")
            {
                server_to_client.SendTo(enemy3bpos_bytes, anyIP); // sends spawn position only when it happens
            }
            if (enemy4pos != "None")
            {
                server_to_client.SendTo(enemy4pos_bytes, anyIP); // sends spawn position only when it happens
            }
            if (addlife_notif != "None")
            {
                server_to_client.SendTo(sendExtraLife_bytes, anyIP);
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

        if (extra_life == 1) // add extra life to player 1
        {
            globalGameInfo.Add_life = 1;
            globalGameInfo.Looses_life = looses_life;
            extra_life = 0;
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
                } else if (strlist[0] == "1750") // extra life
                {
                    print("Got extra life");
                    extra_life = 1;
                    looses_life = int.Parse(strlist[1]);
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

    void OnGUI(){
        // levels

        Rect rectObj = new Rect(500, 30, 100, 40);

        GUIStyle style = new GUIStyle();

        style.alignment = TextAnchor.UpperLeft;
        style.font = myFont;
        style.fontSize = 45;
        style.normal.textColor = Color.green;

        if (globalGameInfo.Level_3 == 1)
        {
            GUI.Box(rectObj, "NIVEL 3", style);
        }
        else if (globalGameInfo.Level_2 == 1)
        {
            GUI.Box(rectObj, "NIVEL 2 ", style);
        }
        else if (globalGameInfo.Level_2 == 0)
        {
            GUI.Box(rectObj, "NIVEL 1 ", style);
        }

    }

}
