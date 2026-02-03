using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
        public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit"); 
    }
        public void StartGame()
    {
        SceneManager.LoadScene("Opening"); 
    }
            public void Guidline()
    {
        SceneManager.LoadScene("Guide"); 
    }
}
