using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
	public GameObject hero;
	public GameObject bullet, clone;
	private float MAX_HEIGHT = 9.0f;

	private Vector3 speed = new Vector3(0.0f, 7.5f, 0.0f);

	private float delay = 2.0f;
    // Start is called before the first frame update
    void Start(){
        InvokeRepeating("Spawn", delay, delay);
    }

    // Update is called once per frame
    void Update(){
    	print(clone.transform.position.y);
    	if (clone.transform.position.y >= MAX_HEIGHT){
    		Destroy(clone);
    	}
    }

    void Spawn () {
		clone = Instantiate (bullet, new Vector3 (hero.transform.position.x, 
			hero.transform.position.y + 2.2f, 0), Quaternion.identity);
		clone.GetComponent<Rigidbody>().velocity = speed;
	}
}
