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
        if (Master.gameObject.CompareTag("Player"))
        {
            weaponMaster = Master.gameObject.transform.GetComponentInChildren<WeaponMasterModule>();
        }
        else
        {
            weaponMaster = Master.weaponMasterModule;
        }
            
    }

    protected virtual void StartDeath()
    {
        if (Master.gameObject.CompareTag("Player"))
        {
            if (!ranDeathAction)
            {
                if (delay > 0)
                {
                    //Debug.Log("Invoking ondeath");
                    Master.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    DialogueManager.Instance.StopPlayer(true);
                    Invoke(nameof(OnDeath), delay);
                }
                else
                {
                    //Debug.Log("First run");
                    weaponMaster.gameObject.SetActive(false);
                    Master.characterAnimator.Play("PlayerDeath");
                }
            }
        }
        else
        {
            if (!ranDeathAction)
            {
                ranDeathAction = true;

                if (Master.weaponMasterModule)
                    Master.weaponMasterModule.gameObject.SetActive(false);

                if (delay > 0)
                {
                    Master.characterAnimator.SetBool("Death", true);
                    Invoke(nameof(OnDeath), delay);
                }
                else
                {
                    OnDeath();
                }
            }
        }
        
    }

    /// <summary>
    /// Called when the character dies and after the specified <see
    /// cref="delay"/>.
    /// </summary>
    protected abstract void OnDeath();

    protected virtual void StartRevive()
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