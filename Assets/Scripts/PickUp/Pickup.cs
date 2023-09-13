using UnityEngine;

/// <summary>
/// Defines a pickup. Pickups can only be picked-up by characters. Once
/// successfully picked-up, it will destroy itself.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
public abstract class Pickup : MonoBehaviour
{
    #region Variables
    [Tooltip("What layers can this interact with?")]
    public LayerMask mask = LayersManager.GetMask(LayersManager.Player);
    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (mask.ContainsLayer(other.gameObject.layer) &&
            other.HasComponent(out Character character))
        {
            if (TryPickup(character))
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Attempts to pick up this pickup. If this returns true, then destroy the
    /// gameObject.
    /// </summary>
    /// <param name="character">The character attempting to do the
    /// pickup.</param>
    /// <returns></returns>
    protected abstract bool TryPickup(Character character);
}