using UnityEngine;
using System.Collections;
//using static MenuToGame;
using static globalGameInfo;

public class mpEnemy3 : MonoBehaviour
{

	private float delay;// = 0.8f; //in seconds
	private float MIN_DEPTH = -5.5f;
	public GameObject cube, clone, bullet, bullet_clone_x, bullet_clone_y;

	private Vector3 bulletSpeed_y = new Vector3(0.0f, -7.5f, 0.0f);
	private Vector3 bulletSpeed_x = new Vector3(7.5f, 0.0f, 0.0f);

	public int player; // 0 -> server, 1 -> client

	// Use this for initialization
	void Start()
	{
		if (player == 0)
		{
			this.delay = 11.5f;
			//this.delay = 2.5f;
			Physics.IgnoreLayerCollision(9, 9, true);

			//InvokeRepeating("Spawn", 10.0f + delay, delay);
			InvokeRepeating("Spawn", 5.0f, 2.5f);
		}
	}

	void SpawnBullet()
	{
		if (clone == null)
		{
			return;
		}
		Vector3 position = clone.transform.position;
		bullet_clone_x = Instantiate(bullet, new Vector3(position.x+1.0f, position.y, position.z), Quaternion.identity);
		bullet_clone_x.GetComponent<Rigidbody>().velocity = bulletSpeed_x;
		bullet_clone_x.GetComponent<Rigidbody>().useGravity = false;
		bullet_clone_x.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);

		bullet_clone_y = Instantiate(bullet, new Vector3(position.x, position.y-1.0f, position.z), Quaternion.identity);
		bullet_clone_y.GetComponent<Rigidbody>().velocity = bulletSpeed_y;
		bullet_clone_y.GetComponent<Rigidbody>().useGravity = false;

		// notify spawn of enemy3 bullet

		globalGameInfo.E3_b = 1;
		globalGameInfo.E3_bx = position.x;
		globalGameInfo.E3_by = position.y;

	}

	void Spawn()
	{
		if (globalGameInfo.Sp_e3 == 0 && player == 0) // can spawn
		{
			float direction = Random.Range(-5.0f, 5.0f);

			if (direction <= 0.0f) // y axis
			{
				float rand_x = Random.Range(-6, 6);
				clone = Instantiate(cube, new Vector3(rand_x, 10, 0),
					Quaternion.identity);
				clone.GetComponent<Rigidbody>().useGravity = false;
				clone.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, -4.0f, 0.0f);
				globalGameInfo.Dir_e3 = 1; // Notify y axis spawn
				globalGameInfo.Sp_e3_x = rand_x;
				globalGameInfo.Sp_e3_y = 10;
			}
			else
			{
				float rand_y = Random.Range(-1.5f, 6);
				clone = Instantiate(cube, new Vector3(-15, rand_y, 0),
					Quaternion.identity);
				clone.GetComponent<Rigidbody>().useGravity = false;
				clone.GetComponent<Rigidbody>().velocity = new Vector3(4.0f, 0.0f, 0.0f);
				globalGameInfo.Dir_e3 = 0; // Notify x axis spawn
				globalGameInfo.Sp_e3_x = -15;
				globalGameInfo.Sp_e3_y = rand_y;
			}

			print("Sending enemy3");

			//InvokeRepeating("SpawnBullet", 2.0f, 2.5f);
			Invoke("SpawnBullet", 2.5f);
       
			globalGameInfo.Sp_e3 = 1; // notify server of a spawn
		}
	}

	void Update()
	{
		/*print("clone " + clone.transform.position.y);
		if (clone.transform.position.y <= MIN_DEPTH){
			Destroy(clone);
		}*/
		GameObject[] instances = GameObject.FindGameObjectsWithTag("Enemy3");
		for (int i = 0; i < instances.Length; i++)
		{
			if (instances[i].transform.position.y <= MIN_DEPTH || instances[i].transform.position.x >= 15.0f)
			{
				Destroy(instances[i]);
				break;
			}
		}

		GameObject[] bullets = GameObject.FindGameObjectsWithTag("Enemy3Bullet");
		for (int i = 0; i < bullets.Length; i++)
		{
			if (bullets[i].transform.position.y <= MIN_DEPTH || bullets[i].transform.position.x >= 15.0f)
			{
				Destroy(bullets[i]);
				break;
			}
		}
	}

}