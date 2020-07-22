using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static MenuToGame;
using static globalGameInfo;

public class mpBullets : MonoBehaviour
{
    public GameObject hero;
    public GameObject bullet, clone;
    public AudioSource gunshot;
    private float MAX_HEIGHT = 9.5f;
    private bool shot = false;
    private bool power = false;

    private bool AUTO_SHOOTING = false; //testing purposes

    private Vector3 speed = new Vector3(0.0f, 7.5f, 0.0f);

    private float delay = 0.3f;

    Thread receiveThread;
    UdpClient client;
    public int port;

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(10, 10);
        if (AUTO_SHOOTING)
        {
            InvokeRepeating("Spawn", delay, delay);
        }
        else
        {

            //client = new UdpClient (port);

            port = 5070;

            print("Sending to 127.0.0.1 : " + port);
            StartCoroutine(Wait(0.8f));

            //receiveThread = new Thread (new ThreadStart(ReceiveData));
            //receiveThread.IsBackground = true;
            //receiveThread.Start ();
        }
    }

    private void ReceiveData()
    {
        print("Recieve thread BULL");
        client = new UdpClient(port);
        while (true)
        {
            //while (MenuToGame.Alive == 1){
            try
            {
                if (MenuToGame.Alive == 0)
                {
                    client.Close();
                    break;
                }
                //client = new UdpClient (port);
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

                byte[] data = client.Receive(ref anyIP);

                string text = Encoding.UTF8.GetString(data);
                //print (">> BULLET " + text);
                //lastReceivedUDPPacket=text;
                //allReceivedUDPPackets=allReceivedUDPPackets+text;
                if (text == "100")
                {
                    shot = true;
                }
                else if (text == "200")
                {
                    MenuToGame.Power_status = 0; // cant activate power
                }
                else if (text == "300")
                {
                    MenuToGame.Power_status = 1; // can activate power
                }
                else if (text == "400")
                {
                    MenuToGame.Power_status = 2; // power IS activated
                    power = true;
                }
                //client.Close();
            }
            catch (Exception e)
            {
                print(e.ToString());
            }
        }
        //client.Close();
    }

    /*void OnApplicationQuit(){
        if (receiveThread != null) {
            //if (client != null)
            //    client.Close();
            receiveThread.Abort();
            Debug.Log(receiveThread.IsAlive); //must be false
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        /*print("bullet " + clone.transform.position.y);
    	if (clone.transform.position.y >= MAX_HEIGHT){
    		Destroy(clone);
    	}*/
        //try{
        //hero = GameObject.FindWithTag("PlayerObj");

        if (shot)
        {
            globalGameInfo.Sp_b = 1; // notify bullet shot from myself
            Spawn();
            shot = false;
        }

        if (power)
        {
            globalGameInfo.Send_life = 1; // Notify life
            update_life();
            power = false;
        }
        
        GameObject[] instances = GameObject.FindGameObjectsWithTag("MainBullet");
        for (int i = 0; i < instances.Length; i++)
        {
            if (instances[i].transform.position.y >= MAX_HEIGHT)
            {
                Destroy(instances[i]);
                break;
            }
        }
        

            // } catch (Exception e){
            ;
        //}

        /*if (MenuToGame.Alive == 0){
        /*if (MenuToGame.Alive == 0){
            //receiveThread.Abort();
            //client.Close();
        }*/
    }

    void Spawn()
    {

        globalGameInfo.Shots_done++;

        clone = Instantiate(bullet, new Vector3(hero.transform.position.x,
            hero.transform.position.y + 0.2f, 0), Quaternion.identity);
        clone.GetComponent<Rigidbody>().velocity = speed;
        gunshot = clone.GetComponent<AudioSource>();
        gunshot.Play();
    }

    void update_life() // Sent life, update local values
    {
        if (globalGameInfo.Player1_lives > 1)
        {
            globalGameInfo.Player1_lives -= 1;
            globalGameInfo.Player2_lives += 1;
        }
    }

}
