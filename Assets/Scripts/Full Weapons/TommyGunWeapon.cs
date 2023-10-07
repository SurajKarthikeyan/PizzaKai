using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;

/// <summary>
/// Represents the Tommy Gun, child class of WeaponModule
/// 
/// Primarily used for player character
/// 
/// <br/>
/// 
/// Authors: Zane O'Dell (2023)
/// </summary>
public class TommyGunWeapon : WeaponModule
{
    #region Variables
    [Header("Alt Fire Settings")]
    [Tooltip("Point from which tommy flash starts")]
    public Transform playerTransform;

    [Tooltip("Damage done by the tommy alt flash")]
    public int altFireDamage = 20;

    [Tooltip("Tommy flash UI image")]
    public UnityEngine.UI.Image tommyFlashImage;

    [Tooltip("Enemy layer mask")]
    [SerializeField]
    [ReadOnly]
    private int enemyLayerNum = 10;
    private int layerMask;

    private List<Collider2D> visibleEnemies = new();
    #endregion

    #region Init
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        weaponName = WeaponAudioStrings.TommyName;
        //Sets the image for the tommy flash to be clear

        //Left shifts the layer num  to represent layer number by a single bit in the 32-bit integer
        layerMask = 1 << enemyLayerNum;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Overrides the alt fire function of weapon module - Tommy Flash
    /// </summary>
    override public void AltFire()
    {
        base.AltFire();

        // Probably fix the camera shake, but leave it here for now.
        //StartCoroutine(cameraScript.Shake());
        StartCoroutine(tommyAltFlash());

        FindEnemies();

        //TODO: GetComponent is expensive, maybe look into optimizing this. 
        foreach (Collider2D enemy in visibleEnemies)
        {
            enemy.GetComponent<EnemyBasic>().currentHP -= altFireDamage;
        }
    }

    /// <summary>
    /// Finds all colliders on the enemy layer within a certain range
    /// </summary>
    public void FindEnemies()
    {
        visibleEnemies.Clear();

        /**
         * Gets all colliders in range that are on the enemy layer. 10 is a testing value, will need to change later. 
         * Also do we want to use overlap rectangle? Overlap circle is better performance-wise but screen is rectangular. 
         * Using circle will mean we will hit enemies off-screen, which poses a design problem. 
         */
        visibleEnemies = Physics2D.OverlapCircleAll(playerTransform.position, 10, layerMask).ToList();
    }

    /// <summary>
    /// Flashes the tommy alt image on the screen to replicate the flash bang effect
    /// </summary>
    /// <returns>WaitForSeconds in between the various alpha values</returns>
    public IEnumerator tommyAltFlash()
    {
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
    }
    #endregion
}
