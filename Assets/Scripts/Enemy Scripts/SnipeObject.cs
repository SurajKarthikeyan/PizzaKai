using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipeObject : MonoBehaviour
{
    public GameObject player;


    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + Vector3.up*0.75f;
    }

    public void StartSnipe(float time, GameObject follow)
    {
        player = follow;
        Invoke(nameof(Fire), time);
    }

    public void Fire()
    {
        //Check if player is behind cover and then kill if not.

        //Destroy self
        Destroy(this.gameObject);
    }
}
