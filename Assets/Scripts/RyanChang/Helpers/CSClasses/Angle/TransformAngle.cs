using UnityEngine;

public class TransformAngle : Angle
{
    /// <summary>
    /// Internal values of the vector
    /// </summary>
    private Transform transform;

    /// <summary>
    /// Creates an angle based off of a transform.
    /// </summary>
    /// <param name="transform">The transform.</param>
    /// <param name="unit">The units of the angle. This will affect the return value of the methods.</param>
    public TransformAngle(Transform transform, Units unit = Units.Degrees)
    {
        this.transform = transform;
        this.unit = unit;
    }

    /// <summary>
    /// Is true if this angle is valid and false otherwise. Specifically, it is not valid if
    /// transform is missing or destroyed.
    /// </summary>
    public bool IsValid => transform;

    /// <summary>
    /// The numerical value of this angle
    /// </summary>
    public float FloatValue => transform.eulerAngles.z;

    /// <summary>
    /// Returns either a 1 or Mathf.Deg2Rad depending on the units
    /// </summary>
    /// <returns></returns>
    private float GetUnitMult()
    {
        return (unit == Units.Degrees) ? 1 : Mathf.Deg2Rad;
    }

    public override float AsNegative()
    {
        return FloatValue.AsNegativeDegrees() * GetUnitMult();
    }

    public override float AsCentered()
    {
        return FloatValue.AsPlusMinus180() * GetUnitMult();
    }

    public override float AsPositive()
    {
        return FloatValue.AsPositiveDegrees() * GetUnitMult();
    }

    public override Angle ConvertUnits(Units newUnit)
    {
        return new TransformAngle(transform, newUnit);
    }
}