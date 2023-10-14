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
public class PistolWeapon : WeaponModule
{
    #region Variables
    [SerializeField] private Camera realCamera;
    [SerializeField] private float grappleDistance;
    [SerializeField] private float grappleForce;
    private Vector2 hitPoint;
    private Rigidbody2D rb;
    private bool isGrappling = false;
    private float grappleMultiplier = 0f;
    [SerializeField] private LineRenderer lineRend;
    #endregion

    #region Init
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        weaponName = WeaponAudioStrings.PistolName;
        //This line below will need to be changed to have it get the player's RB
        rb = Master.r2d;

    }
    #endregion

    #region Methods

    private void FixedUpdate()
    {
        if (isGrappling)
        {
            lineRend.enabled = true;
            rb.velocity = (rb.velocity * 0.5f) + grappleForce * (hitPoint - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).normalized * grappleMultiplier;
            grappleMultiplier += 1f * Time.fixedDeltaTime;
            lineRend.SetPosition(0, firePoint.position);
            lineRend.SetPosition(1, hitPoint);
        }
        else
        {
            grappleMultiplier = 0f;
            lineRend.enabled = false;
        }
    }

    /// <summary>
    /// Overrides the alt fire function of weapon module - Tommy Flash
    /// </summary>
    override public void AltFire()
    {
        base.AltFire();

        if (!isGrappling)
        {
            RaycastHit2D hit;
            LayerMask mask = LayerMask.GetMask("Ground", "Box");
            hit = Physics2D.Raycast(gameObject.transform.position + Vector3.up, realCamera.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position, grappleDistance, mask);

            hitPoint = hit.point;

            if (hit.collider != null)
            {
                isGrappling = true;
            }
        }
    }

    public override void AltFireKeyUp()
    {
        isGrappling = false;
    }

    #endregion
}
