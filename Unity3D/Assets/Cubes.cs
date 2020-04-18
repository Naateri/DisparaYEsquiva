using UnityEngine;
using System.Collections;

public class Cubes : MonoBehaviour {

	private float delay = 1.1f; //in seconds
	private float MIN_DEPTH = -5.5f;
	public GameObject cube;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("Spawn", delay, 0.0f);
	}

	void Spawn () {
		Instantiate (cube, new Vector3 (Random.Range (-6, 6), 10, 0), Quaternion.identity);
	}

	void Update(){
		if (cube.transform.position.y <= MIN_DEPTH){
			Destroy(cube);
		}
	}
}
