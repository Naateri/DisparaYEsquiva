using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalGameInfo
{

	// sp: spawn
	// e1: enemy 1
	// 0 -> can spawn
	// 1 -> has spawned, clear
	private static int sp_e1 = 0;

	public static int Sp_e1
    {
		get { return sp_e1; }
		set { sp_e1 = value; }
    }

	// sp: spawn
	// e1: enemy 1
	// x: position_x
	private static float sp_e1_x = 0.0f;

	public static float Sp_e1_x
    {
        get { return sp_e1_x; }
		set { sp_e1_x = value; }
    }

	// sp: spawn
	// e1: enemy 1
	// y: position_y
	private static float sp_e1_y = 0.0f;

	public static float Sp_e1_y
	{
		get { return sp_e1_y; }
		set { sp_e1_y = value; }
	}



}
