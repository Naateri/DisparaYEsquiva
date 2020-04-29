using UnityEngine;
using System.Collections;
using static MenuToGame;

public class Cubes : MonoBehaviour {

	private float delay;// = 0.8f; //in seconds
	private float MIN_DEPTH = -5.5f;
	public GameObject cube, clone;
   
    // Use this for initialization
    void Start () {

    	if (MenuToGame.Difficulty == 1){
    		this.delay = 1.2f;
    	} else if (MenuToGame.Difficulty == 2){
    		this.delay = 0.6f;
    	}

		InvokeRepeating ("Spawn", 0.0f, delay);
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
		GameObject[] instances = GameObject.FindGameObjectsWithTag("Enemy1");
        for(int i = 0; i < instances.Length; i++){
            if (instances[i].transform.position.y <= MIN_DEPTH){
                Destroy(instances[i]);
                break; 
            }
        }
	}

}
