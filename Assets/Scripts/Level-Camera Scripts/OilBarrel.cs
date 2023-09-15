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
        Instantiate(ExplosionManager.SelectExplosion(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)));
    }
}
