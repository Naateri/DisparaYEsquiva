using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Sphere : MonoBehaviour
{

	public int lives = 5;
	AudioSource cube_audio;

	void Start(){
		print("Start Sphere");
	}

	void Update(){
		
	}

    private void OnCollisionEnter(Collision collision){

    	//only do this if colission is with cube
    	//not with bullet

    	if (collision.gameObject.tag == "Enemy1"){

    		this.lives--;
    		print("lives: " + this.lives);

    		cube_audio = collision.gameObject.GetComponent<AudioSource>();
    		cube_audio.Play();

    		collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
    		(collision.gameObject.GetComponent(typeof(BoxCollider)) as Collider).enabled = false;
    		collision.gameObject.GetComponent<Rigidbody>().useGravity = false;

    		Destroy(collision.gameObject,2);

    		if (this.lives == 0){
    			Destroy(gameObject, 2);
    		}
    	}
    }

    void OnGUI(){
		Rect rectObj= new Rect (40,450,200,400);
		
		GUIStyle style = new GUIStyle ();
		
		style.alignment = TextAnchor.UpperLeft;
		
		if (this.lives > 0){
			GUI.Box (rectObj,"Lives remaining: " + this.lives, 
		          style );
		} else {
			GUI.Box (rectObj,"GAME OVER", 
		          style );
		}

	}

}
