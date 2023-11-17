using UnityEngine;

public class GunDamageUpgrade : MonoBehaviour
{
    //public GunScriptableObject gunSO;
    public int upgradePercentage = 10;
    private float realUpgradePercentage;
    private GameObject gunSOBullet;
    // Start is called before the first frame update
    

    void UpgradeDamage()
    {
        realUpgradePercentage = (float)upgradePercentage / 100f;

        if (gunSOBullet.HasComponent(out DamagingWeaponSpawn damaging))
        {
            damaging.damageModifier.Add(
                new PriorityKey<int>(this, "gun_upgrade"),
                new Modifier(0, realUpgradePercentage)
            );
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
