using UnityEngine;

/// <summary>
/// Represents the death action of the breadstick (break into two separate
/// enemies).
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
public class BreadstickDeathModule : DeathModule
{
    #region Variables
    [Tooltip("The breadstick prefab to use.")]
    public Character breadstickPrefab;

    [Tooltip("How many splits to do?")]
    public int size = 3;

    [Tooltip("How many sticks to split into?")]
    public int stickSplit = 2;

    [Tooltip("How much force is the child bread sticks subjected to? " +
        "Independent of mass.")]
    public float ejectionForce;

    private const float SIZE_CHANGE = 0.75f;
    #endregion

    protected override void OnDeath()
    {
        if (size > 0)
        {
            float deltaTheta = 180 / (stickSplit + 1);
            Vector2 force = new(ejectionForce, 0);

            for (int i = 0; i < stickSplit; i++)
            {
                // First instantiate the breadboi and set up all variables.
                Character breadInst = breadstickPrefab.InstantiateComponent();
                breadInst.transform.MatchOther(transform);

                breadInst.RequireComponentInChildren(out BreadstickDeathModule breb);
                breb.Master.transform.localScale = Master.transform.localScale * SIZE_CHANGE;

                // Now add force.
                force = force.RotateVector2(deltaTheta);
                breb.Master.r2d.AddForce(force * breb.Master.r2d.mass);
            }
        }
        
        // Kill original.
        Destroy(Master.gameObject);
    }
}