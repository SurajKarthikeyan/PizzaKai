using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A character is an enemy or a player, and contains references to variables
/// used by either. This consolidates references and makes further development
/// much more easier and less bug prone.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(DefaultFlipModule))]
public sealed class Character : MonoBehaviour
{
    #region Variables
    #region User Settings
    /// <summary>
    /// The current hp of the character. Please use <see cref="HP"/> instead, as
    /// that fires the <see cref="EventManager.onCharacterDeath"/> event.
    /// </summary>
    private int hp;

    /// <summary>
    /// The maximum health of the character.
    /// </summary>
    public int maxHP;
    #endregion

    #region Autogenerated
    [ReadOnly]
    public Rigidbody2D r2d;

    [ReadOnly]
    public Collider2D c2d;

    [ReadOnly]
    public SpriteRenderer sr;

    [ReadOnly]
    public DefaultFlipModule flipModule;

    [Tooltip("Optional.")]
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private Duration damageInvulnerability;

    [SerializeField]
    private float knockbackMultiplier = 1;
    #endregion

    #endregion

    #region Properties
    /// <summary>
    /// Determines whether or not the character is a player based on the
    /// existence of a <see cref="PlayerMoveModule"/>.
    /// </summary>
    public bool IsPlayer { get; private set; }

    /// <summary>
    /// True if HP is 0.
    /// </summary>
    public bool IsDead => hp <= 0;

    /// <summary>
    /// The current HP of the character. Setting this value also calls <see
    /// cref="EventManager.onCharacterDeath"/> if the character's health drops
    /// below 0.
    /// </summary>
    public int HP
    {
        get => hp;
        private set
        {
            hp = value;

            if (hp <= 0)
            {
                hp = 0;
                EventManager.Instance.onCharacterDeath.Invoke(this);
                onCharacterDeath.Invoke();
            }
        }
    }

    public List<Module> Modules { get; private set; } = new();
    #endregion

    // These are local events that can be captured by child Modules. By
    // convention, please make sure to invoke these before related events in
    // EventManager.
    #region Local Events
    /// <summary>
    /// Called on character death.
    /// </summary>
    public readonly UnityEvent onCharacterDeath = new();

    /// <summary>
    /// Called when the character is revived.
    /// </summary>
    public readonly UnityEvent onCharacterRevive = new();


    /// <summary>
    /// Called on character hurt.
    /// 
    /// <br/>
    /// 
    /// T0 = Damage taken.
    /// </summary>
    public readonly UnityEvent<int> onCharacterHurt = new();
    #endregion

    #region Methods
    #region Instantiation
    private void Awake()
    {
        SetVars();
        HP = maxHP;
    }

    // private void OnValidate()
    // {
    //     SetVars();
    // }

    private void SetVars()
    {
        GetComponentsInChildren(true, Modules);
        Modules.ForEach(module => module.LinkToMaster(this));

        this.RequireComponent(out r2d);
        this.RequireComponent(out c2d);
        this.RequireComponentInChildren(out sr);

        this.RequireComponent(out flipModule);

        IsPlayer = this.HasComponentInChildren(out PlayerControlModule player);

        if (IsPlayer)
        {
            GameManager.Instance.Player = player.Master;
        }
    }
    #endregion

    #region MonoBehavior Methods
    public void Update()
    {
        // Required to get this to work properly.
        damageInvulnerability.IncrementUpdate(false);
    }
    #endregion

    #region Flipping Methods
    /// <inheritdoc cref="DefaultFlipModule.SetFacingAngle(float)"/>
    public void SetLookAngle(float theta)
    {
        flipModule.SetFacingAngle(theta);
    }
    #endregion

    #region Health Methods
    /// <summary>
    /// Take <paramref name="damage"/> amount of damage.
    /// </summary>
    /// <param name="damage">How much damage to take.</param>
    public void TakeDamage(int damage)
    {
        if (damageInvulnerability.IsDone)
        {
            HP -= damage;
            damageInvulnerability.Reset();
        }

        if (IsPlayer)
        {
            PlayerKnockback();
        }
    }

    /// <summary>
    /// Take damage with knockback.
    /// </summary>
    /// <inheritdoc cref="TakeConstantDamage(int, Vector2, string)"/>
    public void TakeDamage(int damage, Vector2 knockback)
    {
        TakeDamage(damage);
        r2d.AddForce(knockback * knockbackMultiplier, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Heals the character by a specific amount.
    /// </summary>
    /// <param name="healthIncrease"></param>
    public void Heal(int healthIncrease)
    {
        HP += healthIncrease;
    }

    /// <summary>
    /// Revives the character with max HP.
    /// </summary>
    public void Revive()
    {
        HP = maxHP;
        onCharacterRevive.Invoke();
    }

    /// <summary>
    /// Kills the character by dealing maximum damage.
    /// </summary>
    public void Die()
    {
        TakeDamage(int.MaxValue);
    }

    private void PlayerKnockback()
    {
        Vector2 knockbackVector = new Vector2(r2d.velocity.x, r2d.velocity.y).normalized;
        r2d.velocity *= knockbackVector;
    }
    #endregion
    #endregion

    /// <summary>
    /// Legacy function to play audio. Consider using <see
    /// cref="AudioDictionary"/> instead.
    /// </summary>
    /// <param name="clip">The audio clip to play.</param>
    public void PlayClip(AudioClip clip)
    {
        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.PlayOneShot(clip);
    }
}