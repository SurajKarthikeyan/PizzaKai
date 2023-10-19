using System.Collections;
using UnityEngine;

public class AIActShoot : AIAction
{
    #region Variables
    [Tooltip("If true, fires continuously. If single fire weapon, only one " +
        "shot will be performed per AI update.")]
    [SerializeField]
    private bool continuousFire = true;

    /// <summary>
    /// Coroutine for the continuous fire option.
    /// </summary>
    private Coroutine continuousFire_CR = null;
    #endregion

    #region Action Implementation
    public override void StartAI(EnemyControlModule enemy, TargetToken target)
    {
        if (continuousFire)
        {
            continuousFire_CR = StartCoroutine(ContinuousFire(enemy, target));
        }
    }

    protected override void PerformAction(EnemyControlModule enemy, TargetToken target)
    {
        DoFire(enemy, target);
    }

    public override void ExitAI(EnemyControlModule enemy)
    {
        if (continuousFire)
        {
            StopCoroutine(continuousFire_CR);
            enemy.Master.weaponMasterModule.ReleaseWeaponTrigger();
        }
    }

    #region Helpers
    private void DoFire(EnemyControlModule enemy, TargetToken target)
    {
        if (enemy.Master.weaponMasterModule)
        {
            Debug.Log("Firing weapon");
            enemy.Master.weaponMasterModule.AimAt(target.Target);

            enemy.Master.weaponMasterModule.PressWeaponTrigger(continuousFire);
        }
        else
        {
            Debug.LogWarning($"No weapon master found on {enemy}");
        }
    }

    /// <summary>
    /// Coroutine for the continuous fire functionality.
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    private IEnumerator ContinuousFire(EnemyControlModule enemy, TargetToken target)
    {
        while (enabled)
        {
            DoFire(enemy, target);    
            yield return new WaitForFixedUpdate();
        }
    }
    #endregion
    #endregion
}