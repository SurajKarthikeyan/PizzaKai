using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneManager : MonoBehaviour
{
    public void LoadFirstScene()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
