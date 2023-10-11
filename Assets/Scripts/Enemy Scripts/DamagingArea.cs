using UnityEngine;

/// <summary>
/// Applies constant damage to anything that collides with it (if it has a
/// normal collider) or is within it (if it has a trigger collider).
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
public class DamagingArea : MonoBehaviour
{
    #region Variables
    [Tooltip("What layers can this deal damage to?")]
    public LayerMask collisionMask;

    /// <summary>
    /// How much damage this deals.
    /// </summary>
    public ModifiedFloat damage = new(5);
    #endregion

    #region Implementation
    private void OnTriggerStay2D(Collider2D other)
    {
        DealDamage(other.gameObject);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        DealDamage(other.gameObject);
    }

    private void DealDamage(GameObject other)
    {
        if (collisionMask.ContainsLayer(other.layer) &&
            other.HasComponent(out Character character))
        {
            character.TakeDamage(damage.Value.Round());
        }
    }
    #endregion
}