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

    protected override void OnLinked()
    {
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
                Debug.Log("Invoking ondeath");
                Master.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                DialogueManager.Instance.StopPlayer(true);
                Invoke(nameof(OnDeath), delay);
            }
            else
            {
                Debug.Log("First run");
                weaponMaster.gameObject.SetActive(false);
                Master.characterAnimator.Play("PlayerDeath");
            }
        }   
        
    }

    protected abstract void OnDeath();

    private void StartRevive()
    {
        ranDeathAction = false;
    }
}