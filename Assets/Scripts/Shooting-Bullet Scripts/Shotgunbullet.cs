using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgunbullet : bulletScript
{
    bool firstProjectile = true;
    List<GameObject> bulletInScene = new List<GameObject>();
    // Start is called before the first frame update
    override public void Start()
    {

        CheckForBullets();
        CloneMe();
        
        base.Start();
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();
    }

    void CloneMe()
    {
        Debug.Log("1");
        if (firstProjectile == true)
        {
            //makes a bunch of extra bullets. If we need more, add more instantiates.
            Debug.Log("2");
            Instantiate(this.gameObject);
            Instantiate(this.gameObject);
            Instantiate(this.gameObject);
            Instantiate(this.gameObject);
            Instantiate(this.gameObject);
            
        }
        Debug.Log("3");
    }

    //checks to see if there are more than 1 bullet
    void CheckForBullets()
    {
        Shotgunbullet[] findList = FindObjectsOfType<Shotgunbullet>();
        

        for (int i = 0; i < findList.Length; i++)
        {
            if (findList.Length > 1) //if the list is more than 1, turn off firstProjectile
            findList[i].firstProjectile = false;
        }

    }
}
