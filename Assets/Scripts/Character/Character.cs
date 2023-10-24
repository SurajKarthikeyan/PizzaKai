using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using static CharacterMovementModule;

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
    [SerializeField]
    [ReadOnly]
    private int hp;

    /// <summary>
    /// The maximum health of the character.
    /// </summary>
    public int maxHP;

    /// <summary>
    /// Animator of the character, used for death animation
    /// </summary>
    public Animator characterAnimator;

    [Tooltip("Animator Parameter - Death")]
    [AnimatorParam(nameof(characterAnimator), AnimatorControllerParameterType.Bool)]
    [SerializeField]
    private string animParamDeath;
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

    [BoxGroup("Common Modules")]
    [Tooltip("The weapon master module, if one exists. Make sure to check if " +
        "null.")]
    [ReadOnly]
    public WeaponMasterModule weaponMasterModule;
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
        set
        {
            hp = value;

            if (hp <= 0)
            {
                hp = 0;
                XnTelemetry.Telemetry_Cloud.CUSTOMEVENTLOG("Death");
                EventManager.Instance.onCharacterDeath.Invoke(this);
                onCharacterDeath.Invoke();
            }
        }
    }

    private List<Module> Modules { get; set; } = new();
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

        IsPlayer = this.HasComponentInChildren(out PlayerControlModule player);

        if (IsPlayer)
        {
            GameManager.Instance.Player = player.Master;
        }

        HP = maxHP;
    }

    private void SetVars()
    {
        transform.position = transform.position.ToVector2();

        GetComponentsInChildren(true, Modules);
        Modules.ForEach(module => module.LinkToMaster(this));

        this.RequireComponent(out r2d);
        this.RequireComponent(out c2d);
        this.RequireComponentInChildren(out sr);

        this.RequireComponent(out flipModule);

        this.HasComponentInChildren(out weaponMasterModule);
    }

    private void Reset()
    {
        hp = 20;
    }
    #endregion

    #region Module Methods
    /// <summary>
    /// Adds <paramref name="module"/> to the list of modules.
    /// Fails if <paramref name="module"/> already exists in that list.
    /// </summary>
    /// <param name="module">The module.</param>
    /// <returns>True if the module was not previously in the list, false otherwise.</returns>
    public bool AddModule(Module module)
    {
        if (Modules.Contains(module))
            return false;

        Modules.Add(module);
        module.LinkToMaster(this);
        return true;
    }

    /// <summary>
    /// Removes <paramref name="module"/> from the list of modules.
    /// Fails if <paramref name="module"/> does not exist in that list.
    /// </summary>
    /// <param name="module">The module.</param>
    /// <returns>True if the module was removed, false otherwise.</returns>
    public bool RemoveModule(Module module)
    {
        if (!Modules.Contains(module))
            return false;

        Modules.Remove(module);
        return true;
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
    /// If the character is dead, revive the character. Otherwise, restore all
    /// health.
    /// </summary>
    public void MaxHeal()
    {
        if (IsDead)
            Revive();
        else
            HP = maxHP;
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

    private void UpdateDeathAnim()
    {
        // Death animation.
        if (!string.IsNullOrWhiteSpace(animParamDeath) && characterAnimator)
        {
            characterAnimator.SetBool(animParamDeath,
                false);
        }
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

    /// <summary>
    /// Uses the <see cref="AudioDictionary"/> to play a sound.
    /// </summary>
    /// <param name="audKey">The key of the audio dictionary.</param>
    public void PlayClip(string audKey)
    {
        if (AudioDictionary.aDict.audioDict.TryGetValue(audKey, out AudioClip clip))
        {
            PlayClip(clip);
        }
        else
        {
            Debug.LogWarning(
                $"Key {audKey} not found in AudioDictionary",
                AudioDictionary.aDict
            );
        }
    }
}