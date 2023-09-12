using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Contains events that can be fired. Events make development much easier by
/// making it possible to communicate information between scripts without the
/// use of references. For more information, see <see
/// cref="https://docs.unity3d.com/Manual/UnityEvents.html"/>.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
public class EventManager : MonoBehaviour
{
    #region Instance
    /// <summary>
    /// The static reference to an EventManager instance.
    /// </summary>
    private static EventManager instance;

    /// <summary>
    /// The static reference to an EventManager instance.
    /// </summary>
    public static EventManager Instance => instance;
    #endregion

    private void Awake()
    {
        this.InstantiateSingleton(ref instance, false);
    }

    #region Event Definitions
    /// <summary>
    /// An event that binds with a singular <see cref="Character"/> parameter.
    /// </summary>
    public class CharacterEvent : UnityEvent<Character> { }

    /// <summary>
    /// Called when a character dies.
    /// </summary>
    public CharacterEvent onCharacterDeath = new();

    /// <summary>
    /// Called when a weapon is switched.
    /// 
    /// <br/>
    /// 
    /// T0 = The weapon's master module that caused the switching.
    /// T1 = The old weapon.
    /// T2 = The new (current) weapon.
    /// </summary>
    public UnityEvent<WeaponMasterModule, IWeapon, IWeapon> onWeaponSwitch = new();

    /// <summary>
    /// Called when we want a camera shake.
    /// 
    /// <br/>
    /// 
    /// T0 = Curve for the camera shake.
    /// </summary>
    public UnityEvent<AnimationCurve> onCameraShake = new();
    #endregion
}