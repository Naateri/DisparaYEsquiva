using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MenuToGame;
using static Enemy2;

public class Enemy2Collisions : MonoBehaviour
{
    private float UPPER_THRESHOLD = 9.0f;
    private int lives = 2;

    void Start()
    {
        
    }


    private void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "MainBullet"){

            if(transform.position.y >= UPPER_THRESHOLD){
                Destroy(collision.gameObject);
                return;
            }


            Destroy(collision.gameObject);

            //GameObject enemy = this.gameObject.GetComponent<Enemy2>();
            //enemy.lives--

            print("Enemy 2 lives " + this.lives);

            if (this.lives-1 == 0){


            	this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            	(gameObject.GetComponent(typeof(BoxCollider)) as Collider).enabled = false;

            	MenuToGame.Score += 2;
            	print("Score " + MenuToGame.Score);

            	Destroy(this.gameObject, 2);
            }
            this.lives--;
        }
    }
}
