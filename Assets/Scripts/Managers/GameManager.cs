using NaughtyAttributes;
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

    [Tooltip("The player character.")]
    [ReadOnly]
    [SerializeField]
    private Character player = null;
    #endregion

    #region Properties
    /// <summary>
    /// The player character.
    /// </summary>
    public Character Player
    {
        get => player;
        set
        {
            if (player)
            {
                throw new System.InvalidOperationException(
                    "Cannot have multiple players!"
                );
            }
            else
            {
                player = value;
            }
        }
    }

    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            this.InstantiateSingleton(ref instance);
        }

    }
}