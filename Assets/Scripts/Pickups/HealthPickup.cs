using UnityEngine;

public class HealthPickup : Pickup
{
    #region Variables
    public int healthIncrease;
    #endregion

    #region Pickup Implementation
    protected override bool ReceiveCharacter(Character character)
    {
        if (character.HP == character.maxHP)
            return false;

        character.Heal(healthIncrease);
        return true;
    }
    #endregion
}