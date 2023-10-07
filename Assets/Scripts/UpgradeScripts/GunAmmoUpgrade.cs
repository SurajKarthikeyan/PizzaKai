using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAmmoUpgrade : MonoBehaviour
{

    //public GunScriptableObject gunSO;
    public int ammoUpgrade = 10;

    void UpgradeAmmo()
    {
        //gunSO.ammoCountMax += ammoUpgrade;
        //gunSO.ammoCountCurrent = gunSO.ammoCountMax;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            UpgradeAmmo();
            Destroy(gameObject);
        }
    }
}
