using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Sphere : MonoBehaviour
{

	public int lives = 5;

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
    		Destroy(collision.gameObject, 2);

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
