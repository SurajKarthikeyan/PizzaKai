using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDropScript : MonoBehaviour
{
    public bool spikes;

    public GameObject spikesGO;

    public List<GameObject> drops;

    private Vector3 boxLocation;

    private void Start()
    {
        boxLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        print(boxLocation);
    }
    private void OnDestroy()
    {
        // if box marked for spikes, will only and always drop spikes
        if(spikes)
        {
            Instantiate(spikesGO, boxLocation, Quaternion.identity);
            return;
        }

        // for the future if there are other drops such as powerups or something

        //used to simulate the chance for a box to drop something
        if (Random.Range(0, 100) <= 30)
        {
            int dropChoice = Random.Range(0, drops.Count);
            Instantiate(drops[dropChoice], boxLocation, Quaternion.identity);
            
            
        }

    }

}
