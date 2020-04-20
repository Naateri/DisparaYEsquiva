using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesCollision : MonoBehaviour
{
   private void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "MainBullet"){
	    	Destroy(this.gameObject);
    		Destroy(collision.gameObject);
		}
    }
}
