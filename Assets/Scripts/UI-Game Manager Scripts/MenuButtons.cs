using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public UIManager UIScript;
    public GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButtonPress()
    {
        SceneManager.LoadScene("PeppyFirstSteps");
        Time.timeScale = 1;
    }

    public void QuitButtonPress()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    public void CreditsButtonPress()
    {
        //change canvas to credits
    }

    public void CitationButtonPress()
    {
        //change canvas to citations
    }
    public void MenuButtonPress()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        FindObjectOfType<LevelAnalytics>().PostLevelAnalytics();
    }

    public void ResumeButtonPress()
    {
        //the way this is set rn it may just set the button to false not the whole menu
        Time.timeScale = 1;
        UIScript.isPaused = false;
        pauseMenu.SetActive(false);
    }
}
