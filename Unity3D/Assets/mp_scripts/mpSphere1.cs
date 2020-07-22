using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MenuToGame;

public class mpSphere1 : MonoBehaviour
{

    private int lives = 10;
    private int score = 0;
    AudioSource cube_audio;
    private ParticleSystem particle;
    public Font myFont;
    public GameObject leaveButton;
    private bool restarted = false;

    public int player = 0;

    void Start()
    {
        print("Start Sphere");

        MenuToGame.Alive = 1;
        MenuToGame.Power_status = 1;
        MenuToGame.Menu_Cellphone = 1; // can create menu socket again
    }

    void Update()
    {
        /*if (this.lives <= 0){
			StartCoroutine(BackToMenu());
		}*/

        this.score = MenuToGame.Score;

        if (player == 1)
        {
            this.lives = globalGameInfo.Player1_lives;
        } else if (player == 2)
        {
            this.lives = globalGameInfo.Player2_lives;
        }

        //if (this.lives >= 1)
        if (globalGameInfo.Player1_lives > 1 && globalGameInfo.Player2_lives > 1)
        {
            if (globalGameInfo.Add_life == 1)
            {
                /*if (this.player == 1) // Player1 got the life
                {
                    globalGameInfo.Player1_lives++;
                    globalGameInfo.Player2_lives--;
                } else if (this.player == 2) // Player2 got the life
                {
                    globalGameInfo.Player1_lives--;
                    globalGameInfo.Player2_lives++;
                }*/
                if (globalGameInfo.Looses_life == 1)
                {
                    globalGameInfo.Player1_lives--;
                    globalGameInfo.Player2_lives++;
                } else if (globalGameInfo.Looses_life == 2)
                {
                    globalGameInfo.Player2_lives--;
                    globalGameInfo.Player1_lives++;
                }
                globalGameInfo.Add_life = 0;
                globalGameInfo.Looses_life = 0;
            }
                
        }

        if (globalGameInfo.Add_life == 1)
        {
            globalGameInfo.Add_life = 0;
            globalGameInfo.Looses_life = 0;
        }

        if (globalGameInfo.Player1_lives <= 0 && globalGameInfo.Player2_lives <= 0)
        {
            print("End game");
            MenuToGame.Alive = 0;
            //StartCoroutine(EndGame());
            StartCoroutine(RestartGame());
        }
    }

    private IEnumerator Break(Collision collision)
    {
        //impacto de cubo con el jugador
        collision.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        particle = collision.gameObject.GetComponent<ParticleSystem>();
        particle.Play();

        cube_audio = collision.gameObject.GetComponent<AudioSource>();
        cube_audio.Play();

        collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
        (collision.gameObject.GetComponent(typeof(BoxCollider)) as Collider).enabled = false;
        //collision.gameObject.GetComponent<Rigidbody>().useGravity = false;


        yield return new WaitForSeconds(0.8f);

    }

    private IEnumerator RestartGame()
    {
        //Destroy(gameObject, 2);
        yield return new WaitForSeconds(2.0f);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        (gameObject.GetComponent(typeof(SphereCollider)) as Collider).enabled = false;
        restarted = true;

        /*
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("menu");
        Destroy(gameObject);*/
    }

    private IEnumerator EndGame()
    {
        if (!restarted)
        {
            yield return new WaitForSeconds(1.0f);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            (gameObject.GetComponent(typeof(SphereCollider)) as Collider).enabled = false;
        }
        yield return new WaitForSeconds(1.0f);
        MenuToGame.Alive = 0;
        print("Loading scene");
        SceneManager.LoadScene("stats");
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {

        //only do this if colission is with cube
        //not with bullet

        if (collision.gameObject.tag == "Enemy1")
        {
            this.lives--;
            print("lives: " + this.lives);

            StartCoroutine(Break(collision));
            Destroy(collision.gameObject, 2);
            //yield return new WaitForSeconds(particle.main.StartLifetime.constantMax);

        }
        else if (collision.gameObject.tag == "Enemy2")
        {
            print("Collision with enemy2");
            if (MenuToGame.Game_mode == 0)
                this.lives -= 2;
            print("lives: " + this.lives);
            StartCoroutine(Break(collision));

            Destroy(collision.gameObject, 2);
        }
        else if (collision.gameObject.tag == "Enemy3")
        {
            print("Collision with enemy3");
            if (MenuToGame.Game_mode == 0)
                this.lives -= 3;
            print("lives: " + this.lives);
            StartCoroutine(Break(collision));

            Destroy(collision.gameObject, 2);
        }
        else if (collision.gameObject.tag == "Enemy3Bullet")
        {
            print("Collision with enemy3 bullet");
            if (MenuToGame.Game_mode == 0)
                this.lives -= 1;
            print("lives: " + this.lives);
            Destroy(collision.gameObject, 0);
        }
        else if (collision.gameObject.tag == "Enemy4")
        {
            if (MenuToGame.Game_mode == 0)
            {
                this.lives = 0;
                globalGameInfo.Player1_lives = 0;
                globalGameInfo.Player2_lives = 0;
            }
            Destroy(collision.gameObject, 0);
        }

        print("Player " + player + " lives = " + this.lives);

        if (player == 1)
        {
            globalGameInfo.Player1_lives = this.lives;
        } else if (player == 2)
        {
            globalGameInfo.Player2_lives = this.lives;
        }

        if (this.lives <= 0)
        {
            //MenuToGame.Alive = 0;
            //Destroy(gameObject, 2);
            StartCoroutine(RestartGame());
        }
    }

    void OnGUI()
    {
        if (player == 1)
        {
            Rect rectObj = new Rect(50, 70, 200, 400);

            GUIStyle style = new GUIStyle();

            style.alignment = TextAnchor.UpperLeft;
            style.font = myFont;
            style.fontSize = 35;
            style.normal.textColor = Color.yellow;
            if (this.lives > 0)
            {
                GUI.Box(rectObj, "VIDAS: " + this.lives,
                      style);

            }
            else
            {
                GUI.Box(rectObj, "PERDISTE",
                      style);
            }

            Rect rectObj2 = new Rect(50, 20, 200, 400);

            GUIStyle style2 = new GUIStyle();
            style2.font = myFont;
            style2.fontSize = 40;
            style2.normal.textColor = Color.white;
            style2.alignment = TextAnchor.UpperRight;

            GUI.Box(rectObj2, "SCORE : " + MenuToGame.Score, style2);
        }
        else if (player == 2)
        {
            Rect rectObj = new Rect(900, 70, 200, 400);

            GUIStyle style = new GUIStyle();

            style.alignment = TextAnchor.UpperLeft;
            style.font = myFont;
            style.fontSize = 35;
            style.normal.textColor = Color.yellow;
            if (this.lives > 0)
            {
                GUI.Box(rectObj, "VIDAS: " + this.lives,
                      style);

            }
            else
            {
                GUI.Box(rectObj, "PERDISTE",
                      style);
            }

            Rect rectObj2 = new Rect(900, 20, 200, 400);

            GUIStyle style2 = new GUIStyle();
            style2.font = myFont;
            style2.fontSize = 40;
            style2.normal.textColor = Color.white;
            style2.alignment = TextAnchor.UpperRight;

            GUI.Box(rectObj2, "SCORE : " + MenuToGame.Score2, style2);
        }


        /*
        Rect powerNotif = new Rect(800, 70, 200, 350);
        style.fontSize = 30;
        style.normal.textColor = Color.green;
        if (MenuToGame.Power_status == 0)
        {
            GUI.Box(powerNotif, "PODER NO DISPONIBLE", style);
        }
        else if (MenuToGame.Power_status == 1)
        {
            GUI.Box(powerNotif, "PODER DISPONIBLE", style);
        }
        else if (MenuToGame.Power_status == 2)
        {
            GUI.Box(powerNotif, "PODER ACTIVADO", style);
        }

    */

    }
}
