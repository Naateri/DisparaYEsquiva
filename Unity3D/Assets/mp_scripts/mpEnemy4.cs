﻿using UnityEngine;
using System.Collections;
//using static MenuToGame;
using static globalGameInfo;
public class mpEnemy4 : MonoBehaviour
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
			this.delay = 2.0f;

			InvokeRepeating("Spawn", 10.0f, delay);
		}
	}

	void Spawn()
	{
		if (globalGameInfo.Sp_e4 == 0 && player == 0) // can spawn
		{
			float direction = Random.Range(-5.0f, 5.0f);

			if (direction <= 0.0f) // y axis
			{
				float rand_x = Random.Range(-6, 6);
				clone = Instantiate(cube, new Vector3(rand_x, 10, 0),
					Quaternion.identity);
				clone.GetComponent<Rigidbody>().useGravity = false;
				clone.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, -5.0f, 0.0f);
				globalGameInfo.Dir_e4 = 1; // Notify y axis spawn
				globalGameInfo.Sp_e4_x = rand_x;
				globalGameInfo.Sp_e4_y = 10;
			}
			else
			{
				float rand_y = Random.Range(-1.5f, 6);
				clone = Instantiate(cube, new Vector3(-15, rand_y, 0),
					Quaternion.identity);
				clone.GetComponent<Rigidbody>().useGravity = false;
				clone.GetComponent<Rigidbody>().velocity = new Vector3(5.0f, 0.0f, 0.0f);
				globalGameInfo.Dir_e4 = 0; // Notify x axis spawn
				globalGameInfo.Sp_e4_x = -15;
				globalGameInfo.Sp_e4_y = rand_y;
			}

			globalGameInfo.Sp_e4 = 1; // notify server of a spawn
		}
	}

	void Update()
	{
		/*print("clone " + clone.transform.position.y);
		if (clone.transform.position.y <= MIN_DEPTH){
			Destroy(clone);
		}*/
		GameObject[] instances = GameObject.FindGameObjectsWithTag("Enemy4");
		for (int i = 0; i < instances.Length; i++)
		{
			if (instances[i].transform.position.y <= MIN_DEPTH || instances[i].transform.position.x >= 15.0f)
			{
				Destroy(instances[i]);
				break;
			}
		}
	}

}