using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using static MenuToGame;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CargarEscena(string nombre)
    {
        if (nombre == "socketTest"){
            MenuToGame.Game_mode = 0;
        } else if (nombre == "menu"){
            MenuToGame.Alive = 0;
        }
        SceneManager.LoadScene(nombre);
    }

    public void SetDifficulty(int num){
    	MenuToGame.Difficulty = num;
    	print("Difficulty = " + num);
    	SceneManager.LoadScene("menu");
    }

    public void SetGameMode(){
        MenuToGame.Game_mode = 1;


        SceneManager.LoadScene("socketTest");
        
    }

    public void ExitGame(){
    	Application.Quit();
    }
}
