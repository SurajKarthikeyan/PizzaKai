using UnityEngine;

/// <summary>
/// Used to allow enemies to drop items based on a drop table.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class ItemDropModule : Module
{
    public MultiSelector<GameObject> dropTable;

    [Tooltip("What's the maximum random force that will be applied to the " +
        "spawned objects?")]
    public float maxForce = 2;

    protected override void OnLinked()
    {
        Master.onCharacterDeathEvent.AddListener(DropItems);
    }

    private void DropItems()
    {
        foreach (var item in dropTable.DoSelection())
        {
            var itemInst = Instantiate(item);
            itemInst.SetActive(true);
            itemInst.transform.parent = null;
            itemInst.transform.position = transform.position;

            if (itemInst.HasComponent(out Spawner spawner))
            {
                // Spawner doesn't activate immediately, so we can go ahead and
                // assign this now.
                spawner.maxStartForce = maxForce;
            }

            if (itemInst.HasComponent(out Rigidbody2D r2d))
            {

                r2d.AddForce(
                    RNGExt.WithinCircle(Mathf.Abs(maxForce)),
                    ForceMode2D.Impulse
                );
            }
        }
    }
}