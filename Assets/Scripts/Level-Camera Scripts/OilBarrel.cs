using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilBarrel : MonoBehaviour
{
    public float explosionRadius = 2f;
    public int damage = 5;

    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject);
    }

    GameObject Contains(Collider2D[] colliders, string tag)
    {
        foreach(Collider2D collider in colliders)
        {
            if(collider.tag == tag)
            {
                return collider.gameObject;
            }
        }
        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerFireBullet")
        {
            Destroy(this.gameObject);
            //Damage player if they are near the explosion
        }
    }

    private void OnDestroy()
    {
        Vector2 explosionPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

        GameObject playerCheck = Contains(Physics2D.OverlapCircleAll(gameObject.transform.position, explosionRadius), "Player");

        if (playerCheck != null)
        {
            Debug.Log("TEST");
            playerCheck.GetComponent<Character>().TakeDamage(damage);
        }

        //Dont damage player if they are near an oil barrel going into the oven
        ExplosionManager.Instance.SelectExplosionRandom(explosionPos, -90);
    }
}
