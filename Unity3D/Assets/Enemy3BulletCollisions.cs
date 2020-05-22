using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3BulletCollisions : MonoBehaviour
{
	  private float UPPER_THRESHOLD = 9.0f;
   


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
		/*if (collision.gameObject.tag == "Enemy1" || 
			collision.gameObject.tag == "Enemy2" ||
			collision.gameObject.tag == "Enemy3" ||
			collision.gameObject.tag == "Enemy3Bullet"){*/
		/*if (collision.gameObject.layer == this.gameObject.layer || collision.gameObject.tag == this.gameObject.tag){
			Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), 
				this.gameObject.GetComponent<Collider>());
		}*/
		if (collision.gameObject.tag == "MainBullet"){ //impact with bullet

            if(transform.position.y >= UPPER_THRESHOLD){
                Destroy(collision.gameObject);
                return;
            }

         



            Destroy(collision.gameObject, 0);

            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            (gameObject.GetComponent(typeof(BoxCollider)) as Collider).enabled = false;

            //MenuToGame.Score++;
            //print("Score " + MenuToGame.Score);

            Destroy(this.gameObject, 0);
        }
    }
}
