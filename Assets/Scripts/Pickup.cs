using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    
    private UICode UI;
    private GameObject cameraRef;
    private GameObject playerRef;
    private PlayerMovement player;
    [SerializeField] private int healthIncrease;
    public SwitchWeapons switchWeapons;
    // Start is called before the first frame update
    void Start()
    {
        cameraRef = GameObject.Find("Main Camera");
        playerRef = GameObject.Find("Player");
        UI = cameraRef.GetComponent<UICode>();
        player = playerRef.GetComponent<PlayerMovement>();
        
        switchWeapons = playerRef.GetComponent<SwitchWeapons>();
    }

    // Update is called once per frame
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(gameObject.GetComponent<Tutorial>() != null)
            {
                return;
            }

            if (gameObject.tag == "HealthPickup")
            {
                playerRef.GetComponent<Character>().HealPlayer(healthIncrease);
                Destroy(gameObject);
            }
            //for any other pickups follow the same if statement above with the proper tags

            if (gameObject.tag == "Level1 Key")
            {
                //player gets key
                player.level1Key = true;
                Destroy(gameObject);
            }

            if (gameObject.tag == "Level2 Key")
            {
                //player gets key
                player.level2Key = true;
                Destroy(gameObject);
            }

            if (gameObject.tag == "Shotgun")
            {
                
                Destroy(gameObject);
                switchWeapons.Shotgun = true;
            }

            if (gameObject.tag == "Flamethrower")
            {

                Destroy(gameObject);
                switchWeapons.Flamethrower = true;
            }
        }
    }

}
