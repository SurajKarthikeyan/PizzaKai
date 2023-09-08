using System;
using UnityEngine;

/// <summary>
/// Contains methods pertaining to angles represented as simple floats.
/// </summary>
public static class AngleExt
{
    public const float PI2 = Mathf.PI * 2;

    #region Angle Representation Conversion
    /// <summary>
    /// Returns an angle between [-360, 360] degrees
    /// </summary>
    /// <param name="theta">The angle to consider</param>
    /// <returns>An angle between [-360, 360] degrees</returns>
    public static float AsPlusMinus360(this float theta)
    {
        while (theta < -360)
            theta += 360;

        while (theta > 360)
            theta -= 360;

        return theta;
    }

    /// <summary>
    /// Returns an angle between [0, 360) degrees
    /// </summary>
    /// <param name="theta">The angle to consider</param>
    /// <returns>An angle between [0, 360) degrees</returns>
    public static float AsPositiveDegrees(this float theta)
    {
        while (theta < 0)
            theta += 360;

        while (theta > 360)
            theta -= 360;

        return theta;
    }

    /// <summary>
    /// Returns an angle between [0, 2pi) radians
    /// </summary>
    /// <param name="theta">The angle to consider</param>
    /// <returns>An angle between [0, 2pi) radians</returns>
    public static float AsPositiveRadians(this float theta)
    {
        while (theta < 0)
            theta += PI2;

        while (theta > PI2)
            theta -= PI2;

        return theta;
    }

    /// <summary>
    /// Returns an angle between (-360, 0] degrees
    /// </summary>
    /// <param name="theta">The angle to consider</param>
    /// <returns>An angle between (-360, 0] degrees</returns>
    public static float AsNegativeDegrees(this float theta)
    {
        theta = theta.AsPositiveDegrees();

        if (theta == 0)
            return 0;
        else
            return theta - 360;
    }

    /// <summary>
    /// Returns an angle between (-2pi, 0] degrees
    /// </summary>
    /// <param name="theta">The angle to consider</param>
    /// <returns>An angle between (-2pi, 0] degrees</returns>
    public static float AsNegativeRadians(this float theta)
    {
        theta = theta.AsPositiveRadians();

        if (theta == 0)
            return 0;
        else
            return theta - PI2;
    }

    /// <summary>
    /// Returns an angle between [-180, 180] degrees
    /// </summary>
    /// <param name="theta"></param>
    /// <returns>An angle between [-180, 180] degrees</returns>
    public static float AsPlusMinus180(this float theta)
    {
        while (theta < -180)
            theta += 360;

        while (theta > 180)
            theta -= 360;

        return theta;
    }

    /// <summary>
    /// Returns an angle between [-pi, pi] radians
    /// </summary>
    /// <param name="theta"></param>
    /// <returns>An angle between [-pi, pi] radians</returns>
    public static float AsPlusMinusPi(this float theta)
    {
        while (theta < -Mathf.PI)
            theta += PI2;

        while (theta > Mathf.PI)
            theta -= PI2;

        return theta;
    }

    /// <summary>
    /// Returns an angle with a side. True is right side and false is left.
    /// <br/><br/>
    /// For example:<br/>
    /// theta = 120 returns 60 and false<br/>
    /// theta = 186 returns -6 and false<br/>
    /// theta = 16 returns 16 and true<br/>
    /// theta = 291 returns -69 and true
    /// </summary>
    /// <param name="theta"></param>
    /// <returns></returns>
    public static Tuple<float, bool> AsPlusMinus90AndSide(this float theta)
    {
        theta = theta.AsPlusMinus180();

        if ((theta >= 0 && theta <= 90) || (theta < 0 && theta > -90))
        {
            //Right side
            return new Tuple<float, bool>(theta, true);
        }
        else
        {
            if (theta >= 0)
                theta = 180 - theta;
            else
            {
                theta += 180;
                theta *= -1;
            }

            return new Tuple<float, bool>(theta, false);
        }
    }
    #endregion

    /// <summary>
    /// Returns true if both angles represents the same angle
    /// </summary>
    /// <param name="angle1"></param>
    /// <param name="angle2"></param>
    /// <returns></returns>
    public static bool AngleEqual(this float angle1, float angle2)
    {
        return Mathf.DeltaAngle(angle1, angle2) == 0;
    }

    /// <summary>
    /// Returns true if angle is between theta1 and theta2, that is, angle lies in the
    /// smallest arc formed by theta1 and theta2
    /// </summary>
    /// <param name="angle">The angle to evaluate</param>
    /// <param name="theta1"></param>
    /// <param name="theta2"></param>
    /// <returns>An angle between -180 and 180.</returns>
    public static bool AngleIsBetween(this float angle, float theta1, float theta2)
    {
        float min = Mathf.Min(theta1, theta2);
        float max = Mathf.Max(theta1, theta2);

        //Debug.Log($"{min} {max} {angle}");
        //Debug.Log($"min.GetDeltaTheta(angle): {min.GetDeltaTheta(angle)}");
        //Debug.Log($"max.GetDeltaTheta(angle): {max.GetDeltaTheta(angle)}");

        return min.GetDeltaTheta(angle) >= 0 && max.GetDeltaTheta(angle) <= 0;
    }

    /// <summary>
    /// Returns angle if it falls within the smallest arc formed by theta1 and theta2.
    /// Else, returns either theta1 or theta2.
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="theta1"></param>
    /// <param name="theta2"></param>
    /// <returns></returns>
    public static float ClampAngle(this float angle, float theta1, float theta2)
    {
        float min = Mathf.Min(theta1, theta2);
        float max = Mathf.Max(theta1, theta2);

        if (max - min == 180)
        {
            throw new ArgumentException($"Unable to clamp angle ({angle}) as theta 1 ({theta1}) and theta2 ({theta2}) " +
                $"differ by 180 degrees. There are two locations to clamp to.");
        }

        switch (min.GetDeltaTheta(angle))
        {
            case < 0:
                return min;
            case > 0:
                return max;
            default:
                return angle;
        }
    }

    /// <summary>
    /// Gets the direction of travel from actualTheta to targetTheta.
    /// If return value is -1, turn clockwise.
    /// If return value is 1, turn counterclockwise.
    /// </summary>
    /// <param name="actualTheta"></param>
    /// <param name="targetTheta"></param>
    /// <returns></returns>
    public static int DirectionToAngle(this float actualTheta, float targetTheta)
    {
        actualTheta = actualTheta.AsPositiveDegrees();
        targetTheta = targetTheta.AsPositiveDegrees();

        if (actualTheta.Approx(targetTheta))
            return 0;
        else
            return (Mathf.DeltaAngle(actualTheta, targetTheta) < 0) ? 1 : -1;
    }

    /// <summary>
    /// Gets the direction of travel from actualTheta to targetTheta.
    /// If return value is -1, turn clockwise.
    /// If return value is 1, turn counterclockwise.
    /// </summary>
    /// <param name="actualTheta"></param>
    /// <param name="targetTheta"></param>
    /// <returns></returns>
    public static int DirectionToAngle(this float actualTheta, float targetTheta, float margin)
    {
        actualTheta = actualTheta.AsPositiveDegrees();
        targetTheta = targetTheta.AsPositiveDegrees();

        if (actualTheta.Approx(targetTheta, margin))
            return 0;
        else
            return (Mathf.DeltaAngle(actualTheta, targetTheta) < 0) ? 1 : -1;
    }

    /// <summary>
    /// Returns the shortest angle between actualTheta and targetTheta.
    /// </summary>
    /// <param name="actualTheta"></param>
    /// <param name="targetTheta"></param>
    /// <returns></returns>
    public static float GetDeltaTheta(this float actualTheta, float targetTheta)
    {
        return Mathf.DeltaAngle(actualTheta, targetTheta);
    }
}