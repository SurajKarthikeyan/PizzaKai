using NaughtyAttributes;
using UnityEngine;

public class DefaultFlipModule : Module
{
    #region Variables
    [ReadOnly]
    [SerializeField]
    protected Vector3 originalScale;

    [SerializeField]
    private bool negate;
    #endregion

    public int Flipped { get; private set; }

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
        if (theta.AngleIsBetween(-85, 85))
        {
            // Consider the character to be looking to the right.
            Flipped = 1;
        }
        else if (theta.AngleIsBetween(95, 265))
        {
            // Consider the character to be looking to the left.
            Flipped = -1;
        }

        // The negate.
        Flipped *= negate ? -1 : 1;

        Vector3 scale = Master.transform.localScale;
        scale.x = originalScale.x * Flipped;
        Master.transform.localScale = scale;
    }
    #endregion
    #endregion
}