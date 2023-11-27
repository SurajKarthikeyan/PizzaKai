using System;
using UnityEngine;

[Serializable]
public class Modifier
{
    [Tooltip("What is added to the base value. " +
    "Final modification calculated by addition + multiplier * input.")]
    public float addition = 0;

    [Tooltip("What the input is multiplied by. " + 
    "Final modification calculated by addition + multiplier * input.")]
    public float multiplier = 1;

    /// <summary>
    /// Creates a new Modifier.
    /// </summary>
    /// <param name="addition">What is added to the base value.
    /// Final modification calculated by addition + multiplier * input.</param>
    /// <param name="multiplier">What the input is multiplied by. 
    /// Final modification calculated by addition + multiplier * input.</param>
    public Modifier(float addition = 0, float multiplier = 1)
    {
        this.addition = addition;
        this.multiplier = multiplier;
    }

    /// <summary>
    /// Calculate the modification by addition + multiplier * input.
    /// </summary>
    /// <param name="input">Input value to modify.</param>
    /// <returns>The modification.</returns>
    public float Modify(float input)
    {
        return input * multiplier + addition;
    }

    /// <summary>
    /// Modifies a range. <see cref="Range.singleValue"/>, <see
    /// cref="Range.scalarMin"/>, and <see cref="Range.scalarMax"/> are modified
    /// as floats. <see cref="Range.modifier"/> is modified memberwise, which
    /// serves to modify the <see cref="Range.RangePattern.Curves"/> and <see
    /// cref="Range.RangePattern.Perlin"/> modes.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Range Modify(Range input)
    {
        return new(input.rangePattern)
        {
            singleValue = Modify(input.singleValue),
            scalarMin = Modify(input.scalarMin),
            scalarMax = Modify(input.scalarMax),
            modifier = MemberwiseModify(input.modifier)
        };
    }

    /// <summary>
    /// Returns a new modifier whose addition component is the sum of the
    /// inputs' addition components and whose multiplier component is the
    /// product of the inputs' multiplier components.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Modifier MemberwiseModify(Modifier input)
    {
        return new(addition + input.addition, multiplier * input.multiplier);
    }

    public override int GetHashCode()
    {
        return Tuple.Create(addition, multiplier).GetHashCode();
    }
}

