using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayQuit : MonoBehaviour
{

    public void Entergame()
    {
 
   SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void Quitgame()
    {
        Application.Quit();
    }
}
