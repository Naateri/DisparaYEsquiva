using UnityEngine;
using System.Collections;
//using static MenuToGame;
using static globalGameInfo;
public class mpEnemy1 : MonoBehaviour
{

	private float delay;// = 0.8f; //in seconds
	private float MIN_DEPTH = -5.5f;
	public GameObject cube, clone;

	public int player; // 0 -> server, 1 -> client

	// Use this for initialization
	void Start()
	{
		if (player == 0)
		{
			this.delay = 1.5f;

			InvokeRepeating("Spawn", 0.0f, delay);
		}
	}

	void Spawn()
	{
		if (globalGameInfo.Sp_e1 == 0 && player == 0) // can spawn
		{
			float rand_x = Random.Range(-6, 6);
			clone = Instantiate(cube, new Vector3(rand_x, 10, 0),
			 Quaternion.identity);
			globalGameInfo.Sp_e1 = 1; // notify server of a spawn
			globalGameInfo.Sp_e1_x = rand_x;
			globalGameInfo.Sp_e1_y = 10;
		}
	}

	void Update()
	{
		/*print("clone " + clone.transform.position.y);
		if (clone.transform.position.y <= MIN_DEPTH){
			Destroy(clone);
		}*/
		GameObject[] instances = GameObject.FindGameObjectsWithTag("Enemy1");
		for (int i = 0; i < instances.Length; i++)
		{
			if (instances[i].transform.position.y <= MIN_DEPTH)
			{
				Destroy(instances[i]);
				break;
			}
		}
	}

}