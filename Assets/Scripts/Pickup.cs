using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    private GameObject cameraRef;
    private GameObject playerRef;
    private PlayerMovement player;

    public GameObject shotgun;

    public GameObject flamethrower;

    public GameObject weaponMaster;

    [SerializeField] private int healthIncrease;
    // Start is called before the first frame update
    void Start()
    {
        cameraRef = GameObject.Find("Main Camera");
        playerRef = GameObject.Find("Player");
        player = playerRef.GetComponent<PlayerMovement>();
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
                player.level1Key = true;
                Destroy(gameObject);
            }

            if (gameObject.tag == "Level2 Key")
            {
                //player gets key
                player.level2Key = true;
                Destroy(gameObject);
            }

            if (gameObject.tag == "Shotgun")
            {

                Destroy(gameObject);
                shotgun.transform.parent = weaponMaster.transform;
                shotgun.transform.position = weaponMaster.transform.position + new Vector3(-0.122f, 0.216f, 0);
                shotgun.SetActive(false);
                weaponMaster.GetComponent<WeaponMasterModule>().weapons.Add(shotgun.GetComponent<ShotGunWeapon>());


                if (gameObject.tag == "Flamethrower")
                {

                    Destroy(gameObject);
                    GameObject flameGO = Instantiate(flamethrower);
                    flameGO.transform.parent = weaponMaster.transform;
                }
            }
        }

    }
}
