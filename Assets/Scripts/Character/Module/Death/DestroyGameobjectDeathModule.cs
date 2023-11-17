/// <summary>
/// Death module that destroys the character's game object.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class DestroyGameobjectDeathModule : DeathModule
{
    protected override void OnDeath()
    {
        Destroy(Master.gameObject);
    }
}