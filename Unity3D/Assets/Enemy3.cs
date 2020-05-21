using UnityEngine;
using System.Collections;
using static MenuToGame;

public class Enemy3 : MonoBehaviour {

	private float delay, start_delay;
	private float MIN_DEPTH = -5.5f;
	public GameObject cube, clone, bullet, bullet_clone;
   
   	private Vector3 speed = new Vector3(0.0f, -2.5f, 0.0f);
   	private Vector3 bulletSpeed = new Vector3(0.0f, -3.0f, 0.0f);

    // Use this for initialization
    void Start () {

    	if (MenuToGame.Difficulty == 1){
    		this.delay = 5.0f;
    		this.start_delay = 2.5f;
    	} else if (MenuToGame.Difficulty == 2){
    		this.delay = 2.5f;
    		this.start_delay = 2.5f;
    	}

		InvokeRepeating ("Spawn", start_delay, delay);
	}

	void SpawnBullet(){
		if (clone == null){
			return;
		}
		Vector3 position = clone.transform.position;
		bullet_clone = Instantiate (bullet, new Vector3 (position.x, position.y, position.z), Quaternion.identity);
		bullet_clone.GetComponent<Rigidbody>().velocity = bulletSpeed;
	}

	void Spawn () {
		clone = Instantiate (cube, new Vector3 (Random.Range (-6, 6), 10, 0),
		 Quaternion.identity);
		clone.GetComponent<Rigidbody>().velocity = speed;
		InvokeRepeating("SpawnBullet", 0.0f, 2.5f);
	}

	void Update(){
		/*print("clone " + clone.transform.position.y);
		if (clone.transform.position.y <= MIN_DEPTH){
			Destroy(clone);
		}*/
		GameObject[] instances = GameObject.FindGameObjectsWithTag("Enemy3");
        for(int i = 0; i < instances.Length; i++){
            if (instances[i].transform.position.y <= MIN_DEPTH){
                Destroy(instances[i]);
                break; 
            }
        }

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Enemy3Bullet");
        for(int i = 0; i < bullets.Length; i++){
            if (bullets[i].transform.position.y <= MIN_DEPTH){
                Destroy(bullets[i]);
                break; 
            }
        }
	}

}
