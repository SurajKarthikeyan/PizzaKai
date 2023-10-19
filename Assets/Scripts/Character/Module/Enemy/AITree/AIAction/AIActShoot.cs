using UnityEngine;

public class AIActShoot : AIAction
{
    #region Action Implementation
    public override void ExitAI(EnemyControlModule enemy)
    {
        // Do nothing.
    }

    public override void InitializeAction(EnemyControlModule enemy)
    {
        // Do nothing.
    }

    protected override void PerformAction(EnemyControlModule enemy, TargetToken target)
    {
        if (enemy.Master.weaponMasterModule)
        {
            Debug.Log("Firing weapon");
            enemy.Master.weaponMasterModule.AimAt(target.Target);
            enemy.Master.weaponMasterModule.TryFire();
        }
        else
        {
            Debug.LogWarning($"No weapon master found on {enemy}");
        }
    }
    #endregion
}