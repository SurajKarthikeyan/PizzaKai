using Fungus;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Contains a min and max value.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
[System.Serializable]
public struct MinMax
{
    #region Enums
    /// <summary>
    /// The mode of which the minmax operates.
    /// </summary>
    public enum Mode
    {
        Between,
        NotBetween,
        GreaterThan,
        LessThan
    }
    #endregion

    #region Variables
    public Mode mode;

    [AllowNesting]
    [ShowIf(nameof(Max_ShowIf))]
    [SerializeField]
    private float min;

    [AllowNesting]
    [ShowIf(nameof(Min_ShowIf))]
    [SerializeField]
    private float max;

    #region Validation
    private readonly bool Min_ShowIf => mode != Mode.GreaterThan;
    private readonly bool Max_ShowIf => mode != Mode.LessThan;
    #endregion
    #endregion

    #region Properties
    public readonly float Min => mode switch {
        Mode.LessThan => float.NaN,
        _ => Mathf.Min(min, max)
    };

    public readonly float Max => mode switch {
        Mode.GreaterThan => float.NaN,
        _ => Mathf.Max(min, max)
    };
    #endregion

    #region Constructors
    public MinMax(float min, float max)
    {
        this.min = min;
        this.max = max;
        mode = Mode.Between;
    }

    public MinMax(float min, float max, Mode mode)
    {
        this.min = min;
        this.max = max;
        this.mode = mode;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Determines if <paramref name="value"/> satisfies this range.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public readonly bool Evaluate(float value)
    {
        return mode switch
        {
            Mode.Between => Min <= value && value <= Max,
            Mode.LessThan => value <= Max,
            Mode.GreaterThan => value >= Min,
            Mode.NotBetween => Min > value && value > Max,
            _ => false
        };
    }
    #endregion
}