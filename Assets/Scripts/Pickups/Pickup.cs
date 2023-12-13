using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    #region Variables
    [Tooltip("Trigger for the pickup.")]
    public Collider2D trigger;

    [Tooltip("What colliders on which layers can activate this pickup?")]
    public LayerMask triggerMask;
    #endregion

    #region Methods
    protected void Start()
    {
        SetVars();
    }

    protected void OnValidate()
    {
        SetVars();
    }

    private void SetVars()
    {
        this.RequireComponent(out trigger);
        trigger.isTrigger = true;

        triggerMask.WarnIfMaskEmpty(gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerMask.ContainsLayer(other.gameObject.layer) &&
            other.HasComponent(out Character character))
        {
            // Destroy if some character has picked up the pickup.
            if (ReceiveCharacter(character))
            {
                Destroy(gameObject);
            }
        }
    }

    #region Abstract
    /// <summary>
    /// Receives a character that will pick up the pickup.
    /// </summary>
    /// <param name="character">Character doing the picking up.</param>
    protected virtual bool ReceiveCharacter(Character character) => false;
    #endregion
    #endregion
}
