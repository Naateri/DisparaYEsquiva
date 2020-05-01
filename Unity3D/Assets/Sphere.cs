using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sphere : MonoBehaviour
{

	public int lives = 5;
	AudioSource cube_audio;
	private ParticleSystem particle;

	void Start(){
		print("Start Sphere");
	}

	/*private void GoBackToMenu(){
		print("Back to menu");
		SceneManager.LoadScene("menu");
	}

	IEnumerator BackToMenu(){
		yield return new WaitForSeconds(4);
		GoBackToMenu();
	}*/

	void Update(){
		/*if (this.lives <= 0){
			StartCoroutine(BackToMenu());
		}*/
	}

    private void OnCollisionEnter(Collision collision){

    	//only do this if colission is with cube
    	//not with bullet

    	if (collision.gameObject.tag == "Enemy1"){

    		this.lives--;
    		print("lives: " + this.lives);

    		particle = collision.gameObject.GetComponent<ParticleSystem>();
            particle.Play();

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
