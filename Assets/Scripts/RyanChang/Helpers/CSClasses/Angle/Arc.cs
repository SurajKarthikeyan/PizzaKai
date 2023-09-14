using UnityEngine;

public class Arc
{
    #region Variables
    /// <summary>
    /// The right-sided angle. The arc is formed starting at this angle and
    /// moves counter-clockwise.
    /// </summary>
    [Tooltip("The right-sided angle. The arc is formed starting at this " +
        "angle and moves counter-clockwise.")]
    public FloatAngle minAngle = new(-45);

    /// <summary>
    /// The left-sided angle. The arc ends at this angle.
    /// </summary>
    [Tooltip("The left-sided angle. The arc ends at this angle.")]
    public FloatAngle maxAngle = new(45);

    /// <summary>
    /// How far does the arc extend?
    /// </summary>
    [Tooltip("How far does the arc extend?")]
    public float radius = 1;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new arc.
    /// </summary>
    /// <param name="minAngle">The right-sided angle. The arc is formed starting
    /// at this angle and moves counter-clockwise.</param>
    /// <param name="maxAngle">The left-sided angle. The arc ends at this
    /// angle.</param>
    /// <param name="radius">How far does the arc extend?</param>
    public Arc(FloatAngle minAngle, FloatAngle maxAngle, float radius)
    {
        this.minAngle = minAngle;
        this.maxAngle = maxAngle;
        this.radius = radius;
    }

    /// <summary>
    /// Creates a new arc with angles measured in degrees.
    /// </summary>
    /// <param name="minDegrees">The right-sided angle in degrees. The arc is
    /// formed starting at this angle and moves counter-clockwise.</param>
    /// <param name="maxDegrees">The left-sided angle in degrees. The arc ends at this angle.</param>
    /// <inheritdoc cref="Arc(FloatAngle, FloatAngle, float)"/>
    public Arc(float minDegrees, float maxDegrees, float radius)
    {
        this.minAngle = new(minDegrees);
        this.maxAngle = new(maxDegrees);
        this.radius = radius;
    }
    #endregion

    public int OverlapCollidersNonAlloc(Vector2 position, Collider2D[] results,
        int layerMask, float minDepth, float maxDepth)
    {
        int cnt = Physics2D.OverlapCircleNonAlloc(position, radius, results,
            layerMask, minDepth, maxDepth);

        int lPtr = results.Length - 1;
        
        for (int i = 0; i < cnt;)
        {
            // Determine if this point is within this arc.
            Collider2D collider = results[i];
            Vector2 hitPos = collider.transform.position;
            Vector2 diff = hitPos - position;
            FloatAngle theta = new(Mathf.Atan2(diff.y, diff.x), FloatAngle.Units.Radians);

            if (!theta.IsBetween(minAngle, maxAngle))
            {
                // Theta is invalid. Swap this element with another element at
                // the end of the array.
                results.Swap(i, lPtr);
                lPtr--;
                cnt--;
            }
            else
            {
                // Theta is valid. Move onto next element.
                i++;
            }
        }

        return cnt;
    }
}