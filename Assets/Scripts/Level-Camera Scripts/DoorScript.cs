using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                if (player.level1Key == true)
                {
                    Destroy(gameObject);
                }
            }

            if (gameObject.tag == "Level 2 Door")
            {
                if (player.level2Key == true)
                {
                    Destroy(gameObject);
                }
            }

        }
    }
}
