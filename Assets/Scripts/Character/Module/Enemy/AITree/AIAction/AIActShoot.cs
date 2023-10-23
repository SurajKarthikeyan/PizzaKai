using System.Collections;
using UnityEngine;

public class AIActShoot : AIAction
{
    #region Variables
    #endregion

    #region Action Implementation
    protected override void PerformAction(EnemyControlModule enemy, TargetToken target)
    {
        if (enemy.Master.weaponMasterModule)
        {
            //Debug.Log("Firing weapon");
            enemy.Master.weaponMasterModule.AimAt(target.Target);

            enemy.Master.weaponMasterModule.PressWeaponTrigger();
        }
        else
        {
            Debug.LogWarning($"No weapon master found on {enemy}");
        }
    }
    #endregion
}