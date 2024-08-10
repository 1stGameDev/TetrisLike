using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayQuit : MonoBehaviour
{

    
    //Function to enter the game
    public void Entergame()
    {
        SceneManager.LoadScene("Game");
    }
//Function to quit the game
    public void Quitgame()
    {
        Debug.Log("Quit");
        Application.Quit();
        Debug.Log("Afterquit");
    }
}
