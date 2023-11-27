using UnityEngine;

/// <summary>
/// Allows animator events to be forwarded, so we don't have to rely on raw
/// strings to be used as indexes into a dictionary of sounds.
///
/// <br/>
///
/// The reason why I'm so adamant on not using the current methodology is that I
/// once spend a week debugging code that utilized a similar set up, and the
/// reason for why it wasn't working is because something was changing the
/// values of the string.
///
/// <br/>
///
/// And henceforth, from that very day, I made a solemn oath, to never again do
/// that one specific thing.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
public class AudioForwarderModule : Module
{
    #region Enums
    public enum WeaponClipType
    {
        Shoot,
        Alt,
        Reload
    }
    #endregion

    #region Variables
    private WeaponMasterModule weaponMaster;
    #endregion

    #region Initialization
    protected override void OnLinked(Character old)
    {
        weaponMaster = Master.GetComponentInChildren<WeaponMasterModule>();
    } 
    #endregion

    public void PlayClip(AudioClip clip) => Master.PlayClip(clip);

    public void PlayAudioDictionaryClip(string audKey)
    {
        Master.PlayClip(audKey);
    }

    public void PlayWeaponClip(WeaponClipType weaponClipType)
    {
        if (!weaponMaster)
            throw new System.NullReferenceException("Weapon Master is null");
        else if (!weaponMaster.CurrentWeapon)
            throw new System.NullReferenceException("Current Weapon is null");

        var weapon = weaponMaster.CurrentWeapon;

        switch (weaponClipType)
        {
            case WeaponClipType.Shoot:
                PlayClip(weapon.firingClip);
                break;
            case WeaponClipType.Alt:
                PlayClip(weapon.altingClip ? weapon.altingClip : weapon.firingClip);
                break;
            case WeaponClipType.Reload:
                PlayClip(weapon.reloadClip);
                break;
        }
    }
}