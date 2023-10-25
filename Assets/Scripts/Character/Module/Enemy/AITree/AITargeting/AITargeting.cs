using System.Text.RegularExpressions;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Responsible for providing a target for the AI to target (to follow, attack,
/// etc). This is accomplished through the <see cref="TargetToken"/> provided by
/// <see cref="GetTarget"/>.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
[System.Serializable]
public abstract class AITargeting : MonoBehaviour
{
    #region Targeting Methods
    /// <summary>
    /// Initializes the targeting.
    /// </summary>
    /// <param name="enemy">The enemy who this targeting belongs to.</param>
    public virtual void InitializeTargeting(EnemyControlModule enemy) { }

    public abstract TargetToken GetTarget();
    #endregion

    #region ToString
    private static readonly Regex actTypeRX = new(@"AITarget(\w+)");

    public override string ToString()
    {
        string actType = GetType().Name;
        actType = actTypeRX.Match(actType).Groups[1].Value;
        return $"[Target:{actType}]";
    }
    #endregion
}