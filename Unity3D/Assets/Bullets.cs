using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Bullets : MonoBehaviour
{
	public GameObject hero;
	public GameObject bullet, clone;
    public AudioSource gunshot;
	private float MAX_HEIGHT = 10.0f;
    private bool shot = false;

	private Vector3 speed = new Vector3(0.0f, 7.5f, 0.0f);

	private float delay = 1.0f;

    Thread receiveThread;
    UdpClient client;
    public int port;

    // Start is called before the first frame update
    void Start(){
        //InvokeRepeating("Spawn", delay, delay);

        port = 5070;

        print ("Sending to 127.0.0.1 : " + port);

        receiveThread = new Thread (new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start ();
    }

    private void ReceiveData(){
        client = new UdpClient (port);
        while (true) {
            try{
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                byte[] data = client.Receive(ref anyIP);

                string text = Encoding.UTF8.GetString(data);
                print (">> " + text);
                //lastReceivedUDPPacket=text;
                //allReceivedUDPPackets=allReceivedUDPPackets+text;
                if (text == "100"){
                    shot = true;
                }
            }catch(Exception e){
                print (e.ToString());
            }
        }
    }

    void OnApplicationQuit(){
        if (receiveThread != null) {
            receiveThread.Abort();
            Debug.Log(receiveThread.IsAlive); //must be false
        }
    }

    // Update is called once per frame
    void Update(){
    	/*print("bullet " + clone.transform.position.y);
    	if (clone.transform.position.y >= MAX_HEIGHT){
    		Destroy(clone);
    	}*/
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
    }

    void Spawn () {
		clone = Instantiate (bullet, new Vector3 (hero.transform.position.x, 
			hero.transform.position.y + 0.2f, 0), Quaternion.identity);
		clone.GetComponent<Rigidbody>().velocity = speed;
        gunshot = clone.GetComponent<AudioSource>();
        gunshot.Play();
	}
}
