using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFireRateUpgrade : MonoBehaviour
{
    public GunScriptableObject gunSO;
    public int upgradePercentage = 10;
    private float realUpgradePercentage;
    

    void UpgradeFireRate()
    {
        realUpgradePercentage = (float)upgradePercentage / 100f;
        gunSO.timeBetweenFiring *= (1 - realUpgradePercentage);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            UpgradeFireRate();
            Destroy(gameObject);
        }
    }
}
