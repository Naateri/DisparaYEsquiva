using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MenuToGame;
using static Enemy2;

public class Enemy2Collisions : MonoBehaviour
{
    private float UPPER_THRESHOLD = 9.0f;
    private int lives = 2;
    public AudioSource audioMetal;
    private ParticleSystem particle;
    public Material newMaterial;
    Renderer rend;

    void Start()
    {
        Physics.IgnoreLayerCollision(9,9);
       
    }


    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag == "MainBullet")
        {
            if (transform.position.y >= UPPER_THRESHOLD)
            {
                Destroy(collision.gameObject);
                return;
            }
            particle = this.gameObject.GetComponent<ParticleSystem>();
            particle.Play();

            audioMetal = this.gameObject.GetComponent<AudioSource>();
            audioMetal.Play();

            Destroy(collision.gameObject);

            //GameObject enemy = this.gameObject.GetComponent<Enemy2>();
            //enemy.lives--

            print("Enemy 2 lives " + this.lives);


            rend = GetComponent<Renderer>();
            rend.enabled = true;
            rend.sharedMaterial = newMaterial;

            if (this.lives - 1 == 0)
            {
                this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                (gameObject.GetComponent(typeof(BoxCollider)) as Collider).enabled = false;

                MenuToGame.Score += 2;
                print("Score " + MenuToGame.Score);

                Destroy(this.gameObject, 2);
            }
            this.lives--;
        }
        else if (collision.gameObject.tag == "Bullet_P2"){
            if (transform.position.y >= UPPER_THRESHOLD)
            {
                Destroy(collision.gameObject);
                return;
            }
            particle = this.gameObject.GetComponent<ParticleSystem>();
            particle.Play();

            audioMetal = this.gameObject.GetComponent<AudioSource>();
            audioMetal.Play();

            Destroy(collision.gameObject);

            //GameObject enemy = this.gameObject.GetComponent<Enemy2>();
            //enemy.lives--

            print("Enemy 2 lives " + this.lives);

            rend = GetComponent<Renderer>();
            rend.enabled = true;
            rend.sharedMaterial = newMaterial;

            if (this.lives - 1 == 0)
            {
                this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                (gameObject.GetComponent(typeof(BoxCollider)) as Collider).enabled = false;

                MenuToGame.Score2 += 2;
                print("Score2 " + MenuToGame.Score2);

                Destroy(this.gameObject, 2);
            }
            this.lives--;
        }
    }
}


