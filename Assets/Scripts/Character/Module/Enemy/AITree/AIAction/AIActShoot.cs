using UnityEngine;

public class AIActShoot : AIAction
{
    #region Variables
    [Tooltip("If true, fires continuously. If single fire weapon, only one " +
        "shot will be performed.")]
    [SerializeField]
    private bool continuousFire = true;
    #endregion

    #region Action Implementation
    protected override void PerformAction(EnemyControlModule enemy, TargetToken target)
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
    #endregion
}