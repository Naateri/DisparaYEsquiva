using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision){
    	if (collision.gameObject.tag == "PlayerObj") return;
		Destroy(this.gameObject);

		if (collision.gameObject.tag == "Enemy3Bullet"){
			Destroy(collision.gameObject);
		}	
	}
}
