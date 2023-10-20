using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltFlameProjectile : SimpleProjectile
{
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.HasComponent(out EnemyBasic enemy))
        {
            enemy.TakeDamage(ActualDamage);
        }

        if (collision.HasComponent(out BurningScript burning))
        {
            Destroy(burning.gameObject);
        }
    }

}
