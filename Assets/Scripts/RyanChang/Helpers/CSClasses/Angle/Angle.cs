/// <summary>
/// Represents an angle
/// </summary>
public abstract class Angle
{
    /// <summary>
    /// Defines the possible units that this angle can be represented as
    /// </summary>
    public enum Units
    {
        Degrees, Radians
    }

    /// <summary>
    /// Current unit
    /// </summary>
    public Units unit;

    /// <summary>
    /// Represents the quadrant this angle is in
    /// </summary>
    public enum Quadrant
    {
        Q1 = 0, UpperRight = 0,
        Q2 = 1, UpperLeft = 1,
        Q3 = 2, LowerLeft = 2,
        Q4 = 3, LowerRight = 3
    }

    /// <summary>
    /// Returns the value of this angle as a positive float [0, 360) degrees, or its radians equivilant
    /// </summary>
    /// <returns>A float on the range of [0, 360) degrees, or its radians equivilant</returns>
    public abstract float AsPositive();

    /// <summary>
    /// Returns the value of this angle as a negative float (-360, 0] degrees, or its radians equivilant
    /// </summary>
    /// <returns>A float on the range of (-360, 0] degrees, or its radians equivilant</returns>
    public abstract float AsNegative();

    /// <summary>
    /// Returns the value of this angle as a float [-180, 180] degrees, or its radians equivilant
    /// </summary>
    /// <returns>A float on the range of [-180, 180] degrees, or its radians equivilant</returns>
    public abstract float AsCentered();

    /// <summary>
    /// Returns the negative of AsCentered. Use this if functions in
    /// Unity display odd behavior with AsCentered.
    /// </summary>
    /// <returns>A float on the range of [-180, 180] degrees</returns>
    public virtual float AsUnityAngle() { return -AsCentered(); }

    /// <summary>
    /// Converts the angle to a new unit and returns that angle.
    /// </summary>
    /// <param name="newUnit">The new unit</param>
    /// <returns>The new angle.</returns>
    public abstract Angle ConvertUnits(Units newUnit);

    /// <summary>
    /// Gets the quadrant this angle is in
    /// </summary>
    /// <returns>The quadrant</returns>
    public Quadrant GetQuadrant()
    {
        float val = AsPositive();

        if (val >= 0 && val < 90)
        {
            return Quadrant.Q1;
        }
        else if (val >= 90 && val < 180)
        {
            return Quadrant.Q2;
        }
        else if (val >= 180 && val < 270)
        {
            return Quadrant.Q3;
        }
        else
        {
            return Quadrant.Q4;
        }
    }

    /// <summary>
    /// Returns the string representation of the angle
    /// </summary>
    /// <returns>The string representation of the angle</returns>
    public override string ToString()
    {
        return $"({base.ToString()}) {unit}";
    }
}
