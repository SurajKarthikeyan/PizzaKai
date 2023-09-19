using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilBarrel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Vector2 explosionPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        Instantiate(ExplosionManager.SelectExplosion(explosionPos, -90));
    }
}
