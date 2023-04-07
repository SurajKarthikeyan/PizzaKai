using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TommyGun : Shooting
{
    public bool canAlt = true;
    public float timeTillAlt;
    public float timestamp;
    public List<SpriteRenderer> visibleEnemies = new List<SpriteRenderer>();

    public AudioSource GunAltSound;
    public AudioClip GunAlt;
    public UICode UI;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        
    }
    
    // Update is called once per frame
    override public void Update()
    {

        if (!UI.isPaused && !UI.isDying)
        {
            FindEnemies();
            if (!canAlt)
            {
                timestamp += Time.deltaTime;
                if (timestamp > timeTillAlt)
                {
                    canAlt = true;
                    timestamp = 0;
                }
            }

            base.Update();

            if (Input.GetMouseButton(1) && canAlt == true)
            {
                GunAltSound.Play();
                canAlt = false;
                StartCoroutine(cameraScript.Shake());
                StartCoroutine(UI.tommyAltFlash());


                if (Input.GetMouseButton(0) && canFire)
                {

                    Debug.Log("Playing Sound");
                }

                for (int i = 0; i < visibleEnemies.Count; i++)
                {
                    if (visibleEnemies[i].GetComponent<Breadstick>() != null)
                    {
                        visibleEnemies[i].GetComponent<Breadstick>().currentHP = visibleEnemies[i].GetComponent<Breadstick>().currentHP - 5;
                    }
                    if (visibleEnemies[i].GetComponent<ChickenWing>() != null)
                    {
                        visibleEnemies[i].GetComponent<ChickenWing>().currentHP = visibleEnemies[i].GetComponent<ChickenWing>().currentHP - 5;
                    }
                    if (visibleEnemies[i].GetComponent<DeepDish>() != null)
                    {
                        visibleEnemies[i].GetComponent<DeepDish>().currentHP = visibleEnemies[i].GetComponent<DeepDish>().currentHP - 5;
                    }
                }



            }
        }
        
    }
    //Find and store all sprite renders on screen and adds them to a list.
    public void FindEnemies()
    {//retrieve all renderers in the scene
        SpriteRenderer[] sceneObjects = FindObjectsOfType<SpriteRenderer>();

        //Store only visible renderers that have the EnemyBasic script
        visibleEnemies.Clear();
        for (int i = 0; i < sceneObjects.Length; i++)
        {
            if ((sceneObjects[i]).isVisible && (sceneObjects[i]).GetComponent<EnemyBasic>())
            {
                visibleEnemies.Add(sceneObjects[i]);
            }
        }

    }

    //checks whether object is within camera frustrum basically it's line of sight
    bool IsVisible(Renderer renderer)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return (GeometryUtility.TestPlanesAABB(planes, renderer.bounds)) ? true : false;
    }
}
