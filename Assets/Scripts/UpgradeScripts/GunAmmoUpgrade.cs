using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAmmoUpgrade : MonoBehaviour
{

    public GunScriptableObject gunSO;
    public int ammoUpgrade = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If the player collides with the object, for now it's just the H Key
        if (Input.GetKeyDown(KeyCode.H))
        {
            UpgradeAmmo();
            Destroy(gameObject);
        }
    }

    void UpgradeAmmo()
    {
        gunSO.ammoCountMax += ammoUpgrade;
        gunSO.ammoCountCurrent = gunSO.ammoCountMax;
    }
}
