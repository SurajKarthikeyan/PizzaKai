using UnityEngine;

/// <summary>
/// Represents a source of damage that can target specific layers.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public abstract class DamageSource : MonoBehaviour
{
    #region Variables
    #region User Settings
    public int damage = 1;

    public LayerMask targetableLayers;
    #endregion
    #endregion
}