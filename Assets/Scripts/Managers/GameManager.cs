using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Instance
    /// <summary>
    /// The static reference to an GameManager instance.
    /// </summary>
    private static GameManager instance;
    
    /// <summary>
    /// The static reference to an GameManager instance.
    /// </summary>
    public static GameManager Instance => instance;
    #endregion

    #region Variables
    [Tooltip("How long is the invulnerability between taking damage?")]
    public float damageTickRate = 0.5f;

    [Tooltip("The layers where characters can jump off of.")]
    public LayerMask canJumpLayers;
    #endregion

    private void Awake()
    {
        this.InstantiateSingleton(ref instance);
    }
}