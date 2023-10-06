using UnityEngine;

public class HealthPickup : Pickup
{
    #region Variables
    public int healthIncrease;
    #endregion

    #region Pickup Implementation
    protected override bool ReceiveCharacter(Character character)
    {
        character.Heal(healthIncrease);
        return true;
    }
    #endregion
}