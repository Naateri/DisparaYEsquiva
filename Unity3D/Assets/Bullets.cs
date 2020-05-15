using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static MenuToGame;

public class Bullets : MonoBehaviour
{
	public GameObject hero;
	public GameObject bullet, clone;
    public AudioSource gunshot;
	private float MAX_HEIGHT = 9.5f;
    private bool shot = false;

    private bool AUTO_SHOOTING = false; //testing purposes

	private Vector3 speed = new Vector3(0.0f, 7.5f, 0.0f);

	private float delay = 0.3f;

    Thread receiveThread;
    UdpClient client;
    public int port;

    // Start is called before the first frame update
    void Start(){

        if (AUTO_SHOOTING){
            InvokeRepeating("Spawn", delay, delay);
        } else {

            port = 5070;

            print ("Sending to 127.0.0.1 : " + port);

            receiveThread = new Thread (new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start ();
        }
    }

    private void ReceiveData(){
        //client = new UdpClient (port);
        //while (true) {
        while (MenuToGame.Alive == 1){
            try{
                client = new UdpClient (port);
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                byte[] data = client.Receive(ref anyIP);

                string text = Encoding.UTF8.GetString(data);
                print (">> " + text);
                //lastReceivedUDPPacket=text;
                //allReceivedUDPPackets=allReceivedUDPPackets+text;
                if (text == "100"){
                    shot = true;
                }
                client.Close();
            }catch(Exception e){
                print (e.ToString());
            }
        }
        //client.Close();
    }

    void OnApplicationQuit(){
        if (receiveThread != null) {
            receiveThread.Abort();
            if (client != null)
                client.Close();
            Debug.Log(receiveThread.IsAlive); //must be false
        }
    }

    // Update is called once per frame
    void Update(){
    	/*print("bullet " + clone.transform.position.y);
    	if (clone.transform.position.y >= MAX_HEIGHT){
    		Destroy(clone);
    	}*/
        //try{
            //hero = GameObject.FindWithTag("PlayerObj");

            if (shot){
                Spawn();
                shot = false;
            }

            GameObject[] instances = GameObject.FindGameObjectsWithTag("MainBullet");
            for(int i = 0; i < instances.Length; i++){
                if (instances[i].transform.position.y >= MAX_HEIGHT){
                    Destroy(instances[i]);
                    break; 
                }
            }

            
       // } catch (Exception e){
            ;
        //}

        if (MenuToGame.Alive == 0){
            receiveThread.Abort();
            //client.Close();
        }
    }

    void Spawn () {
		clone = Instantiate (bullet, new Vector3 (hero.transform.position.x, 
			hero.transform.position.y + 0.2f, 0), Quaternion.identity);
		clone.GetComponent<Rigidbody>().velocity = speed;
        gunshot = clone.GetComponent<AudioSource>();
        gunshot.Play();
	}
}
