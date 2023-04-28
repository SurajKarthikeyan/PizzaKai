using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (gameObject.tag == "Level 1 Door")
            {
                SceneManager.LoadScene("Level2");
            }

            if (gameObject.tag == "Level 2 Door")
            {
                SceneManager.LoadScene("MainMenu");
            }

        }
    }
}
