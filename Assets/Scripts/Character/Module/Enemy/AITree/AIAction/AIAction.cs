using System.Text.RegularExpressions;
using NaughtyAttributes;


/// <summary>
/// Responsible for defining the actions that AI will do.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
public abstract class AIAction : Module
{
    #region Variables
    /// <summary>
    /// True if this action is selected to run.
    /// </summary>
    [ReadOnly]
    public bool selected;
    #endregion

    #region Action Methods
    /// <summary>
    /// Initializes the action. Called when the behavior tree is initialized.
    /// </summary>
    public virtual void InitializeAction(EnemyControlModule enemy) { }

    /// <summary>
    /// Called when the AI has chosen to execute action.
    /// </summary>
    /// <inheritdoc cref="UpdateAI(EnemyControlModule, TargetToken)"/>
    public virtual void StartAI(EnemyControlModule enemy, TargetToken target) { }

    /// <summary>
    /// Called when the AI is updated with FixedUpdate.
    /// </summary>
    /// <param name="enemy">The enemy controlled by the AI.</param>
    /// <param name="target">The target of the enemy, generated by <see
    /// cref="AITargeting.GetTarget"/>.</param>
    public void UpdateAI(EnemyControlModule enemy, TargetToken target)
    {
        PerformAction(enemy, target);
    }

    /// <summary>
    /// Called when it's time to exit control of the AI action.
    /// </summary>
    /// <inheritdoc cref="UpdateAI(EnemyControlModule, TargetToken)"/>
    public virtual void ExitAI(EnemyControlModule enemy) { }
    #endregion

    #region Helpers
    /// <summary>
    /// Performs the selected functions.
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="target"></param>
    protected abstract void PerformAction(EnemyControlModule enemy, TargetToken target);
    #endregion

    #region ToString
    private static readonly Regex actTypeRX = new(@"AIAct(\w+)");

    public override string ToString()
    {
        string actType = GetType().Name;
        actType = actTypeRX.Match(actType).Groups[1].Value;
        return $"[Action:{actType}]";
    }
    #endregion
}