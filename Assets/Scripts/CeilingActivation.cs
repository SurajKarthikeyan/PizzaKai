using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingActivation : MonoBehaviour
{
    [SerializeField] private RespawnModule respawn; //This is bad code. I dont care.
    [SerializeField] private Transform respawnPoint; //This is bad code. I dont care.

    // Start is called before the first frame update
    void Start()
    {
        Invoke("TurnOnHitbox", 6f);
    }

    public void TurnOnHitbox()
    {
        gameObject.GetComponent<Collider2D>().enabled = true;
        if(respawn != null)
        {
            respawn.lastRespawnPoint = respawnPoint;
        }
        else
        {
            Debug.LogError("YOU FORGOT TO ACCOUNT FOR THE FACT THAT MY CODE IS BAD! ASSIGN THE RESPAWN MODULE MANUALLY LOL GET OWNED!");
        }
    }
}
