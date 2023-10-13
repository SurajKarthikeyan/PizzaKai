using NaughtyAttributes;
using UnityEngine;

public class Multishot : WeaponSpawn
{
    [HideInInspector]
    //this variable is needed to properly add extra pellets from the shotgun upgrade
    public int upAmount;

    [System.Serializable]
    public class Shot
    {
        public Range amount = new(8);

        public WeaponSpawn toSpawn;
    }

    [InfoBox("Usage:\n" +
        "amount: Number of projectiles to spawn.\n" +
        "toSpawn: What weapon to spawn.")]
    public Shot[] shots;

    protected override void FireInternal()
    {
        foreach (var shot in shots)
        {
            int amount = Mathf.RoundToInt(shot.amount.Evaluate());
            amount += upAmount;
            for (int i = 0; i < amount; i++)
            {
                // Spawn shots.
                shot.toSpawn.Spawn(firedBy);
            }
        }

        Destroy(gameObject);
    }
}