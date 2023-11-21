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
    #region Variables
    [SerializeField]
    private float min;

    [SerializeField]
    private float max;
    #endregion

    #region Properties
    public readonly float Min => Mathf.Min(min, max);

    public readonly float Max => Mathf.Max(min, max);
    #endregion

    #region Constructors
    public MinMax(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Determines if <paramref name="value"/> is between the values of <see
    /// cref="Min"/> and <see cref="Max"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public readonly bool IsBetween(float value) => Min <= value && value <= Max;
    #endregion
}