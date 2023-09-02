using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.UIElements;

/// <summary>
/// Represents the Tommy Gun, child class of WeaponModule
/// 
/// Primarily used for player character
/// </summary>
public class TommyGunWeapon : WeaponModule
{
    public Transform playerTransform;

    public int altFireDamage = 5;

    public UnityEngine.UI.Image tommyFlashImage;

    private bool isFlashing;

    private List<Collider2D> visibleEnemies = new List<Collider2D>();

    public CameraFollow cameraScript;

    private int enemyLayerNum = 10;
    private int layerMask;

    private void Start()
    {
        base.Start();
        //Sets the image for the tommy flash to be clear
        tommyFlashImage.color = new Color(1, 1, 1, 0);

        //Left shifts the layer num  to represent layer number by a single bit in the 32-bit integer
        layerMask = 1 << enemyLayerNum;
    }

    /// <summary>
    /// Overrides the alt fire function of weapon module - Tommy Flash
    /// </summary>
    override public void AltFire()
    {
        base.AltFire();
        Debug.Log("TommyGunAlt");

        //Play the proper sound for the gun alt, handled by singleton audio dict
        //Have a boolean and respective logic saying whether or not you can alt, set bool to false here

        // Probably fix the camera shake, but leave it here for now.
        StartCoroutine(cameraScript.Shake());
        StartCoroutine(tommyAltFlash());

        FindEnemies();


        //TODO: GetComponent is expensive, maybe look into optimizing this. 
        foreach (Collider2D enemy in visibleEnemies)
        {
            enemy.GetComponent<EnemyBasic>().currentHP -= altFireDamage;
        }
    }

    //Find and store all sprite renders on screen and adds them to a list.
    public void FindEnemies()
    {
        //retrieve all visible enemies in a radius from player
        visibleEnemies.Clear();

        /**
         * Gets all colliders in range that are on the enemy layer. 10 is a testing value, will need to change later. 
         * Also do we want to use overlap rectangle? Overlap circle is better performance-wise but screen is rectangular. 
         * Using circle will mean we will hit enemies off-screen, which poses a design problem. 
         */
        visibleEnemies = Physics2D.OverlapCircleAll(playerTransform.position, 10, layerMask).ToList();

        Debug.Log(visibleEnemies.Count);

    }

    public IEnumerator tommyAltFlash()
    {
        //isFlashing = true;
        
        tommyFlashImage.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.2f);
        tommyFlashImage.color = new Color(1, 1, 1, 0.8f);
        yield return new WaitForSeconds(0.2f);
        tommyFlashImage.color = new Color(1, 1, 1, 0.6f);
        yield return new WaitForSeconds(0.2f);
        tommyFlashImage.color = new Color(1, 1, 1, 0.4f);
        yield return new WaitForSeconds(0.2f);
        tommyFlashImage.color = new Color(1, 1, 1, 0.2f);
        yield return new WaitForSeconds(0.2f);
        tommyFlashImage.color = new Color(1, 1, 1, 0);
        //isFlashing = false;
    }
}
