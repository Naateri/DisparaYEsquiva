using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MenuToGame;

public class CubesCollision : MonoBehaviour
{
    public AudioSource audioWood;
    private float UPPER_THRESHOLD = 9.0f;

    private ParticleSystem particle;
    //private SpriteRenderer sr ;
    public Font myFont;
    
    void Start()
    {
        //audioWood = GetComponent<AudioSource>();
        //sr = GetComponent<SpriteRenderer>();
        Physics.IgnoreLayerCollision(9,9);
    }

    /*
    void OnGUI()
    {
        Rect rectObj = new Rect(0, 20, 200, 400);

        GUIStyle style = new GUIStyle();
        style.font = myFont;
        style.fontSize = 40;
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.UpperRight;

        GUI.Box(rectObj, "SCORE : " + MenuToGame.Score, style);

    }
    */



    private void OnCollisionEnter(Collision collision){

		if (collision.gameObject.tag == "MainBullet"){

            if(transform.position.y >= UPPER_THRESHOLD){
                Destroy(collision.gameObject);
                return;
            }

            particle = this.gameObject.GetComponent<ParticleSystem>();
            particle.Play();

            audioWood = this.gameObject.GetComponent<AudioSource>();
            audioWood.Play();

            


            Destroy(collision.gameObject);

            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            (gameObject.GetComponent(typeof(BoxCollider)) as Collider).enabled = false;

            MenuToGame.Score++;
  

            Destroy(this.gameObject, 2);
        }

        else if (collision.gameObject.tag == "Bullet_P2")
        {

            if (transform.position.y >= UPPER_THRESHOLD)
            {
                Destroy(collision.gameObject);
                return;
            }

            particle = this.gameObject.GetComponent<ParticleSystem>();
            particle.Play();

            audioWood = this.gameObject.GetComponent<AudioSource>();
            audioWood.Play();




            Destroy(collision.gameObject);

            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            (gameObject.GetComponent(typeof(BoxCollider)) as Collider).enabled = false;

            MenuToGame.Score2++;


            Destroy(this.gameObject, 2);
        }
    }
}
