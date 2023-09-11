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

    public override void Fire(WeaponModule weapon)
    {
        base.Fire(weapon);

        foreach (var shot in shots)
        {
            int amount = Mathf.RoundToInt(shot.amount.Evaluate());
            for (int i = 0; i < amount; i++)
            {
                // Spawn shots.
                shot.toSpawn.Spawn(weapon).Fire(weapon);
            }
        }
    }

    protected override void Fire(int burstIndex)
    {
        throw new System.NotImplementedException();
    }
}