using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFireRateUpgrade : MonoBehaviour
{
    public GunScriptableObject gunSO;
    public int upgradePercentage = 10;
    private float realUpgradePercentage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If the player collides with the object, for now it's just the J Key
        if (Input.GetKeyDown(KeyCode.J))
        {
            UpgradeFireRate();
            Destroy(gameObject);
        }
    }

    void UpgradeFireRate()
    {
        realUpgradePercentage = (float)upgradePercentage / 100f;
        gunSO.timeBetweenFiring *= (1 - realUpgradePercentage);
    }
}
