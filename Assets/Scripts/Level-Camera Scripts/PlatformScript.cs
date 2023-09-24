using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Author Matthew DeBoer

public class PlatformScript : MonoBehaviour
{
    Collider2D playerCol;

    bool onPlatform = false;
    bool wait = false;

    //having this script handle all of the player will make it so you can't spam 'S' to repeatedly call DropDown() even when not on a platform
    //otherwise the script handling player movement would have to check if its on a platform.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerCol = collision.collider;
            onPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            onPlatform = false;
        }
    }


    private void Update()
    {
        //for the player
        if (Input.GetKeyDown(KeyCode.S) && onPlatform && !wait)
        {
            wait = true;
            StartCoroutine(OffOnCollider(playerCol.gameObject));
        }
    }

    //this is what other things (probably enemies) that need to drop thorugh platforms will call
    public void DropDown(GameObject target)
    {
        StartCoroutine(OffOnCollider(target));
    }

    private IEnumerator OffOnCollider(GameObject dropper)
    {
        //hold the original layer index
        int tempHolder = dropper.layer;
        dropper.layer = LayerMask.NameToLayer("OneWayPlatform");

        yield return new WaitForSeconds(0.3f);

        //return to original layer
        dropper.layer = tempHolder;
        wait = false;
    }
}
