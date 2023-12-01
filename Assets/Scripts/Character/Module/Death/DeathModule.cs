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

    protected bool ranOnDeath = false;

    protected override void OnLinked(Character old)
    {
        if (old)
        {
            old.onCharacterDeath.RemoveListener(StartDeath);
            old.onCharacterRevive.RemoveListener(StartRevive);
        }

        Master.onCharacterDeath.AddListener(StartDeath);
        Master.onCharacterRevive.AddListener(StartRevive);
        weaponMaster = Master.GetComponentInChildren<WeaponMasterModule>();
    }

    protected virtual void StartDeath()
    {
        
        if (!ranDeathAction)
        {
            if (delay > 0)
            {
                Master.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                DialogueManager.Instance.StopPlayer(true);
                Invoke(nameof(OnDeath), delay);
            }
            else
            {
                if (Master.weaponMasterModule)
                    Master.weaponMasterModule.gameObject.SetActive(false);
            }
        }   
        
    }

    protected abstract void OnDeath();

    private void StartRevive()
    {
        ranDeathAction = false;
    }
}