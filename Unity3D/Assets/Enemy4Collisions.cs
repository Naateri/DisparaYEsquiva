using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MenuToGame;
using static globalGameInfo;

public class Enemy4Collisions : MonoBehaviour
{
    // Start is called before the first frame update
    private float UPPER_THRESHOLD = 9.0f;
    private int lives = 10;
    public AudioSource audioMetal;
    private ParticleSystem particle;
    public Material newMaterial;
    Renderer rend;

    void Start()
    {
        Physics.IgnoreLayerCollision(9, 9);

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "MainBullet") 
        { // detect collision with P1Bullet
            if (transform.position.y >= UPPER_THRESHOLD)
            {
                Destroy(collision.gameObject);
                return;
            }
            /*particle = this.gameObject.GetComponent<ParticleSystem>();
            particle.Play();

            audioMetal = this.gameObject.GetComponent<AudioSource>();
            audioMetal.Play();*/

            Destroy(collision.gameObject);

            //GameObject enemy = this.gameObject.GetComponent<Enemy2>();
            //enemy.lives--

            print("Enemy 4 lives " + this.lives);


            /*rend = GetComponent<Renderer>();
            rend.enabled = true;
            rend.sharedMaterial = newMaterial;*/

            if (this.lives - 1 == 0)
            {
                //this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                //(gameObject.GetComponent(typeof(SphereCollider)) as Collider).enabled = false;
                //gameObject.GetComponent<Collider>().enabled = false;

                MenuToGame.Score += 1;
                print("Score " + MenuToGame.Score);

                if (globalGameInfo.Level_2 == 0) globalGameInfo.Level_2 = 1;
                else globalGameInfo.Level_3 = 1;

                // Notifying end of level by destroying enemy 4

                Destroy(this.gameObject, 0);
            }
            this.lives--;
        }
        else if (collision.gameObject.tag == "Bullet_P2")
        { // Detect collision with P2 bullet
            if (transform.position.y >= UPPER_THRESHOLD)
            {
                Destroy(collision.gameObject);
                return;
            }
            /*particle = this.gameObject.GetComponent<ParticleSystem>();
            particle.Play();

            audioMetal = this.gameObject.GetComponent<AudioSource>();
            audioMetal.Play();*/

            Destroy(collision.gameObject);

            //GameObject enemy = this.gameObject.GetComponent<Enemy2>();
            //enemy.lives--

            print("Enemy 4 lives " + this.lives);

            /*rend = GetComponent<Renderer>();
            rend.enabled = true;
            rend.sharedMaterial = newMaterial;*/

            if (this.lives - 1 == 0)
            {
                //this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                //(gameObject.GetComponent(typeof(SphereCollider)) as Collider).enabled = false;
                //gameObject.GetComponent<Collider>().enabled = false;

                MenuToGame.Score2 += 1;
                print("Score2 " + MenuToGame.Score2);

                if (globalGameInfo.Level_2 == 0) globalGameInfo.Level_2 = 1;
                else globalGameInfo.Level_3 = 1;

                // Notifying end of level by destroying enemy 4

                Destroy(this.gameObject, 0);
            }
            this.lives--;
        } else if (collision.gameObject.tag == "PlayerObj")
        {
            // Destroy player
            //Destroy(collision.gameObject);

        }
    }
}
