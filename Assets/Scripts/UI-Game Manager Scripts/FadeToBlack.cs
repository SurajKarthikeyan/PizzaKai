using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeToBlack : MonoBehaviour
{
    [Tooltip("This should only be the Blackness gameobject in the UI Canvas")]
    public Image blackness;
    bool contact;
    public bool fadeIn;
    bool fadeDone;
    public string nextScene;

    private void Start()
    {
        if (fadeIn)
        {
            blackness.color = new Color(0, 0, 0, 1f);
        }
    }

    private void Update()
    {
        if (contact == true && !fadeDone)
        {
            if (fadeIn)
                FadingIn(nextScene);

            else if (!fadeIn)
                FadingOut();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            contact = true;
    }

    public void FadingIn(string sceneToLoad)
    {
        blackness.color += new Color(0, 0, 0, Time.deltaTime);
        if (blackness.color.a >= 1)
        {
            SceneManager.LoadScene(sceneToLoad);
            fadeDone = true;
        }
    }
    public void FadingOut()
    {
        blackness.color -= new Color(0, 0, 0, Time.deltaTime / 4);
        if (blackness.color.a <= 0)
        {
            fadeDone = true;
        }
    }
}
