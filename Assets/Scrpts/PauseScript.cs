using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    //Lets me refer to the panel in this script
    public GameObject PausePanel;

    private static bool GameIsPaused;

    private static bool canPause;
   
   //When the game starts the canvas is not active, meaning it is invisible
   void Start()
   {
     PausePanel.SetActive(false);
     GameIsPaused = false;
     Time.timeScale = 1f;
     canPause = true;
   }
void Update()
{
if (Input.GetKey(KeyCode.Escape) )
{
    //If esc is pressed it checks if it has paused (or unpaused) with the same key-press
    //If it hasn't --> it pauses. If it has--> does nothing.
    if(canPause == true)
    {
        if (GameIsPaused)
        {
            Resume();
        } else
        {
            Pause();
        }
    }
    canPause = false;
}
else{
    //If esc is not pressed, you can pause again
    canPause = true;
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
