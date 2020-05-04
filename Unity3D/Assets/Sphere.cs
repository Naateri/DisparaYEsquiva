using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MenuToGame;

public class Sphere : MonoBehaviour
{

	public int lives = 5;
	public int score = 0;
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

		this.score = MenuToGame.Score;
	}

    private IEnumerator Break(Collision collision)
    {
        collision.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        particle = collision.gameObject.GetComponent<ParticleSystem>();
        particle.Play();

        cube_audio = collision.gameObject.GetComponent<AudioSource>();
        cube_audio.Play();

        collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
        (collision.gameObject.GetComponent(typeof(BoxCollider)) as Collider).enabled = false;
        //collision.gameObject.GetComponent<Rigidbody>().useGravity = false;
       

        yield return new WaitForSeconds(0.8f);

       
    }

    private void OnCollisionEnter(Collision collision){

    	//only do this if colission is with cube
    	//not with bullet

    	if (collision.gameObject.tag == "Enemy1"){

    		this.lives--;
    		print("lives: " + this.lives);

            StartCoroutine(Break(collision));
            Destroy(collision.gameObject, 2);
            //yield return new WaitForSeconds(particle.main.StartLifetime.constantMax);

        } else if (collision.gameObject.tag == "Enemy2") {
    		print("Collision with enemy2");
    		this.lives -= 2;
    		print("lives: " + this.lives);
            StartCoroutine(Break(collision));
          
            Destroy(collision.gameObject, 0);
    	}
    	
    	if (this.lives <= 0){
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

		Rect rectObj2 = new Rect(800, 450, 200, 400);
		GUIStyle style2 = new GUIStyle();

		style.alignment = TextAnchor.UpperLeft;

		GUI.Box(rectObj2, "SCORE: " + this.score, style2);

	}

}
