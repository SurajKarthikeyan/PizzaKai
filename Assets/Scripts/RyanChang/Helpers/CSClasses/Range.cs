using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Range
{
    public enum RangePattern
    {
        Single,
        Linear,
        Curves,
        Perlin
    }

    [Tooltip("The range pattern to select:\n" +
        "Option 1: Single. A fixed single value is selected.\n" +
        "Option 2: Linear. Randomly select between 2\n\tvalues.\n" +
        "Option 3: Curves. Uses animation curve to select\n\tvalues.\n" +
        "Option 4: Perlin. Uses Perlin noise to select values.\n" +
        "\tPerlin noise is special in that it generates a\n" +
        "\tsmoothrandomization, useful for things like\n" +
        "\tBrownian movement.")]
    public RangePattern rangePattern;

    // [Header("Option 1: Single value as float.")]
    public float singleValue = 0;

    // [Header("Option 2: Specify values as float.")]
    public float scalarMin = -1;
    public float scalarMax = 1;

    // [Header("Option 3: Specify values as curves.")]
    public AnimationCurve curve;

    // [Header("Option 4: Specify forces with Perlin Noise")]
    public float perlinCrawlSpeed = 0.1f;
    private Vector2 crawlPos = RNGExt.RandomVector2(1000);
    private Vector2 crawlDir = RNGExt.OnCircle();

    // [Header("Common to Options 2 and 3")]
    [Tooltip("Modifies the base range to include a multiplication " +
        "followed by an addition.")]
    [FormerlySerializedAs("modifer")]
    public Modifier modifier = new(0, 1);

    /// <summary>
    /// If true, then the range is unfolded in the inspector.
    /// </summary>
    public bool unfoldedInInspector;

    /// <summary>
    /// If true, disable range pattern selection dropdown in inspector.
    /// </summary>
    public bool fixedSelectionInInspector;

    /// <summary>
    /// If true, then the Select method has been called and Range will return
    /// selectedVal when Select is called.
    /// </summary>
    private bool selected = false;

    /// <summary>
    /// The selected value.
    /// </summary>
    private float selectedVal = 0;

    /// <summary>
    /// Constructs a single range.
    /// </summary>
    /// <param name="value">The value to set to.</param>
    public Range(float value)
    {
        singleValue = value;
        scalarMax = value;
        scalarMin = value;
        rangePattern = RangePattern.Single;
        crawlPos = RNGExt.RandomVector2(1000);
        crawlDir = RNGExt.OnCircle();
    }

    /// <summary>
    /// Creates a new linear range with max and min.
    /// </summary>
    /// <param name="min">Minimum value</param>
    /// <param name="max">Maximal value</param>
    public Range(float min, float max)
    {
        singleValue = (min + max) / 2;
        scalarMax = max;
        scalarMin = min;
        rangePattern = RangePattern.Linear;
        crawlPos = RNGExt.RandomVector2(1000);
        crawlDir = RNGExt.OnCircle();
    }

    /// <summary>
    /// Creates new range with desired pattern.
    /// </summary>
    /// <param name="pattern">Pattern to use.</param>
    public Range(RangePattern pattern)
    {
        rangePattern = pattern;
        crawlPos = RNGExt.RandomVector2(1000);
        crawlDir = RNGExt.OnCircle();
    }

    /// <summary>
    /// Locks the range pattern in inspector (put in OnValidate).
    /// </summary>
    /// <param name="pattern">Pattern to lock to.</param>
    public void LockRangePattern(RangePattern pattern)
    {
        rangePattern = pattern;
        fixedSelectionInInspector = true;
    }

    /// <summary>
    /// Evaluates the range and returns the value generated.
    /// </summary>
    /// <returns>The value generated.</returns>
    public float Evaluate()
    {
        switch (rangePattern)
        {
            case RangePattern.Single:
                return singleValue;
            case RangePattern.Linear:
                return RNGExt.RandomFloat(scalarMin, scalarMax);
            case RangePattern.Curves:
                return modifier.Modify(curve.Evaluate(RNGExt.RandomFloat()));
            case RangePattern.Perlin:
                crawlPos += crawlDir * perlinCrawlSpeed;
                return modifier.Modify(Mathf.PerlinNoise(crawlPos.x, crawlPos.y));
            default:
                throw new ArgumentException("Developer needs to update Range Evaluate().");
        }
    }

    /// <summary>
    /// Evaluates the range once. When this function is called again, return the return value
    /// of this function the first time it was called.
    /// </summary>
    /// <returns>The value generated.</returns>
    public float Select()
    {
        if (!selected)
        {
            selected = true;
            selectedVal = Evaluate();
        }

        return selectedVal;
    }

    /// <summary>
    /// Resets the selection
    /// </summary>
    public void Reset()
    {
        selected = false;
    }

    public override string ToString()
    {
        return rangePattern switch
        {
            RangePattern.Single => $"{base.ToString()}, Single Value, {singleValue}",
            RangePattern.Linear => $"{base.ToString()}, Linear, [{scalarMin}, {scalarMax}]",
            RangePattern.Curves => $"{base.ToString()}, Curve, {curve}",
            RangePattern.Perlin => $"{base.ToString()}, Perlin, Crawl Speed = {perlinCrawlSpeed}",
            _ => base.ToString(),
        };
    }
}
