using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public UIManager UIScript;
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
        GameObject PauseMenu = GameObject.FindGameObjectWithTag("Pause Menu");
        UIScript.isPaused = false;
        PauseMenu.SetActive(false);
    }
}
