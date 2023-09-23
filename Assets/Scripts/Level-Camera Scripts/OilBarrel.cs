using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilBarrel : MonoBehaviour
{
    public float explosionRadius = 2f;

    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerFireBullet")
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        Vector2 explosionPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        Physics2D.OverlapCircle(gameObject.transform.position, explosionRadius);
        Instantiate(ExplosionManager.explosionManager.SelectExplosionRandom(explosionPos, -90));
    }
}
