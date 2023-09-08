using NaughtyAttributes;
using UnityEngine;

public class DefaultFlipModule : Module
{
    #region Variables
    [ReadOnly]
    [SerializeField]
    protected Vector3 originalScale;
    #endregion

    #region Methods
    #region Instantiation
    private void Start()
    {
        SetVars();
    }

    // private void OnValidate()
    // {
    //     SetVars();
    // }

    private void SetVars()
    {
        originalScale = Master.transform.localScale;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Sets the direction this character is looking in.
    /// </summary>
    /// <param name="theta">The angle of facing, in degrees, with 0 being due
    /// right.</param>
    public void SetFacingAngle(float theta)
    {
        Vector3 scale = Master.transform.localScale;

        if (theta.AngleIsBetween(-85, 85))
        {
            // Consider the character to be looking to the right.
            scale.x = originalScale.x;
        }
        else if (theta.AngleIsBetween(95, 265))
        {
            // Consider the character to be looking to the left.
            scale.x = -originalScale.x;
        }

        Master.transform.localScale = scale;
    }
    #endregion
    #endregion
}