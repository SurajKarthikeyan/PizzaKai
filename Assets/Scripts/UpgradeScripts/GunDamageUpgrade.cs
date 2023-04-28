using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDamageUpgrade : MonoBehaviour
{
    public GunScriptableObject gunSO;
    public int upgradePercentage = 10;
    private float realUpgradePercentage;
    private GameObject gunSOBullet;
    // Start is called before the first frame update
    

    void UpgradeDamage()
    {
        realUpgradePercentage = (float)upgradePercentage / 100f;
        gunSO.damageMultiplier += realUpgradePercentage;
        
        if (gunSOBullet.GetComponent<bulletScript>() != null)
        {
            gunSOBullet.GetComponent<bulletScript>().damage *= gunSO.damageMultiplier;
        }
        else if (gunSOBullet.GetComponent<Shotgunbullet>() != null)
        {
            gunSOBullet.GetComponent<Shotgunbullet>().damage *= gunSO.damageMultiplier;
        }
        else if (gunSOBullet.GetComponent<FlameShot>() != null)
        {
            gunSOBullet.GetComponent<FlameShot>().damage *= gunSO.damageMultiplier;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            UpgradeDamage();
            Destroy(gameObject);
        }
    }
}
