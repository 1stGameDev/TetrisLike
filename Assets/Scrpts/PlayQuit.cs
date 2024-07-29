using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayQuit : MonoBehaviour
{

    public void Entergame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quitgame()
    {
        Debug.Log("Quit");
        Application.Quit();
        Debug.Log("Afterquit");
    }
}
