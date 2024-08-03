using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    //Lets me refer to the panel in this script
    public GameObject PausePanel;

    public static bool GameIsPaused;
   
   //When the game starts the canvas is not active, meaning it is invisible
   void Start()
   {
     PausePanel.SetActive(false);
     GameIsPaused = false;
     Time.timeScale = 1f;
   }
void Update()
{

if (Input.GetKeyDown(KeyCode.Escape))
{
    if (GameIsPaused)
    {
        Resume();
    } else
    {
        Pause();
    }
}
}


    

public void Pause()
{
    PausePanel.SetActive(true);
    GameIsPaused = true;
    Time.timeScale = 0f;
}

public void Resume()
{
    PausePanel.SetActive(false);
    GameIsPaused = false;
    Time.timeScale = 1f;
}

public void gotoMainMenu()
{
SceneManager.LoadScene(0);
}

}
