using UnityEngine;
using System.Collections;
//using static MenuToGame;
using static globalGameInfo;
public class mpEnemy4 : MonoBehaviour
{

	private float delay;// = 0.8f; //in seconds
	private float MIN_DEPTH = -5.5f;
	public GameObject cube, clone;

	public int player; // 0 -> server, 1 -> client

	private float timer = -1.0f;
	private float cur_time = 0.0f;
	private float wait_time = 60.0f + 10.0f; // 30 + 10
	//private float wait_time = 15.0f; // test

	private int spawns = 0;

	// Use this for initialization
	void Start()
	{
		if (player == 0)
		{
			//this.delay = 2.0f;

			//InvokeRepeating("Spawn", 10.0f, delay);
			// if (end_of_level_1)
			//Invoke("Spawn", 5.0f);
			timer = Time.time;
			// if (end_of_level_2)
			//Invoke("Spawn", 5.0f);
		}
	}

	void Spawn()
	{
		if (globalGameInfo.Sp_e4 == 0 && player == 0)
		{
			float rand_y = Random.Range(-0.5f, 6);
			clone = Instantiate(cube, new Vector3(-15, rand_y, 0),
				Quaternion.identity);
			clone.GetComponent<Rigidbody>().useGravity = false;
			clone.GetComponent<Rigidbody>().velocity = new Vector3(1.2f, 0.0f, 0.0f);
			globalGameInfo.Dir_e4 = 0; // Notify x axis spawn
			globalGameInfo.Sp_e4_x = -15;
			globalGameInfo.Sp_e4_y = rand_y;

			globalGameInfo.Sp_e4 = 1;
		}
	}

	void Update()
	{
		/*print("clone " + clone.transform.position.y);
		if (clone.transform.position.y <= MIN_DEPTH){
			Destroy(clone);
		}*/

		if (timer == -1.0f) timer = Time.time;

		cur_time = Time.time;

		float elapsed_time = cur_time - timer;

		if (spawns < 2 && cur_time != 0.0f && elapsed_time >= wait_time - 0.15f)
        {
			Invoke("Spawn", 0.0f);
			timer = -1.0f;
			cur_time = 0.0f;
			spawns++;
			wait_time = 60.0f;
        }

		GameObject[] instances = GameObject.FindGameObjectsWithTag("Enemy4");
		for (int i = 0; i < instances.Length; i++)
		{
			//instances[i].GetComponent<Rigidbody>().velocity = new Vector3(1.2f, 0.0f, 0.0f);
			if (instances[i].transform.position.y <= MIN_DEPTH || instances[i].transform.position.x >= 15.0f)
			{
				Destroy(instances[i]);
				break;
			}
		}
	}

}