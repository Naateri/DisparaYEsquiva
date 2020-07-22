using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;
using static globalGameInfo;
using static MenuToGame;
using System.Runtime.InteropServices;

public class info : MonoBehaviour
{
    public TextMeshProUGUI playerScore; 
    public TextMeshProUGUI playerEnemies;
    public TextMeshProUGUI playerBullets;
    public static bool player1 = false ;
    public static bool player2 = false;

    public static bool Player1
    {
        get
        {
            return player1;
        }
        set
        {
            player1 = value;
        }
    }

    public static bool Player2
    {
        get
        {
            return player2;
        }
        set
        {
            player2 = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        int score = 0; 
        if (player1)
        {
            score = MenuToGame.Score;
            playerScore.text = "Puntaje1 : " + score  + " " + globalGameInfo.shots_hit + " " + globalGameInfo.shots_done;
        }
        else if (player2)
        {
            score = MenuToGame.Score2;
            playerScore.text = "Puntaje2 : " + score  + " " + globalGameInfo.shots_hit + " " + globalGameInfo.shots_done;
        }
        float done = globalGameInfo.shots_done; 
        float precision = (globalGameInfo.shots_hit / done ) * 100;

        print( precision);
        playerEnemies.text = "Enemigos eliminados : " + globalGameInfo.enemies_destroyed;

        playerBullets.text = "Precisión de disparos : " + precision.ToString() + " %";

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
