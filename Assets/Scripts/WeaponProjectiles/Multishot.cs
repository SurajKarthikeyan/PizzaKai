using UnityEngine;

public class Multishot : WeaponSpawn
{
    [System.Serializable]
    public class Shot
    {
        public Range amount = new(8);

        public WeaponSpawn toSpawn;
    }

    public Shot[] shots;

    protected override void FireInternal()
    {
        foreach (var shot in shots)
        {
            int amount = Mathf.RoundToInt(shot.amount.Evaluate());
            for (int i = 0; i < amount; i++)
            {
                // Spawn shots.
                shot.toSpawn.Spawn(firedBy);
            }
        }

        Destroy(gameObject);
    }
}