using UnityEngine;

public abstract class DeathModule : Module
{
    [Tooltip("How long to delay for until the death action is performed.")]
    public float delay;

    /// <summary>
    /// Flag that avoids running this multiple times.
    /// </summary>
    private bool ranDeathAction;

    protected override void OnLinked()
    {
        Master.onCharacterDeathEvent.AddListener(StartDeath);
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
}