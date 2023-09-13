/// <summary>
/// Represents a weapon.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public interface IWeapon
{
    /// <summary>
    /// Name of the weapon.
    /// </summary>
    public string WeaponName { get; }

    /// <summary>
    /// Tries to fire the weapon.
    /// </summary>
    /// <returns>True on success, false otherwise.</returns>
    public bool TryFireWeapon();

    /// <summary>
    /// Tries to reload the weapon.
    /// </summary>
    /// <returns>True on success, false otherwise.</returns>
    public bool TryReloadWeapon();

    /// <summary>
    /// Resets the burst counter, which is the number of bullets fired during
    /// the current burst (aka the number of bullets fired since the mouse was
    /// held down).
    /// </summary>
    public void ResetBurst();

    /// <summary>
    /// Activates/deactivates the game object.
    /// </summary>
    /// <param name="active"></param>
    public void SetActive(bool active);
}