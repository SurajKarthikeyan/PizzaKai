using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Shooting
{
    public bool canAlt = true;
    public float timeTillAlt;
    public float timestamp;
    public GameObject fireBall;

    public AudioSource GunAltSound;
    public AudioClip GunAlt;

    
    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    override public void Update()
    {

        if (!UIScript.isPaused && !UIScript.isDying)
        {
            if (!canAlt)
            {
                timestamp += Time.deltaTime;
                if (timestamp > timeTillAlt)
                {
                    canAlt = true;
                    timestamp = 0;
                }
            }
            base.Update();

            if (Input.GetMouseButton(1) && canAlt == true)
            {
                GunAltSound.Play();
                canAlt = false;
                Instantiate(fireBall, bulletTransform, Quaternion.identity);
            }
        }
        
    }
}
