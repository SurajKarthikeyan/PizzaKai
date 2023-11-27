using UnityEngine;

public class AIActReload : AIAction
{
    protected override void PerformAction(EnemyControlModule enemy, TargetToken target)
    {
        WeaponMasterModule wmm = enemy.Master.weaponMasterModule;

        if (wmm)
        {
            wmm.TryReload();
        }
        else
        {
            Debug.LogWarning($"No weapon master found on {enemy}");
        }
    }
}