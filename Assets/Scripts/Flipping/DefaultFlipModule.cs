using NaughtyAttributes;
using UnityEngine;

public class DefaultFlipModule : Module
{
    #region Variables
    [ReadOnly]
    [SerializeField]
    protected Vector3 originalScale;

    [Tooltip("Negates the flipping.")]
    [SerializeField]
    private bool negate;

    [Tooltip("Whether or not this is flipped.")]
    [SerializeField]
    [ReadOnly]
    private bool flipped;
    #endregion

    /// <summary>
    /// If <see cref="flipped"/>, then -1. Otherwise, 1.
    /// </summary>
    public int FlipMultiplier => flipped ? -1 : 1;

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
            flipped = false;
        }
        else if (theta.AngleIsBetween(95, 265))
        {
            // Consider the character to be looking to the left.
            flipped = true;
        }

        // Apply negation.
        if (negate)
            flipped = !flipped;

        Vector3 scale = Master.transform.localScale;
        scale.x = originalScale.x * FlipMultiplier;
        Master.transform.localScale = scale;
    }
    #endregion
    #endregion
}