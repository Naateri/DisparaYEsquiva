using UnityEngine;
using System.Collections;
using static MenuToGame;

public class Enemy2 : MonoBehaviour {

	private float delay, start_delay;
	private float MIN_DEPTH = -5.5f;
	public GameObject cube, clone;
   
    // Use this for initialization
    void Start () {

    	if (MenuToGame.Difficulty == 1){
    		this.delay = 1.8f;
    		this.start_delay = 1.5f;
    	} else if (MenuToGame.Difficulty == 2){
    		this.delay = 0.9f;
    		this.start_delay = 0.75f;
    	}

		InvokeRepeating ("Spawn", start_delay, delay);
	}

	void Spawn () {
		clone = Instantiate (cube, new Vector3 (Random.Range (-6, 6), 10, 0),
		 Quaternion.identity);
	}

	void Update(){
		/*print("clone " + clone.transform.position.y);
		if (clone.transform.position.y <= MIN_DEPTH){
			Destroy(clone);
		}*/
		GameObject[] instances = GameObject.FindGameObjectsWithTag("Enemy2");
        for(int i = 0; i < instances.Length; i++){
            if (instances[i].transform.position.y <= MIN_DEPTH){
                Destroy(instances[i]);
                break; 
            }
        }
	}

}
