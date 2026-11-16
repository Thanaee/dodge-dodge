using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Game";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        #if UNITY_WEBGL
        Debug.Log("Im Browser Tab schliessen.");
        #else
        Application.Quit();
        #endif
        
    }

}
