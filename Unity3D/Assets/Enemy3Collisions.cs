using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Collisions : MonoBehaviour
{

	private float UPPER_THRESHOLD = 9.0f;
    private int lives = 1;
    public AudioSource audioGlass;
    private ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(9,9);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	void OnCollisionEnter(Collision collision){ //ignore certain collisions
		if (collision.gameObject.tag == "MainBullet"){

            if(transform.position.y >= UPPER_THRESHOLD){
                Destroy(collision.gameObject);
                return;
            }

            
            particle = this.gameObject.GetComponent<ParticleSystem>();
            particle.Play();

            audioGlass = this.gameObject.GetComponent<AudioSource>();
            audioGlass.Play();

            Destroy(collision.gameObject);
            

            print("Enemy 3 lives " + this.lives);

            //if (this.lives-1 == 0){
            globalGameInfo.Shots_hit++;
            globalGameInfo.Enemies_Destroyed++;

            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            (gameObject.GetComponent(typeof(BoxCollider)) as Collider).enabled = false;

            MenuToGame.Score += 3;
            print("Score " + MenuToGame.Score);

            Destroy(this.gameObject, 2);
            Destroy(collision.gameObject, 0);
            //}
            this.lives--;
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

            audioGlass = this.gameObject.GetComponent<AudioSource>();
            audioGlass.Play();

            Destroy(collision.gameObject);


            print("Enemy 3 lives " + this.lives);

            globalGameInfo.p2_Shots_hit++;
            globalGameInfo.p2_Enemies_Destroyed++;

            //if (this.lives-1 == 0){


            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            (gameObject.GetComponent(typeof(BoxCollider)) as Collider).enabled = false;

            MenuToGame.Score2 += 3;
            print("Score2 " + MenuToGame.Score2);

            Destroy(this.gameObject, 2);
            Destroy(collision.gameObject, 0);
            //}
            this.lives--;
        }
    }
}
