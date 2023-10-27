using UnityEngine;

public abstract class DeathModule : Module
{
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

    protected override void OnLinked()
    {
        Master.onCharacterDeath.AddListener(StartDeath);
        Master.onCharacterRevive.AddListener(StartRevive);
        weaponMaster = Master.GetComponentInChildren<WeaponMasterModule>();
    }

    private void StartDeath()
    {
        if (!ranDeathAction)
        {
            if (delay <= 0)
            {
                OnDeath();
                ranDeathAction = true;
            }

            else
            {
                weaponMaster.gameObject.SetActive(false);
                Master.characterAnimator.Play("PlayerDeath");
                ranDeathAction = true;
                Invoke(nameof(OnDeath), delay);
            }   
        }
    }

    protected abstract void OnDeath();

    private void StartRevive()
    {
        ranDeathAction = false;
    }
}