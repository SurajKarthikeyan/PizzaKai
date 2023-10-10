using UnityEngine;

public abstract class DeathModule : Module
{
    [Tooltip("How long to delay for until the death action is performed.")]
    public float delay;

    /// <summary>
    /// Flag that avoids running this multiple times.
    /// </summary>
    protected bool ranDeathAction;

    protected override void OnLinked()
    {
        Master.onCharacterDeath.AddListener(StartDeath);
        Master.onCharacterRevive.AddListener(StartRevive);
    }

    private void StartDeath()
    {
        if (!ranDeathAction)
        {
            ranDeathAction = true;
            
            if (delay <= 0)
                OnDeath();
            else
                Invoke(nameof(OnDeath), delay);
        }
    }

    protected abstract void OnDeath();

    private void StartRevive()
    {
        ranDeathAction = false;
    }
}