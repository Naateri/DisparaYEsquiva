using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MenuToGame;

public class Sphere : MonoBehaviour
{

	private int lives = 10;
	private int score = 0;
	AudioSource cube_audio;
	private ParticleSystem particle;
    public Font myFont;
	public GameObject leaveButton;

	void Start(){
		print("Start Sphere");
		if (MenuToGame.Game_mode == 0){
			leaveButton.SetActive(false);
		}
		else{
			leaveButton.SetActive(true);
		}

		MenuToGame.Alive = 1;
		MenuToGame.Power_status = 1;
		MenuToGame.Menu_Cellphone = 1; // can create menu socket again
	}

	void Update(){
		/*if (this.lives <= 0){
			StartCoroutine(BackToMenu());
		}*/

		this.score = MenuToGame.Score;
	}

    private IEnumerator Break(Collision collision)
    {
    	//impacto de cubo con el jugador
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

    private IEnumerator RestartGame(){
    	//Destroy(gameObject, 2);
    	yield return new WaitForSeconds(2.0f);
    	gameObject.GetComponent<MeshRenderer>().enabled = false;
    	(gameObject.GetComponent(typeof(SphereCollider)) as Collider).enabled = false;

    	yield return new WaitForSeconds(2.0f);
    	SceneManager.LoadScene("menu");
    	Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision){

    	//only do this if colission is with cube
    	//not with bullet

    	if (collision.gameObject.tag == "Enemy1"){

    		if (MenuToGame.Game_mode == 0)
    			this.lives--;
    		print("lives: " + this.lives);

            StartCoroutine(Break(collision));
            Destroy(collision.gameObject, 2);
            //yield return new WaitForSeconds(particle.main.StartLifetime.constantMax);

        } else if (collision.gameObject.tag == "Enemy2") {
    		print("Collision with enemy2");
    		if (MenuToGame.Game_mode == 0)
    			this.lives -= 2;
    		print("lives: " + this.lives);
            StartCoroutine(Break(collision));
          
            Destroy(collision.gameObject, 2);
    	} else if (collision.gameObject.tag == "Enemy3") {
    		print("Collision with enemy3");
    		if (MenuToGame.Game_mode == 0)
    			this.lives -= 3;
    		print("lives: " + this.lives);
    		// uncomment when enemy3 has sound collision and particle effect
            //StartCoroutine(Break(collision));
          
            Destroy(collision.gameObject, 2);
    	} else if (collision.gameObject.tag == "Enemy3Bullet"){
    		print("Collision with enemy3 bullet");
    		if (MenuToGame.Game_mode == 0)
    			this.lives -= 1;
    		print("lives: " + this.lives);
    		Destroy(collision.gameObject,0);
    	}
    	
    	if (this.lives <= 0){
    		MenuToGame.Alive = 0;
    		//Destroy(gameObject, 2);
    		StartCoroutine(RestartGame());
    	}
    }

    void OnGUI(){
		Rect rectObj= new Rect (40,450,200,400);
		
		GUIStyle style = new GUIStyle ();
		
		style.alignment = TextAnchor.UpperLeft;
        style.font = myFont;
        style.fontSize = 35;
        style.normal.textColor = Color.blue;
		if (this.lives > 0){
			GUI.Box (rectObj,"VIDAS: " + this.lives, 
		          style );
		} else {
			GUI.Box (rectObj,"GAME OVER", 
		          style );
		}
      
        Rect powerNotif = new Rect(750, 30, 200, 350);
        style.fontSize = 30;
        style.normal.textColor = Color.green;
        if (MenuToGame.Power_status == 0){
			GUI.Box (powerNotif, "POWER IS NOT AVAILABLE", style);
		} else if (MenuToGame.Power_status == 1){
			GUI.Box (powerNotif, "POWER AVAILABLE", style);
		} else if (MenuToGame.Power_status == 2){
			GUI.Box (powerNotif, "POWER ACTIVATED", style);
		}

		Rect rectObj2 = new Rect(800, 300, 200, 350);
		GUIStyle style2 = new GUIStyle();

		style.alignment = TextAnchor.UpperLeft;

		//GUI.Box(rectObj2, "SCORE: " + this.score, style2);

	}

}
