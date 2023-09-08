using UnityEngine;

/// <summary>
/// Represents a damaging area. This should contain a collider that can be
/// either a trigger or not a trigger.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class DamageArea : DamageSource
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (targetableLayers.ContainsLayer(other.gameObject.layer) &&
            other.HasComponent(out Character character))
        {
            character.TakeDamage(damage);
        }
    }
}