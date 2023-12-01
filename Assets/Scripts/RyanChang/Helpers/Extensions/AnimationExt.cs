using UnityEngine;

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
        if (animator)
        {
            if (!string.IsNullOrEmpty(name))
            {
                animator.SetBool(name, value);
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc cref="TrySetBool(Animator, string, bool)"/>
    public static bool TrySetFloat(this Animator animator, string name, float value)
    {
        if (animator)
        {
            if (!string.IsNullOrEmpty(name))
            {
                animator.SetFloat(name, value);
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc cref="TrySetBool(Animator, string, bool)"/>
    public static bool TrySetInt(this Animator animator, string name, int value)
    {
        if (animator)
        {
            if (!string.IsNullOrEmpty(name))
            {
                animator.SetInteger(name, value);
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc cref="TrySetBool(Animator, string, bool)"/>
    public static bool TrySetTrigger(this Animator animator, string name)
    {
        if (animator)
        {
            if (!string.IsNullOrEmpty(name))
            {
                animator.SetTrigger(name);
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc cref="TrySetBool(Animator, string, bool)"/>
    public static bool TryUnsetTrigger(this Animator animator, string name)
    {
        if (animator)
        {
            if (!string.IsNullOrEmpty(name))
            {
                animator.ResetTrigger(name);
                return true;
            }
        }

        return false;
    }
    #endregion
    #endregion
}