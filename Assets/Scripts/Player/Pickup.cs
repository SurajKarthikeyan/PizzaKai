using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    private GameObject playerRef;

    public GameObject shotgun;

    public GameObject flamethrower;

    public GameObject weaponMaster;

    [SerializeField] private int healthIncrease;
    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.Find("Player");
    }

    // Update is called once per frame
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (gameObject.GetComponent<Tutorial>() != null)
            {
                return;
            }

            if (gameObject.tag == "HealthPickup")
            {
                playerRef.GetComponent<Character>().HealPlayer(healthIncrease);
                Destroy(gameObject);
            }
            //for any other pickups follow the same if statement above with the proper tags

            if (gameObject.tag == "Level1 Key")
            {
                //player gets key
                Destroy(gameObject);
            }

            if (gameObject.tag == "Level2 Key")
            {
                //player gets key
                Destroy(gameObject);
            }

            if (gameObject.tag == "Shotgun")
            {

                Destroy(gameObject);
                shotgun.transform.parent = weaponMaster.transform;
                shotgun.transform.position = weaponMaster.transform.position + new Vector3(-0.122f, 0.216f, 0);
                WeaponMasterModule weaponMasterScript = weaponMaster.GetComponent<WeaponMasterModule>();
                weaponMasterScript.weapons.Add(shotgun.GetComponent<ShotGunWeapon>());
                weaponMasterScript.weaponIndex = weaponMasterScript.weapons.Count - 1;
                weaponMasterScript.EnableCurrentWeapon();
                shotgun.SetActive(true);
                
            }

            if (gameObject.tag == "Flamethrower")
            {
                Destroy(gameObject);
                flamethrower.transform.parent = weaponMaster.transform;
                flamethrower.transform.position = weaponMaster.transform.position + new Vector3(-0.1029999f, 0.21f, 0);
                WeaponMasterModule weaponMasterScript = weaponMaster.GetComponent<WeaponMasterModule>();
                weaponMasterScript.weapons.Add(flamethrower.GetComponent<FlameThrowerWeapon>());
                weaponMasterScript.weaponIndex = weaponMasterScript.weapons.Count - 1;
                weaponMasterScript.EnableCurrentWeapon();
                flamethrower.SetActive(true);
            }
        }

    }
}
