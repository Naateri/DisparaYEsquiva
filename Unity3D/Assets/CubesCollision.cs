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
    
    void Start()
    {
        //audioWood = GetComponent<AudioSource>();
        //sr = GetComponent<SpriteRenderer>();
    }


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
            print("Score " + MenuToGame.Score);

            Destroy(this.gameObject, 2);
        }
    }
}
