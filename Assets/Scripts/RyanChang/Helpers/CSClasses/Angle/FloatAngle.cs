using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Represents an angle with greater control than a float
/// </summary>
[Serializable]
public class FloatAngle : Angle
{
    /// <summary>
    /// The internal value of the angle
    /// </summary>
    [SerializeReference]
    private float value;

    /// <summary>
    /// Create a new angle with a degrees value. Allows convertion
    /// from radian value
    /// </summary>
    /// <param name="value">Value of the angle</param>
    /// <param name="unit">If true, then convert.</param>
    public FloatAngle(float value, Units unit = Units.Degrees)
    {
        this.value = value;
        this.unit = unit;
    }

	/// <summary>
	/// Returns a float representation of this FloatAngle. Ranges from
	/// [0,360] for degrees and [0,2pi] for radians.
	/// </summary>
    public override float AsPositive()
    {
        return (unit == Units.Degrees) ? value.AsPositiveDegrees() : value.AsPositiveRadians();
    }

	/// <summary>
	/// Returns a float representation of this FloatAngle. Ranges from
	/// [-360,0] for degrees and [-2pi,0] for radians.
	/// </summary>
    public override float AsNegative()
    {
        return (unit == Units.Degrees) ? value.AsNegativeDegrees() : value.AsNegativeRadians();
    }

	/// <summary>
	/// Returns a float representation of this FloatAngle. Ranges from
	/// [-180,180] for degrees and [-pi,pi] for radians.
	/// </summary>
    public override float AsCentered()
    {
        return (unit == Units.Degrees) ? value.AsPlusMinus180() : value.AsPlusMinusPi();
    }

    /// <summary>
    /// Gets the direction to other. +1 for counterclockwise,
    /// -1 for clockwise
    /// </summary>
    /// <param name="other">The other angle you are compairing</param>
    /// <returns>See above</returns>
    public int GetDirectionTo(FloatAngle other)
    {
        return (this - other).AsCentered().Sign();
    }

    public override Angle ConvertUnits(Units newUnit)
    {
        if (unit == newUnit)
            return this;

        float newValue = value;

        switch (newUnit)
        {
            case Units.Degrees:
                newValue *= Mathf.Rad2Deg;
                break;
            case Units.Radians:
                newValue *= Mathf.Deg2Rad;
                break;
        }

        return new FloatAngle(newValue, newUnit);
    }

    /// <summary>
    /// Adds 2 angles. If they have the same units, then preserve both units. Otherwise, 
    /// b will be converted to a's units.
    /// </summary>
    /// <param name="a">First angle</param>
    /// <param name="b">Second angle</param>
    /// <returns></returns>
    public static FloatAngle operator +(FloatAngle a, FloatAngle b)
    {
        if (a.unit == b.unit)
        {
            // Like units. Can add directly.
            return new FloatAngle(a.value + b.value, a.unit);
        }
        else
        {
            // Unlike units, convert b to unit of a to get matching units
            var b_val = b.value * (b.unit == Units.Degrees ? Mathf.Deg2Rad : Mathf.Rad2Deg);
            return new FloatAngle(a.value + b_val, a.unit);
        }
    }

    /// <summary>
    /// Subtracts 2 angles. If they have the same units, then preserve both units. Otherwise, 
    /// b will be converted to a's units.
    /// </summary>
    /// <param name="a">First angle</param>
    /// <param name="b">Second angle</param>
    /// <returns></returns>
    public static FloatAngle operator -(FloatAngle a, FloatAngle b)
    {
        if (a.unit == b.unit)
        {
            // Like units. Can subtract directly.
            return new FloatAngle(a.value - b.value, a.unit);
        }
        else
        {
            // Unlike units, convert b to unit of a to get matching units
            var b_val = b.value * (b.unit == Units.Degrees ? Mathf.Deg2Rad : Mathf.Rad2Deg);
            return new FloatAngle(a.value - b_val, a.unit);
        }
    }

    /// <summary>
    /// Multiplies a FloatAngle with a float, resulting in a new FloatAngle b times the size.
    /// Units do not matter.
    /// </summary>
    /// <param name="a">The FloatAngle</param>
    /// <param name="b">The float to multipy a by</param>
    /// <returns>The new FloatAngle, units are preserved.</returns>
    public static FloatAngle operator *(FloatAngle a, float b)
    {
        return new FloatAngle(a.value * b, a.unit);
    }

    /// <summary>
    /// Divides a FloatAngle by a float, resulting in a new FloatAngle 1/b times the size.
    /// Units do not matter.
    /// </summary>
    /// <param name="a">The FloatAngle</param>
    /// <param name="b">The float to divide a by</param>
    /// <returns>The new FloatAngle, units are preserved.</returns>
    public static FloatAngle operator /(FloatAngle a, float b)
    {
        return new FloatAngle(a.value / b, a.unit);
    }

    /// <summary>
    /// Returns the string representation of the angle
    /// </summary>
    /// <returns>The string representation of the angle</returns>
    public override string ToString()
    {
        return $"({base.ToString()}) {value} {unit}";
    }
}
