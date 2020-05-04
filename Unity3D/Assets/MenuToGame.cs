using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuToGame
{
	private static int difficulty = 1; //1 = easy, 2 = hard

	public static int Difficulty{
		get {
			return difficulty;
		}
		set {
			difficulty = value;
		}
	}

	private static int score = 0;

	public static int Score{
		get {
			return score;
		}
		set {
			score = value;
		}
	}

}
