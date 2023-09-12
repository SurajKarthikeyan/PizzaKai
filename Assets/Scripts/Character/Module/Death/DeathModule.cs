using UnityEngine;

public abstract class DeathModule : Module
{
    [Tooltip("How long to delay for until the death action is performed.")]
    public float delay;

    protected override void OnLinked()
    {
        Master.onCharacterDeathEvent.AddListener(StartDeath);
    }

    private void StartDeath()
    {
        if (delay <= 0)
            OnDeath();
        else
            Invoke(nameof(OnDeath), delay);
    }

    protected abstract void OnDeath();
}