using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeapons : MonoBehaviour
{
    public GameObject gun1;
    public GameObject gun2;
    public GameObject gun3;

    public bool notReloading;

    public bool Tommy;
    public bool Shotgun;
    public bool Flamethrower;

    public UICode UIScript;
    

    // Start is called before the first frame update
    void Start()
    {
        Shotgun = false;
        Flamethrower = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIScript.isPaused && !UIScript.isDying)
        {
            if (FindObjectOfType<Shooting>().reloading == true)
            {
                notReloading = false;
            }
            if (FindObjectOfType<Shooting>().reloading == false)
            {
                notReloading = true;
            }

            if (Input.GetKeyDown("" + 1) && notReloading == true && Tommy == true && UIScript.isFlashing == false)
            {
                gun2.SetActive(false);
                gun3.SetActive(false);
                gun1.SetActive(true);
            }

            if (Input.GetKeyDown("" + 2) && notReloading == true && Shotgun == true && UIScript.isFlashing == false)
            {
                gun1.SetActive(false);
                gun3.SetActive(false);
                gun2.SetActive(true);
            }


            if (Input.GetKeyDown("" + 3) && notReloading == true && Flamethrower == true && UIScript.isFlashing == false)
            {
                gun1.SetActive(false);
                gun2.SetActive(false);
                gun3.SetActive(true);
            }
        }
        


    }
}
