using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkyOven : MonoBehaviour
{

    [SerializeField] private Generator[] generators;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        //If the player enters the oven, kill the player
        if(collision.gameObject.tag == "Player")
        {
            //There doesnt appear to be a way to get player maxhealth so I just picked a big number
            collision.gameObject.GetComponent<Character>().TakeDamage(999999);
        }
        else
        {
            Debug.Log("TEST");
            if(collision.gameObject.GetComponent<OilBarrel>() != null)
            {
                foreach(Generator generator in generators)
                {
                    if(generator != null)
                    {
                        generator.SetVulnerability();
                    }
                }
            }
            Destroy(collision.gameObject);
        }

    }
}