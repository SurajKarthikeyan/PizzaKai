using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionExplode : MonoBehaviour
{

    ChickenMolotov molotov;
    // Start is called before the first frame update
    void Start()
    {
        molotov = GetComponentInParent<ChickenMolotov>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Player"))
        {
            molotov.Explode(true);
        }
        else if (
            collision.collider.CompareTag("PlayerBullet") || collision.collider.CompareTag("PlayerFireBullet"))
        {
            molotov.Explode(false);
        }
    }
}
