using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mpEnemy2 : MonoBehaviour
{
    private float delay, start_delay;
    private float MIN_DEPTH = -5.5f;
    public GameObject cube, clone;
    // Start is called before the first frame update

    public int player; // 0 -> server, 1 -> client

    void Start()
    {
       // if (MenuToGame.Difficulty == 1)
        //{
            this.delay = 1.8f;
            this.start_delay = 1.5f;
       /* }
        else if (MenuToGame.Difficulty == 2)
        {
            this.delay = 0.9f;
            this.start_delay = 0.75f;
        }
       */
        InvokeRepeating("Spawn", start_delay, delay);
    }

    void Spawn()
    {
        if (globalGameInfo.Sp_e2 == 0 && player == 0) // can spawn
        {
            float rand_x = Random.Range(-6, 6);
            clone = Instantiate(cube, new Vector3(rand_x, 10, 0),
             Quaternion.identity);
            globalGameInfo.Sp_e2 = 1; // notify server of a spawn
            globalGameInfo.Sp_e2_x = rand_x;
            globalGameInfo.Sp_e2_y = 10;
        }
    }
    // Update is called once per frame
    void Update()
    {
        GameObject[] instances = GameObject.FindGameObjectsWithTag("Enemy2");
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
