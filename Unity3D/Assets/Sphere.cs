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
    	this.lives--;
    	print("lives: " + this.lives);
    	//Destroy(collision.gameObject);

    	if (this.lives == 0){
    		Destroy(gameObject, 2);
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
