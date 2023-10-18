using NaughtyAttributes;
using UnityEngine;

public class AITreeModule : Module
{
    #region Variables
    public AIBranch root;

    private AIBranch current = null;

    [SerializeField]
    [ReadOnly]
    protected float deltaTime;
    #endregion


    #region Methods
    public void AIInitialize(EnemyControlModule enemy)
    {
        root.InitializeAI(enemy);
    }

    public void AIUpdate(EnemyControlModule enemy, float deltaTime)
    {
        this.deltaTime = deltaTime;
        current ??= root;
        current.tree = this;
        current = current.UpdateAI(enemy);
    }
    #endregion
}