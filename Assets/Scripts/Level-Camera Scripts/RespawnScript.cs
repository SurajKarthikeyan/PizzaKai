using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{

    public GameObject player;
    public GameObject respawnPoint;

    public UICode UIScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void respawnPlayer ()
    {
        UIScript.isDying = false;
        player.layer = 7;
        player.transform.position = respawnPoint.transform.position;
        
    }
}
