using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public List<string> tutorials;
    public bool canSkip;
    public int firstImportant;

    public GameObject textBox;
    public TMP_Text tutText;
    private int currentTutorial;
    private bool tutActive = false;

    public KeyCode key;
    public KeyCode alwaysKey = KeyCode.D;
    public KeyCode skipKey = KeyCode.S;

    public GameObject enemyRestrict;
    public bool isSpawned;

    void OnTriggerStay2D(Collider2D other)
    {
        if (enemyRestrict != null) return;

        if (!other.CompareTag("Player")) return;
        if (tutActive) return;

        tutActive = true;
        textBox.SetActive(true);
        Time.timeScale = 0;
        currentTutorial = 0;
        tutText.text = tutorials[currentTutorial];
    }

    void Update()
    {
        if (isSpawned)
        {
            textBox = GameObject.Find("TutorialPanel");
            GameObject tutTextGO = GameObject.Find("TutorialText");
            tutText = tutTextGO.GetComponent<TMP_Text>();
            if (textBox != null && tutText != null) isSpawned = false;
        }

        if (!tutActive) return;

        if (tutorials.Count > (currentTutorial + 1) && Input.GetKeyDown(skipKey))
        {
            if (canSkip)
            {
                currentTutorial = firstImportant;
                tutText.text = tutorials[currentTutorial];
                canSkip = false;
                return;
            }
        }

        if (tutorials.Count > (currentTutorial + 1) &&
            (Input.GetKeyDown(key) || Input.GetKeyDown(alwaysKey)))
        {
            currentTutorial++;
            tutText.text = tutorials[currentTutorial];
        }
        else if (tutorials.Count == (currentTutorial + 1) &&
            (Input.GetKeyDown(key) || Input.GetKeyDown(alwaysKey)
            || Input.GetKeyDown(skipKey)))
        {
            textBox.SetActive(false);
            Time.timeScale = 1;
            Destroy(this);
        }

    }
}
