using UnityEngine;

public abstract class DeathModule : Module
{
    #region Variable
    [Tooltip("How long to delay for until the death action is performed.")]
    public float delay;

    /// <summary>
    /// Flag that avoids running this multiple times.
    /// </summary>
    protected bool ranDeathAction = false;

    /// <summary>
    /// Reference to the weapon master to disable it for death
    /// </summary>
    protected WeaponMasterModule weaponMaster;
    #endregion

    protected override void OnLinked(Character old)
    {
        if (old)
        {
            old.onCharacterDeath.RemoveListener(StartDeath);
            old.onCharacterRevive.RemoveListener(StartRevive);
        }

        Master.onCharacterDeath.AddListener(StartDeath);
        Master.onCharacterRevive.AddListener(StartRevive);
        weaponMaster = Master.weaponMasterModule;
    }

    protected void StartDeath()
    {
        if (!ranDeathAction)
        {
            ranDeathAction = true;

            if (Master.weaponMasterModule)
                Master.weaponMasterModule.gameObject.SetActive(false);

            if (delay > 0)
            {
                Invoke(nameof(OnDeath), delay);
            }
            else
            {
                OnDeath();
            }
        }
    }

    /// <summary>
    /// Called when the character dies and after the specified <see
    /// cref="delay"/>.
    /// </summary>
    protected abstract void OnDeath();

    protected void StartRevive()
    {
        if (ranDeathAction)
        {
            ranDeathAction = false;

            if (Master.weaponMasterModule)
                Master.weaponMasterModule.gameObject.SetActive(true);

            OnRevive();
        }
    }

    /// <summary>
    /// Called to "undo" the effects of death. Optionally defined.
    /// </summary>
    protected virtual void OnRevive() { }
}