using System.Linq;
using UnityEngine;
using ACPType = UnityEngine.AnimatorControllerParameterType;
using ACP = UnityEngine.AnimatorControllerParameter;
using System.Collections.Generic;

/// <summary>
/// Contains extension methods for Animations and Animators.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public static class AnimationExt
{
    #region Animator
    #region Set Variable
    private static bool ParamMatches(this Animator animator,
        string name, ACPType type)
    {
        return animator.parameters.Any(p => p.name == name && p.type == type);
    }

    /// <summary>
    /// Tries to set the animation parameter with name <paramref name="name"/>.
    /// </summary>
    /// <param name="animator">The animator.</param>
    /// <param name="name">Name of the animation parameter.</param>
    /// <param name="value">Value of the animation parameter.</param>
    /// <returns>True if animator exists and if value is not null and not
    /// empty.</returns>
    public static bool TrySetBool(this Animator animator, string name, bool value)
    {
        if (animator && !string.IsNullOrEmpty(name) &&
            animator.ParamMatches(name, ACPType.Bool))
        {
            animator.SetBool(name, value);
            return true;
        }

        return false;
    }

    /// <inheritdoc cref="TrySetBool(Animator, string, bool)"/>
    public static bool TrySetFloat(this Animator animator, string name, float value)
    {
        if (animator && !string.IsNullOrEmpty(name) &&
            animator.ParamMatches(name, ACPType.Float))
        {
            animator.SetFloat(name, value);
            return true;
        }

        return false;
    }

    /// <inheritdoc cref="TrySetBool(Animator, string, bool)"/>
    public static bool TrySetInt(this Animator animator, string name, int value)
    {
        if (animator && !string.IsNullOrEmpty(name) &&
            animator.ParamMatches(name, ACPType.Int))
        {
            animator.SetInteger(name, value);
            return true;
        }

        return false;
    }

    /// <inheritdoc cref="TrySetBool(Animator, string, bool)"/>
    public static bool TrySetTrigger(this Animator animator, string name)
    {
        if (animator && !string.IsNullOrEmpty(name) &&
            animator.ParamMatches(name, ACPType.Trigger))
        {
            animator.SetTrigger(name);
            return true;
        }

        return false;
    }

    /// <inheritdoc cref="TrySetBool(Animator, string, bool)"/>
    public static bool TryUnsetTrigger(this Animator animator, string name)
    {
        if (animator && !string.IsNullOrEmpty(name) &&
            animator.ParamMatches(name, ACPType.Trigger))
        {
            animator.ResetTrigger(name);
            return true;
        }

        return false;
    }
    #endregion
    
    #region Play/Pause
    public static bool TryPlay(this Animator animator, string state, int layer = -1)
    {
        if (animator)
        {
            animator.Play(state, layer);
            return true;
        }

        return false;
    }
    #endregion
    #endregion
}