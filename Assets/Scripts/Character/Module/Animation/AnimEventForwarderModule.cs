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
public class AnimEventForwarderModule : Module
{
    public void CallGunshotEvent(int subweaponIndex)
    {
        Master.onGunshotEvent.Invoke(subweaponIndex);
    }

    public void CallReloadEvent(int subweaponIndex)
    {
        Master.onReloadEvent.Invoke(subweaponIndex);
    }
}