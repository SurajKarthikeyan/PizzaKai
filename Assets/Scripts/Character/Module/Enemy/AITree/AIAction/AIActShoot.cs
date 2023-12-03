using System.Collections;
using UnityEngine;

public class AIActShoot : AIAction
{
    #region Variables
    [SerializeField]
    [Tooltip("If true, start reloading when exiting the action.")]
    private bool reloadOnExit;

    [SerializeField]
    [Tooltip("If true, consider gravity.")]
    private bool projectilesHaveGravity;
    #endregion

    #region Action Implementation
    protected override void PerformAction(EnemyControlModule enemy, TargetToken target)
    {
        WeaponMasterModule wmm = enemy.Master.weaponMasterModule;

        if (wmm)
        {
            if (wmm.CurrentWeapon.bullet is CollisionProjectile cp)
            {
                float v_0_2 = cp.force * cp.force;
                
            }

            StartCoroutine(Aiming_CR(enemy, target));

            wmm.PressWeaponTrigger();

            if (!wmm.CurrentWeapon.autofire)
            {
                // Release trigger.
                wmm.ReleaseWeaponTrigger();
            }
        }
        else
        {
            Debug.LogWarning($"No weapon master found on {enemy}");
        }
    }

    public override void ExitAI(EnemyControlModule enemy)
    {
        StopAllCoroutines();

        WeaponMasterModule wmm = enemy.Master.weaponMasterModule;

        if (wmm)
        {
            wmm.ReleaseWeaponTrigger();

            if (reloadOnExit)
            {
                wmm.TryReload();
            }
        }
        else
        {
            Debug.LogWarning($"No weapon master found on {enemy}");
        }
    }
    #endregion

    #region Helpers
    private IEnumerator Aiming_CR(EnemyControlModule enemy, TargetToken target)
    {
        WeaponMasterModule wmm = enemy.Master.weaponMasterModule;

        while (selected && enemy)
        {
            yield return new WaitForFixedUpdate();

            wmm.AimAt(target.Position);
        }
    }
    #endregion
}