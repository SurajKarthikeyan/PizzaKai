using TMPro;
using UnityEngine;

/// <summary>
/// Controls <see cref="TypewriterText"/>. Allows each <see
/// cref="TypewriterText"/> to be staggered, so that only a few will be active
/// at a time. Starts each one after the other, with configurable amount of
/// delay.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
/// <seealso cref="TypewriterText"/>.
public class TypewriterController : SimultaneousController
{
    #region Variables
    [Tooltip("The delay, in seconds, between each character.")]
    public Range characterDelay = new(0.05f);

    private void Reset()
    {
        characterDelay = new(0.05f);
    }

    protected override SimultaneousControl[] CreateControlsList()
    {
        return GetControls<TypewriterText, TextMeshProUGUI>();
    }    
    #endregion
}