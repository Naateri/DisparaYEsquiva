using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuToGame
{
	private static int difficulty = 1; //1 = easy, 2 = hard
    // Start is called before the first frame update

	public static int Difficulty{
		get {
			return difficulty;
		}
		set {
			difficulty = value;
		}
	}

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
