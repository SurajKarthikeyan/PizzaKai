using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// Responsible for providing a target for the AI to target (to follow, attack,
/// etc). This is accomplished through the <see cref="TargetToken"/> provided by
/// <see cref="GetTarget"/>. Changing the target while the branch is executing
/// is not supported.
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

    /// <summary>
    /// Calculates the target.
    /// </summary>
    /// <returns>The target.</returns>
    public abstract TargetToken GetTarget(EnemyControlModule enemy);
    #endregion

    #region Object Overrides
    /// <summary>
    /// Regex for <see cref="ToString"/>
    /// </summary>
    private static readonly Regex actTypeRX = new(@"AITarget(\w+)");

    public override string ToString()
    {
        string actType = GetType().Name;
        actType = actTypeRX.Match(actType).Groups[1].Value;
        return $"[Target:{actType}]";
    }
    #endregion
}