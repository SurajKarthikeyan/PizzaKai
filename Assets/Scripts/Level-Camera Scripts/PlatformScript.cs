using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    Collider2D playerCol;

    //a really scuff way to not repeatedly call the coroutine
    bool waitNow = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerCol = collision.collider;
        }
    }

    //this is going to need to be changed to allow AI to drop through as well.
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetKey(KeyCode.S) && !waitNow)
        {
            StartCoroutine("OffOnCollider");
            waitNow = true;
        }
       
    }

    private IEnumerator OffOnCollider()
    {
        playerCol.enabled = false;

        yield return new WaitForSeconds(0.2f);

        playerCol.enabled = true;
        waitNow = false;
    }
}
