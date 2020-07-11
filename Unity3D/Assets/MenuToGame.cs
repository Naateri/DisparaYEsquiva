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

	private static int score2 = 0;
	public static int Score2
	{
		get
		{
			return score2;
		}
		set
		{
			score2 = value;
		}
	}
	private static int game_mode = 0;
	//0 = regular
	//1 = training

	public static int Game_mode{
		get{
			return game_mode;
		}
		set {
			game_mode = value;
		}
	}

	private static int alive = 1;

	public static int Alive{
		get{
			return alive;
		}
		set {
			alive = value;
		}
	}

	// 0 -> can't activate power
	// 1 -> can activate power
	// 2 -> power is active

	private static int power_status = 1;

	public static int Power_status{
		get{
			return power_status;
		}
		set {
			power_status = value;
		}
	}

	// 0 -> DO NOT create extra socket
	// 1 -> Can create socket for menu interaction

	private static int menu_cellphone = 1;

	public static int Menu_Cellphone{
		get {
			return menu_cellphone;
		}
		set {
			menu_cellphone = value;
		}
	}

}
