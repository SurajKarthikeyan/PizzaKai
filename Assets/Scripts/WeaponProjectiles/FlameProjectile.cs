using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameProjectile : SimpleProjectile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box") || collision.gameObject.CompareTag("Burniing Box"))
        {
            Destroy(gameObject);
        }
    }
}
