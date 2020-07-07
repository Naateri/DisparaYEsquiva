using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalGameInfo
{

	// sp: spawn
	// e1: enemy 1
	// e2: enemy 2
	// 0 -> can spawn
	// 1 -> has spawned, clear
	// dir: direction
	// 0: x axis
	// 1: y axis

	private static int sp_e1 = 0;
	private static int sp_e2 = 0;

	public static int Sp_e1
	{
		get { return sp_e1; }
		set { sp_e1 = value; }
	}
	public static int Sp_e2
	{
		get { return sp_e2; }
		set { sp_e2 = value; }
	}

	// sp: spawn
	// e1: enemy 1
	// x: position_x
	private static float sp_e1_x = 0.0f;
	private static float sp_e2_x = 0.0f;

	public static float Sp_e1_x
	{
		get { return sp_e1_x; }
		set { sp_e1_x = value; }
	}

	public static float Sp_e2_x
	{
		get { return sp_e2_x; }
		set { sp_e2_x = value; }
	}


	// sp: spawn
	// e1: enemy 1
	// y: position_y
	private static float sp_e1_y = 0.0f;
	private static float sp_e2_y = 0.0f;

	public static float Sp_e1_y
	{
		get { return sp_e1_y; }
		set { sp_e1_y = value; }
	}

	public static float Sp_e2_y
	{
		get { return sp_e2_y; }
		set { sp_e2_y = value; }
	}

	// dir: direction
	// e1: enemy1
	// e2: enemy2

	private static int dir_e1 = 0;
	private static int dir_e2 = 0;

	public static int Dir_e1
    {
		get { return dir_e1; }
		set { dir_e1 = value; }
    }

	public static int Dir_e2
    {
		get { return dir_e2; }
		set { dir_e2 = value; }
    }

	// sp: spawn
	// b: bullet

	private static int sp_b = 0;
	// 0 -> no shot
	// 1 -> cur player has shot, must update other player

	public static int Sp_b
    {
		get { return sp_b; }
        set { sp_b = value; }
    }

}
