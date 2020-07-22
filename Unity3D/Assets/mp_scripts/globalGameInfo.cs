//using NUnit.Framework.Internal.Filters;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class globalGameInfo
{

	// sp: spawn
	// e1: enemy 1
	// e2: enemy 2
	// e3: enemy 3
	// 0 -> can spawn
	// 1 -> has spawned, clear
	// dir: direction
	// 0: x axis
	// 1: y axis

	private static int sp_e1 = 0;
	private static int sp_e2 = 0;
	private static int sp_e3 = 0;
	private static int sp_e4 = 0;

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
	public static int Sp_e3
	{
		get { return sp_e3; }
		set { sp_e3 = value; }
	}
	public static int Sp_e4
	{
		get { return sp_e4; }
		set { sp_e4 = value; }
	}
	// sp: spawn
	// e1: enemy 1
	// x: position_x
	private static float sp_e1_x = 0.0f;
	private static float sp_e2_x = 0.0f;
	private static float sp_e3_x = 0.0f;
	private static float sp_e4_x = 0.0f;

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

	public static float Sp_e3_x
	{
		get { return sp_e3_x; }
		set { sp_e3_x = value; }
	}

	public static float Sp_e4_x
	{
		get { return sp_e4_x; }
		set { sp_e4_x = value; }
	}

	// sp: spawn
	// e1: enemy 1
	// y: position_y
	private static float sp_e1_y = 0.0f;
	private static float sp_e2_y = 0.0f;
	private static float sp_e3_y = 0.0f;
	private static float sp_e4_y = 0.0f;

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

	public static float Sp_e3_y
	{
		get { return sp_e3_y; }
		set { sp_e3_y = value; }
	}

	public static float Sp_e4_y
	{
		get { return sp_e4_y; }
		set { sp_e4_y = value; }
	}

	// dir: direction
	// e1: enemy1
	// e2: enemy2

	private static int dir_e1 = 0;
	private static int dir_e2 = 0;
	private static int dir_e3 = 0;
	private static int dir_e4 = 0;

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

	public static int Dir_e3
	{
		get { return dir_e3; }
		set { dir_e3 = value; }
	}

	public static int Dir_e4
	{
		get { return dir_e4; }
		set { dir_e4 = value; }
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

	// Enemy3Bullet

	private static int e3_b = 0;
	// 0 -> no spawn, can spawn
	// 1 -> has spawned on server, spawn on client
	private static float e3_bx = 0.0f; // enemy3 bullet position x
	private static float e3_by = 0.0f; // enemy3 bullet position y

	public static int E3_b
	{
		get { return e3_b; }
		set { e3_b = value; }
	}

	public static float E3_bx
	{
		get { return e3_bx; }
		set { e3_bx = value; }
	}

	public static float E3_by
	{
		get { return e3_by; }
		set { e3_by = value; }
	}


	// Start new levels

	private static int level_2 = 0;
	private static int level_3 = 0;

	public static int Level_2
	{
		get { return level_2; }
		set { level_2 = value; }
	}

	public static int Level_3
    {
		get { return level_3; }
		set { level_3 = value; }
    }

	// Extra life

	private static int add_life = 0;
	public static int Add_life
    {
		get { return add_life; }
		set { add_life = value; }
	}

	private static int send_life = 0;

	public static int Send_life
	{
		get { return send_life; }
		set { send_life = value; }
    }

	private static int looses_life = 0;

	public static int Looses_life
    {
        get { return looses_life; }
		set { looses_life = value; }
    }

	// Lives counter

	private static int player1_lives = 10;
	private static int player2_lives = 10;

	public static int Player1_lives
    {
		get { return player1_lives; }
		set { player1_lives = value; }
    }

	public static int Player2_lives
    {
		get { return player2_lives; }
		set { player2_lives = value; }
    }

	// DATA to display after ending the game

	public static int shots_done = 0;

	public static int Shots_done
    {
		get { return shots_done; }
		set { shots_done = value; }
    }

	public static int shots_hit = 0;

	public static int Shots_hit
    {
		get { return shots_hit; }
		set { shots_hit = value; }
    }

	public static int enemies_destroyed = 0;

	public static int Enemies_Destroyed
    {
		get { return enemies_destroyed; }
		set { enemies_destroyed = value; }
    }

}
