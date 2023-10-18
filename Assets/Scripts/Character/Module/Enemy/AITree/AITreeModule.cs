public class AITreeModule : Module
{
    #region Variables
    public AIBranch root;

    private AIBranch current = null;
    #endregion


    public void AIUpdate(EnemyControlModule enemy, float deltaTime)
    {
        current = (current ??= root).UpdateAI(enemy, deltaTime);
    }

    public void AIInitialize(EnemyControlModule enemy)
    {
        root.InitializeAI(enemy);
    }
}